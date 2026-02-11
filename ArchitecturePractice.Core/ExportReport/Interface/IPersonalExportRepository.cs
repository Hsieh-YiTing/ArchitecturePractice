using ArchitecturePractice.Core.ExportReport.ExportModel.PersonalHealthExam;

namespace ArchitecturePractice.Core.ExportReport.Interface
{
    /// <summary>
    /// 個人健檢報告Repository介面。
    /// </summary>
    public interface IPersonalExportRepository
    {
        /// <summary>
        /// 傳回的結構為多段查詢結果組合後的結構。
        /// </summary>
        /// <param name="model">傳入的Model。</param>
        /// <returns>PersonalHealthExamModel。</returns>
        Task<PersonalHealthExamModel> GetPersonalHealthExamAsync(PersonalExportRequest model);
    }
}
