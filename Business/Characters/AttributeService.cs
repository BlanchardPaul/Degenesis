using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Domain.NPCs;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface IAttributeService
{
    Task<Result<List<AttributeDto>>> GetAllAttributesAsync();
    Task<Result<AttributeDto>> GetAttributeByIdAsync(Guid id);
    Task<Result<object>> CreateAttributeAsync(AttributeCreateDto attributeCreate);
    Task<Result<object>> UpdateAttributeAsync(AttributeDto attribute);
    Task<Result<object>> DeleteAttributeAsync(Guid id);
}

public class AttributeService(ApplicationDbContext context, IMapper mapper) : IAttributeService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<AttributeDto>>> GetAllAttributesAsync()
    {
        try
        {
            var attributes = await _context.Attributes.ToListAsync();
            return new Result<List<AttributeDto>> { Value = _mapper.Map<List<AttributeDto>>(attributes) };
        }
        catch (Exception)
        {
            return new Result<List<AttributeDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<AttributeDto>> GetAttributeByIdAsync(Guid id)
    {
        try
        {
            var attribute = await _context.Attributes
                .Include(a => a.Skills)
                .OrderBy(a => a.Name)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (attribute is null)
                return new Result<AttributeDto> { IsError = true,Error = "Attribute not found" };

            return new Result<AttributeDto>{ Value = _mapper.Map<AttributeDto>(attribute) };
        }
        catch (Exception)
        {
            return new Result<AttributeDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateAttributeAsync(AttributeCreateDto attributeCreate)
    {
        try
        {
            var attribute = _mapper.Map<CAttribute>(attributeCreate);
            _context.Attributes.Add(attribute);


            // Link the new attribute to every existing character
            var characters = await _context.Characters.ToListAsync();
            foreach (var character in characters)
            {
                _context.CharacterAttributes.Add(new CharacterAttribute
                {
                    CharacterId = character.Id,
                    AttributeId = attribute.Id,
                    Level = 0
                });
            }

            // Link the new attribute to every existing base NPC
            var npcs = await _context.BaseNPCs.ToListAsync();
            foreach (var npc in npcs)
            {
                _context.BaseNPCAttributes.Add(new BaseNPCAttribute
                {
                    NPCId = npc.Id,
                    AttributeId = attribute.Id,
                    Level = 0
                });
            }

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception) { 
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateAttributeAsync(AttributeDto attribute)
    {
        try
        {
            var existingAttribute = await _context.Attributes
                .Include(a => a.Skills)
                .FirstOrDefaultAsync(a => a.Id == attribute.Id);

            if (existingAttribute is null)
                return new Result<object> { IsError = true, Error = "Attribute not found" };

            // Mapping ignores Skills, we don't modify them here (ecxept for IsFocusOriented)
            _mapper.Map(attribute, existingAttribute);

            // Cascade IsFocusOriented to all linked Skills
            foreach (var skill in existingAttribute.Skills)
            {
                skill.IsFocusOriented = existingAttribute.IsFocusOriented;
            }

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception) {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteAttributeAsync(Guid id)
    {
        try
        {
            var attribute = await _context.Attributes.FirstOrDefaultAsync(a => a.Id == id);
            
            if (attribute is null)
                return new Result<object> { IsError = true, Error = "Attribute not found" };

            _context.Attributes.Remove(attribute);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null};
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}