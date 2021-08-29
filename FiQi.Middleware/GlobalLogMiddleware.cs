using FiQi.Util.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
namespace FiQi.Middleware
{
    /// <summary>
    /// 全局异常处理中间件
    /// </summary>
    public class GlobalLogMiddleware
    {
        private readonly ILogger<GlobalLogMiddleware> _logger;
        private readonly RequestDelegate _next;
        public GlobalLogMiddleware(RequestDelegate next
            , ILogger<GlobalLogMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string uid = context.TraceIdentifier;

            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"请求标识：{uid}");
            sb.AppendLine($"请求头：{FiQiJson.Serialize(context.Request.Headers)}");
            sb.AppendLine($"请求地址：{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}");
            sb.AppendLine($"请求方式：{context.Request.Method}");

            await LogRequest(context.Request, sb);

            var response = context.Response.Body;
            using var newResponse = new MemoryStream();
            context.Response.Body = newResponse;

            var startTime = DateTime.Now;

            await _next(context);

            var takeTime = (DateTime.Now - startTime).TotalMilliseconds;
            sb.AppendLine($"耗时：{takeTime} 毫秒");

            await LogResponse(context.Response, response, sb);
        }
        private async Task LogRequest(HttpRequest request, StringBuilder sb)
        {
            try
            {
                string body = string.Empty;

                if (request.Method.ToLower().Equals("post"))
                {
                    request.EnableBuffering();
                    var requestReader = new StreamReader(request.Body);
                    body = await requestReader.ReadToEndAsync();
                    request.Body.Seek(0, SeekOrigin.Begin);
                }
                else if (request.Method.ToLower().Equals("get"))
                {
                    body = request.QueryString.Value;
                }

                _logger.LogTrace(sb.AppendLine($"请求主体：{body}").ToString());
            }
            catch (Exception e)
            {
                _logger.LogError($"api请求日志错误：{e.Message}", e);
                throw;
            }
        }

        private async Task LogResponse(HttpResponse newResponse, Stream origiBody, StringBuilder sb)
        {
            try
            {
                string body = string.Empty;

                var reader = new StreamReader(newResponse.Body);

                newResponse.Body.Seek(0, SeekOrigin.Begin);
                body = await reader.ReadToEndAsync();
                newResponse.Body.Seek(0, SeekOrigin.Begin);

                _logger.LogTrace(sb.Append($"返回结果：{body}").ToString());

                await newResponse.Body.CopyToAsync(origiBody);
            }
            catch (Exception e)
            {
                _logger.LogError($"api返回日志错误：{e.Message}", e);
                throw;
            }
        }
    }
}
