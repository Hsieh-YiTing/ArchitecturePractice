using ArchitecturePractice.Core.ExportReport.ExportModel;

namespace ArchitecturePractice.Core.ExportReport.Interface
{
    /// <summary>
    /// 報表產生器介面。
    /// </summary>
    public interface IReportGenerator
    {
        /// <summary>
        /// 實際產生報表方法。
        /// </summary>
        /// <param name="model">傳入的Model。</param>
        /// <returns>ExportResultModel。</returns>
        Task<ExportResultModel> GenerateReportAsync(ExportRequestBaseModel model);
    }
}
