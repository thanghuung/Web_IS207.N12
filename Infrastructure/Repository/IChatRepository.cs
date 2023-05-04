using System.Collections.Generic;
using System.Threading.Tasks;
using WEB2.Models;

namespace WEB2.Infrastructure.Respository
{
    public interface IChatRepository
    {
        Chat GetChat(string id);
        IEnumerable<Chat> GetChats(string userId);
        Task<Message> CreateMessage(string chatId, string message, string userId);
    }
}