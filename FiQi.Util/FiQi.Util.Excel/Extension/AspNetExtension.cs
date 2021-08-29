using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace FiQi.Util.Excel
{
    public static class AspNetExtension
    {
        public static IServiceCollection AddFiQiExcel(this IServiceCollection services, string excelFolder = "ExcelOutput")
        {
            services.AddSingleton<IFiQiExcel>(new FiQiExcel(excelFolder));
            return services;
        }

        public static IApplicationBuilder UseFiQiExcel(this IApplicationBuilder builder, string FileFolder)
        {
            string FileRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileFolder);
            if (!Directory.Exists(FileRootPath))
            {
                Directory.CreateDirectory(FileRootPath);
            }

            builder.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(FileRootPath),
                RequestPath = $"/{FileFolder}"
            });

            return builder;
        }
    }
}
