using MessagePack;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MessagePackBlog.Common
{
    public sealed class MessagePackOutputFormatter : OutputFormatter
    {
        public MessagePackOutputFormatter(params string[] mediaTypes)
        {
            foreach (string mt in mediaTypes)
            {
                SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(mt));
            }
        }

        protected override bool CanWriteType(Type type)
        {
            return true;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            try
            {
                if (context.Object == null)
                {
                    byte[] nullMP = new byte[] { MessagePackCode.Nil };
                    await context.HttpContext.Response.Body.WriteAsync(nullMP);
                }
                else
                {
                    byte[] bytes = MessagePackSerializer.Serialize(context.Object);

                    await context.HttpContext.Response.Body.WriteAsync(bytes);
                }
            }
            catch (Exception ex)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(ex.Message));
            }
        }
    }
}
