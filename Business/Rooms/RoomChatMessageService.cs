using DataAccessLayer;
using Degenesis.Shared.DTOs.Rooms;
using Domain.Rooms;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace Business.Rooms;
public interface IRoomChatMessageService
{
    Task SaveMessageAsync(Guid roomId, string sender, string text);
    Task<List<RoomChatMessageDto>> GetLastMessagesAsync(Guid roomId, int count);
    Task SaveRollResultAsync(Guid roomId, DiceRollResultDto result);
}

public class RoomChatMessageService(ApplicationDbContext context) : IRoomChatMessageService
{
    public async Task SaveMessageAsync(Guid roomId, string sender, string text)
    {
        context.RoomChatMessages.Add(new RoomChatMessage
        {
            IdRoom = roomId,
            Sender = sender,
            Text = text
        });
        await context.SaveChangesAsync();
    }

    public async Task<List<RoomChatMessageDto>> GetLastMessagesAsync(Guid roomId, int count)
    {

        var rawMessages = await context.RoomChatMessages
            .Where(m => m.IdRoom == roomId)
            .OrderByDescending(m => m.SentAtUtc)
            .Take(count)
            .OrderBy(m => m.SentAtUtc)
            .Select(m => new
            {
                m.Sender,
                m.Text,
                m.SentAtUtc,
                m.RollResultJson
            })
            .ToListAsync();

        return [.. rawMessages.Select(m => new RoomChatMessageDto
        {
            Sender = m.Sender,
            Text = m.Text,
            SentAtUtc = m.SentAtUtc,
            RollResult = m.RollResultJson is null
                ? null
                : JsonSerializer.Deserialize<DiceRollResultDto>(m.RollResultJson)
        })];
    }

    public async Task SaveRollResultAsync(Guid roomId, DiceRollResultDto result)
    {
        context.RoomChatMessages.Add(new RoomChatMessage
        {
            IdRoom = roomId,
            Sender = result.Roller,
            Text = result.RollDescription,
            RollResultJson = JsonSerializer.Serialize(result)
        });
        await context.SaveChangesAsync();
    }
}