using ArchitecturePractice.Core.ExportReport.ExportModel.PersonalHealthExam;

namespace ArchitecturePractice.Services.ExportReport
{
    internal class PersonalReportMapper
    {
        /// <summary>
        /// 組合GetPersonalHealthExamAsync()與GetReviewReportListAsync()的結果。
        /// </summary>
        /// <param name="healthReportItems">健康檢查報告資料。</param>
        /// <param name="reviewReports">醫師總評報告資料。</param>
        /// <returns>PersonalHealthExamModel。</returns>
        internal PersonalHealthExamModel IntergratePersonalHealthExam(IEnumerable<QueryHealthReportItemModel> healthReportItems, IEnumerable<QueryReviewReportModel> reviewReports)
        {
            var personalHealthExamModel = new PersonalHealthExamModel
            {
                PatientBasicInfo = new PatientBasicInfo
                {
                    套餐名稱 = healthReportItems?.FirstOrDefault()?.套餐名稱,
                    姓名 = healthReportItems?.FirstOrDefault()?.姓名,
                    性別 = healthReportItems?.FirstOrDefault()?.性別,
                    出生日期 = healthReportItems?.FirstOrDefault()?.出生日期,
                    身份證字號 = healthReportItems?.FirstOrDefault()?.身份證字號,
                    病歷號 = healthReportItems?.FirstOrDefault()?.病歷號碼,
                    檢查日期 = healthReportItems?.FirstOrDefault()?.健檢日期?.ToString("yyyy-MM-dd"),
                    前次檢查日期 = healthReportItems?.Where(p => p.DateRank == 2).FirstOrDefault()?.健檢日期?.ToString("yyyy-MM-dd"),
                    電話 = healthReportItems?.FirstOrDefault()?.電話,
                    住址 = healthReportItems?.FirstOrDefault()?.地址
                },
                HealthReportItems = healthReportItems?.Select(item => new HealthReportItemModel
                {
                    檢查類別 = item.細項類別名稱,
                    項目 = item.細項名稱,
                    結果 = item.檢查結果,
                    單位 = item.單位,
                    正常值 = item.正常值_中,
                    檢驗評值 = item.檢驗評值.ToString(),
                    報告類型 = item.報告類型,
                    IsDisplay = item.是否列印細項參考值,
                    DateRank = item.DateRank
                }) ?? Enumerable.Empty<HealthReportItemModel>(),
                OrganReviewReports = reviewReports?.Select(item => new OrganReviewReportModel
                {
                    器官 = item.器官名稱,
                    診斷 = item.診斷,
                    建議 = item.建議
                }) ?? Enumerable.Empty<OrganReviewReportModel>(),
                DoctorInfo = new DoctorInfo
                {
                    負責醫師 = healthReportItems?.FirstOrDefault()?.負責醫師,
                    醫師證號 = healthReportItems?.FirstOrDefault()?.醫師證號,
                    醫師章授權 = healthReportItems?.FirstOrDefault()?.醫師章授權
                }
            };

            return personalHealthExamModel;
        }
    }
}
