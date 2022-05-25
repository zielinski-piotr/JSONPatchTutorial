using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace JSONPatchTutorial.Serialization.Helpers
{
    public static class JsonSerializationHelper
    {
        public static ValueTask<T> DeserializeJsonFromStream<T>(Stream stream, JsonSerializerOptions options = default, CancellationToken cancellationToken = default)
        {
            return stream is not {CanRead: true} json ? default : JsonSerializer.DeserializeAsync<T>(json, options, cancellationToken);
        }

        public static Task<string> StreamToStringAsync(Stream stream)
        {
            if (stream is not {CanRead: true}) return Task.FromResult<string>(null);
            
            using var sr = new StreamReader(stream);
            return sr.ReadToEndAsync();
        }

        public static Task SerializeMessage<TMessage>(Stream stream, TMessage message, JsonSerializerOptions options = default, CancellationToken cancellationToken = default)
        {
            return JsonSerializer.SerializeAsync(stream, message, message.GetType(), options, cancellationToken);
        }
    }
}