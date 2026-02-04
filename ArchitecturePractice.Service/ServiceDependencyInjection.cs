using ArchitecturePractice.Core.ExportReport.Interface;
using ArchitecturePractice.Services.ExportReport;
using Microsoft.Extensions.DependencyInjection;

namespace ArchitecturePractice.Services
{
    public static class ServiceDependencyInjection
    {
        /// <summary>
        /// 集中管理Service的註冊。
        /// </summary>
        /// <param name="services">擴充方法。</param>
        /// <returns>IServiceCollection，支援鏈式調用。</returns>
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddScoped<IExportReportService, ExportReportService>();
            return services;
        }
    }
}
