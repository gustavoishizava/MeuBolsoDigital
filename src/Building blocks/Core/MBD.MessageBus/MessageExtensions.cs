using System;
using System.Text;
using System.Text.Json;

namespace MBD.MessageBus
{
    public static class MessageExtensions
    {
        public static T GetMessage<T>(this ReadOnlyMemory<byte> bytes) where T : class
        {
            var contentArray = bytes.ToArray();
            var contentString = Encoding.UTF8.GetString(contentArray);
            var message = JsonSerializer.Deserialize<T>(contentString, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            return message;
        }
    }
}