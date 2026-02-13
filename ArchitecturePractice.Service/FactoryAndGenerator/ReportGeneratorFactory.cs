using ArchitecturePractice.Core.ExportReport.ExportModel;
using ArchitecturePractice.Core.ExportReport.ExportModel.PersonalHealthExam;
using ArchitecturePractice.Core.ExportReport.Interface;
using ArchitecturePractice.Core.ExportReport.ReportSetting;
using Microsoft.Extensions.DependencyInjection;

namespace ArchitecturePractice.Services.FactoryAndGenerator
{
    /// <summary>
    /// 實作工廠介面。
    /// </summary>
    public class ReportGeneratorFactory(IServiceProvider serviceProvider) : IReportGeneratorFactory
    {
        // 直接傳入DI容器，在工廠中取得對應的產生器
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        /// <inheritdoc/>
        public IReportGenerator GetReportGenerator(ExportRequestBaseModel model)
        {
            // 第一層Switch: 類別型別配對，檢查傳入Model的型別，true就會自動轉型並賦值給變數
            return model switch
            {
                // 第二層Switch: 比對第一層轉型後的ReportOption與ReportFormat，true就會回傳對應的產生器
                PersonalExportRequest personalExportRequest => (personalExportRequest.ReportOption, personalExportRequest.ReportFormat) switch
                {
                    (PersonalHealthExamOption.不含前次, ReportFormat.PDF) => _serviceProvider.GetRequiredService<PersonalExportGenerator>(),
                    (PersonalHealthExamOption.包含前次, ReportFormat.PDF) => _serviceProvider.GetRequiredService<PersonalComparisonExportGenerator>(),
                    _ => throw new Exception("個人健檢匯出找不到對應的ReportGenerator。")
                },

                _ => throw new Exception("找不到對應的ReportGenerator。")
            };
        }
    }
}
