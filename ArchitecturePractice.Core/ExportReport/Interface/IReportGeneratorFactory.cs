using ArchitecturePractice.Core.ExportReport.ExportModel;

namespace ArchitecturePractice.Core.ExportReport.Interface
{
    /// <summary>
    /// 報表工廠介面。
    /// </summary>
    public interface IReportGeneratorFactory
    {
        /// <summary>
        /// 取得報表產生器。
        /// </summary>
        /// <param name="model">傳入的Model。</param>
        /// <returns>IReportGenerator介面的產生器。</returns>
        IReportGenerator GetReportGenerator(ExportRequestBaseModel model);
    }
}
