using MessagePack;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MessagePackBlog.Common
{
    public sealed class MessagePackMediaTypeFormatter : MediaTypeFormatter
    {
        public MessagePackMediaTypeFormatter(string mediaType)
        {
            this.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(mediaType));
        }
        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            if (readStream.Length == 0)
            {
                if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
                    return Activator.CreateInstance(type);
                else
                    return null;
            }
            else
            {
                object model = await MessagePackSerializer
                   .DeserializeAsync(type, readStream)
                   .ConfigureAwait(false);

                return model;
            }
        }

        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            byte[] bytes = MessagePackSerializer.Serialize(value);

            if (value == null)
            {
                byte[] nullMP = new byte[] { MessagePackCode.Nil };
                using MemoryStream memoryStream = new MemoryStream(nullMP);
                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(writeStream).ConfigureAwait(false);
            }
            else
            {
                using MemoryStream memoryStream = new MemoryStream(bytes);
                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(writeStream).ConfigureAwait(false);
            };
        }
    }
}
