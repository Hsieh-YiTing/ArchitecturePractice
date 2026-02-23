using ArchitecturePractice.Core.ExportReport.ExportModel.PersonalHealthExam;

namespace ArchitecturePractice.Core.ExportReport.Interface
{
    /// <summary>
    /// 個人健檢報告Repository介面。
    /// </summary>
    public interface IPersonalExportRepository
    {
        /// <summary>
        /// 查詢個人健檢報告所需資料(套餐、病患資訊、細項、醫師)。
        /// </summary>
        /// <param name="model">傳入的Model。</param>
        /// <returns>IEnumerable<QueryHealthReportItemModel>。</returns>
        Task<IEnumerable<QueryHealthReportItemModel>> GetHealthReportItemListAsync(PersonalExportRequest model);

        /// <summary>
        /// 查詢醫師總評報告資料。
        /// </summary>
        /// <param name="rv01">預約序號。</param>
        /// <returns>IEnumerable<QueryReviewReportModel>。</returns>
        Task<IEnumerable<QueryReviewReportModel>> GetReviewReportListAsync(int? rv01);
    }
}
