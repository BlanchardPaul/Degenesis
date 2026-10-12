using DataAccessLayer;
using Degenesis.Shared.DTOs.Rooms;
using Domain.Rooms;
using Microsoft.EntityFrameworkCore;

namespace Business.Rooms;
public interface IRoomChatMessageService
{
    Task SaveMessageAsync(Guid roomId, string sender, string text);
    Task<List<RoomChatMessageDto>> GetLastMessagesAsync(Guid roomId, int count);
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
        return await context.RoomChatMessages
            .Where(m => m.IdRoom == roomId)
            .OrderByDescending(m => m.SentAtUtc)
            .Take(count)
            .OrderBy(m => m.SentAtUtc)
            .Select(m => new RoomChatMessageDto
            {
                Sender = m.Sender,
                Text = m.Text,
                SentAtUtc = m.SentAtUtc
            })
            .ToListAsync();
    }
}