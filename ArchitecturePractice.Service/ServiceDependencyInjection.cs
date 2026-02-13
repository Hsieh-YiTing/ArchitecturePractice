using ArchitecturePractice.Core.ExportReport.Interface;
using ArchitecturePractice.Services.ExportReport;
using ArchitecturePractice.Services.FactoryAndGenerator;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

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
            services.AddScoped<IReportGeneratorFactory, ReportGeneratorFactory>();

            // PDF授權以及產生器
            QuestPDF.Settings.License = LicenseType.Community;
            services.AddScoped<PersonalExportGenerator>();
            services.AddScoped<PersonalComparisonExportGenerator>();
            return services;
        }
    }
}
