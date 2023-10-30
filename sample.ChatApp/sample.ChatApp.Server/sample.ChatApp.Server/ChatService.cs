using sample.ChatApp.Shared.Services;
using MagicOnion;
using MagicOnion.Server;
using MessagePack;
using Microsoft.Extensions.Logging;

namespace sample.ChatApp.Server
{
    public class ChatService : ServiceBase<IChatService>, IChatService
    {
        private readonly ILogger _logger;

        public ChatService(ILogger<ChatService> logger)
        {
            _logger = logger;
        }

        public UnaryResult GenerateException(string message)
        {
            Console.WriteLine($"[ChatService][GenerateException] called.");
            throw new System.NotImplementedException();
        }

        public UnaryResult SendReportAsync(string message)
        {
            //_logger.LogDebug($"{message}");
            Console.WriteLine($"[ChatService][SendReportAsync] {message}");
            return UnaryResult.CompletedResult;
        }
    }
}
