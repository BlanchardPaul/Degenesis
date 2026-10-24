using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Domain.Rooms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

namespace Business.Characters;
public interface ICultService
{
    Task<Result<CultDto>> GetCultByIdAsync(Guid id);
    Task<Result<List<CultDto>>> GetAllCultsAsync();
    Task<Result<object>> CreateCultAsync(CultCreateDto cultCreate);
    Task<Result<object>> UpdateCultAsync(CultDto cult);
    Task<Result<object>> SetCultImageAsync(Guid cultId, IFormFile file);
    Task<Result<object>> RemoveCultImageAsync(Guid cultId);
    Task<Result<object>> DeleteCultAsync(Guid id);
}

public class CultService(ApplicationDbContext context, IMapper mapper,
    IWebHostEnvironment env) : ICultService
{
    private readonly string _cultImagesFolder = Path.Combine(env.WebRootPath!, "cult-images");
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    private const int RequiredImageWidth = 1200;
    private const int RequiredImageHeight = 600;
    private const long MaxImageSizeBytes = 2 * 1024 * 1024;
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".webp"];

    public async Task<Result<CultDto>> GetCultByIdAsync(Guid id)
    {
        try
        {
            var cult = await _context.Cults
                .Include(c => c.BonusSkills)
                .OrderBy(c => c.Name)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (cult is null)
                return new Result<CultDto> { IsError = true, Error = "Cult not found" };

            return new Result<CultDto> { Value = _mapper.Map<CultDto>(cult) };
        }
        catch (Exception)
        {
            return new Result<CultDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<List<CultDto>>> GetAllCultsAsync()
    {
        try
        {
            var cults = await _context.Cults
                .Include(c => c.BonusSkills)
                .ToListAsync();
            return new Result<List<CultDto>> { Value = _mapper.Map<List<CultDto>>(cults) };
        }
        catch (Exception)
        {
            return new Result<List<CultDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateCultAsync(CultCreateDto cultCreate)
    {
        try
        {
            var cult = _mapper.Map<Cult>(cultCreate);

            foreach (var skillDto in cultCreate.BonusSkills)
            {
                var existingSkill = await _context.Skills.FindAsync(skillDto.Id);
                if (existingSkill is null)
                    return new Result<object> { IsError = true, Error = "Skill not found" };
                cult.BonusSkills.Add(existingSkill);
            }

            // Propagate to existing rooms for the playergroup relationships
            var roomIds = await _context.Rooms.Select(r => r.Id).ToListAsync();
            _context.RoomCultRelationships.AddRange(roomIds.Select(roomId => new RoomCultRelationship
            {
                IdCult = cult.Id,
                IdRoom = roomId,
                IsDiscovered = false,
                Relationship = 0
            }));

            _context.Cults.Add(cult);
            await _context.SaveChangesAsync();

            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<object>> UpdateCultAsync(CultDto cultDto)
    {
        try
        {
            var existingCult = await _context.Cults
                .Include(c => c.BonusSkills)
                .FirstOrDefaultAsync(c => c.Id == cultDto.Id);
            if (existingCult is null)
                return new Result<object> { IsError = true, Error = "Cult not found" };

            _mapper.Map(cultDto, existingCult);

            existingCult.BonusSkills.Clear();
            foreach (var skillDto in cultDto.BonusSkills)
            {
                var skill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Id == skillDto.Id);
                if (skill is null)
                    return new Result<object> { IsError = true, Error = "Skill not found" };

                existingCult.BonusSkills.Add(skill);
            }

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
        
    }

    public async Task<Result<object>> SetCultImageAsync(Guid cultId, IFormFile file)
    {
        try
        {
            var cult = await _context.Cults.FindAsync(cultId);
            if (cult is null)
                return new Result<object> { IsError = true, Error = "Cult not found" };

            if (file.Length == 0)
                return new Result<object> { IsError = true, Error = "No file provided" };
            if (file.Length > MaxImageSizeBytes)
                return new Result<object> { IsError = true, Error = "File too large (max 2 MB)" };

            // Check file extension
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedImageExtensions.Contains(extension))
                return new Result<object> { IsError = true, Error = "Only JPG, PNG or WEBP allowed" };

            // Pass the image as memory stream
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            // Validate image dimensions
            var dimensionError = ValidateImageDimensions(ms);
            if (dimensionError is not null)
                return dimensionError;

            var fileName = $"{cultId}{extension}";
            var fullPath = Path.Combine(_cultImagesFolder, fileName);

            // Remove a previous image with a different extension (cultId.png vs cultId.jpg)
            foreach (var old in Directory.GetFiles(_cultImagesFolder, $"{cultId}.*"))
                if (!string.Equals(old, fullPath, StringComparison.OrdinalIgnoreCase))
                    File.Delete(old);

            ms.Position = 0;

            // Create file in the back
            await using (var fs = File.Create(fullPath))
                await ms.CopyToAsync(fs);

            cult.ImageName = fileName;
            await _context.SaveChangesAsync();

            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> RemoveCultImageAsync(Guid cultId)
    {
        try
        {
            var cult = await _context.Cults.FindAsync(cultId);
            if (cult is null)
                return new Result<object> { IsError = true, Error = "Cult not found" };

            if (cult.ImageName is not null)
            {
                try
                {
                    var fullPath = Path.Combine(_cultImagesFolder, cult.ImageName);
                    if (File.Exists(fullPath))
                        File.Delete(fullPath);
                }
                catch { /* Don't break everything if file not found, if not found it's because it's already deleted */}
            }

            cult.ImageName = null;
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteCultAsync(Guid id)
    {
        try
        {
            var cult = await _context.Cults
                .Include(c => c.BonusSkills)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (cult is null)
                return new Result<object> { IsError = true, Error = "Cult not found" };

            if (cult.ImageName is not null)
            {
                try
                {
                    var fullPath = Path.Combine(_cultImagesFolder, cult.ImageName);
                    if (File.Exists(fullPath))
                        File.Delete(fullPath);
                }
                catch { /* ignore file system errors */ }
            }

            _context.Cults.Remove(cult);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    // Helper to validate Cult image format and size
    private static Result<object>? ValidateImageDimensions(MemoryStream ms)
    {
        try
        {
            ms.Position = 0;
            using var data = SKData.Create(ms);
            using var codec = SKCodec.Create(data);

            if (codec is null)
                return new Result<object> { IsError = true, Error = "Invalid image file" };

            var width = codec.Info.Width;
            var height = codec.Info.Height;

            if (width != RequiredImageWidth || height != RequiredImageHeight)
                return new Result<object>
                {
                    IsError = true,
                    Error = $"Image must be exactly {RequiredImageWidth}x{RequiredImageHeight} px (received {width}x{height})"
                };

            return null;
        }
        catch
        {
            return new Result<object> { IsError = true, Error = "Invalid image file" };
        }
    }
}
