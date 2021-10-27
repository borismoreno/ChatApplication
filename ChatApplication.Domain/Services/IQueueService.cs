using ChatApplication.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApplication.Domain.Services
{
    public interface IQueueService
    {
        void SendMessage(string message, bool incoming = false);

        List<Response> ReceieveMessage();
    }
}
