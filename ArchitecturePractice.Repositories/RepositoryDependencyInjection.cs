using ArchitecturePractice.Core.ExportReport.Interface;
using ArchitecturePractice.Repositories.ExportReport;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace ArchitecturePractice.Repositories
{
    public static class RepositoryDependencyInjection
    {
        /// <summary>
        /// 集中管理Repository的註冊。
        /// </summary>
        /// <param name="services">擴充方法。</param>
        /// <returns>IServiceCollection，支援鏈式調用。</returns>
        public static IServiceCollection AddRepository(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));
            services.AddScoped<IExportReportDropdownRepository, ExportReportDropdownRepository>();
            services.AddScoped<IPersonalExportRepository, PersonalExportRepository>();
            return services;
        }
    }
}
