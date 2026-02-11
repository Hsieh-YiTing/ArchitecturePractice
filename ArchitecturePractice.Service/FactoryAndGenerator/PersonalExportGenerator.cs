using ArchitecturePractice.Core.ExportReport.ExportModel;
using ArchitecturePractice.Core.ExportReport.ExportModel.PersonalHealthExam;
using ArchitecturePractice.Core.ExportReport.Interface;
using ArchitecturePractice.Core.ExportReport.ReportSetting;
using ArchitecturePractice.Services.PdfDocuments;
using Microsoft.Extensions.Options;
using QuestPDF.Fluent;

namespace ArchitecturePractice.Services.FactoryAndGenerator
{
    /// <inheritdoc/>
    public class PersonalExportGenerator(IPersonalExportRepository repository, IOptions<ReportConfig> reportConfig) : IReportGenerator
    {
        private readonly IPersonalExportRepository _repository = repository;

        private readonly ReportConfig _reportConfig = reportConfig.Value;

        public async Task<ExportResultModel> GenerateReportAsync(ExportRequestBaseModel model)
        {
            // 判斷傳入的Model是否為ExportPersonalHealthExamModel，並賦值給變數
            if (model is PersonalExportRequest requestModel)
            {
                // 使用Repository取得個人健康檢查資料
                var personalHealthExam = await _repository.GetPersonalHealthExamAsync(requestModel);

                // 建立個人健康檢查文件並產生PDF
                var document = new PersonalHealthExamDocument(personalHealthExam, _reportConfig);

                // 回傳匯出結果
                var report = new ExportResultModel
                {
                    FileContent = document.GeneratePdf(),
                    FileName = "個人健檢報告匯出(包含前次)",
                    ContentType = "application/pdf"
                };

                return report;
            }

            throw new ArgumentException("錯誤的匯出Model。");
        }
    }
}
