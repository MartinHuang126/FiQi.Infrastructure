using FiQi.Util.Json;
using FiQi.Util.Result;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace FiQi.Middleware
{
    /// <summary>
    /// 全局异常处理中间件
    /// </summary>
    public class GlobalErrorMiddleware
    {
        private readonly ILogger<GlobalErrorMiddleware> _logger;
        private readonly RequestDelegate _next;
        public GlobalErrorMiddleware(RequestDelegate next
            , ILogger<GlobalErrorMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(FiQiException ex)
            {
                await HandleFiQiExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleFiQiExceptionAsync(HttpContext context, FiQiException ex)
        {
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine($"请求地址：{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}");
            builder.AppendLine($"异常信息：{ex.Message}");
            builder.AppendLine($"异常对象：{ex.Source}");
            builder.AppendLine($"调用堆栈：\n" + ex.StackTrace.Trim());
            builder.AppendLine($"触发方法：{ex.TargetSite}");
            builder.AppendLine($"内部异常：{(ex.InnerException != null ? ex.InnerException.ToString() : string.Empty)}");
            builder.AppendLine($"自定义异常：code:{ex.Result.Code} message:{ex.Result.Message}");
            builder.AppendLine();

            _logger.LogError(builder.ToString());

            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync(FiQiJson.Serialize(ex.Result));
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine($"请求地址：{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}");
            builder.AppendLine($"异常信息：{ex.Message}");
            builder.AppendLine($"异常对象：{ex.Source}");
            builder.AppendLine($"调用堆栈：\n" + ex.StackTrace.Trim());
            builder.AppendLine($"触发方法：{ex.TargetSite}");
            builder.AppendLine($"内部异常：{(ex.InnerException != null ? ex.InnerException.ToString() : string.Empty)}");
            builder.AppendLine();

            _logger.LogError(builder.ToString());

            //context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync(FiQiJson.Serialize(IFiQiResult.Error(ex.Message)));
        }
    }
}
