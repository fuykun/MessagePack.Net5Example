using System;
using System.IO;
using System.Threading.Tasks;
using MessagePack;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace MessagePackBlog.Common
{
    public class MessagePackInputFormatter : InputFormatter
    {
        public MessagePackInputFormatter(params string[] mediaTypes)
        {
            foreach (string mt in mediaTypes)
            {
                SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(mt));
            }
        }
        protected override bool CanReadType(Type type)
        {
            return true;
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                await context.HttpContext.Request.Body.CopyToAsync(memoryStream).ConfigureAwait(false);
                memoryStream.Seek(0, SeekOrigin.Begin);

                if (memoryStream.Length == 0)
                {
                    return InputFormatterResult.NoValue();
                }

                object model = await MessagePackSerializer
                    .DeserializeAsync(context.ModelType, memoryStream)
                    .ConfigureAwait(false);

                return InputFormatterResult.Success(model);
            }
            catch (Exception)
            {
                //TODO : Log
                return InputFormatterResult.Failure();
            }
        }
    }
}
