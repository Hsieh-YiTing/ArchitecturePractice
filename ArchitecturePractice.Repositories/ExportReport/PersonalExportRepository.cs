using ArchitecturePractice.Common.Helper;
using ArchitecturePractice.Core.ExportReport.ExportModel.PersonalHealthExam;
using ArchitecturePractice.Core.ExportReport.Interface;
using ArchitecturePractice.Core.ExportReport.ReportSetting;
using ArchitecturePractice.Repositories.Extensions;
using Dapper;
using System.Data;
using System.Text;

namespace ArchitecturePractice.Repositories.ExportReport
{
    public class PersonalExportRepository(IDbConnection dbConnection) : IPersonalExportRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;

        /// <summary>
        /// 醫師總評查詢語法。
        /// </summary>
        private const string OrganReviewReportQuery = @"
            SELECT
	        rr.IC01 AS 細項類別序號,
	        io.IO02 AS 器官名稱,
	        rr.RR02 AS 診斷,
	        rr.RR03 AS 建議
            FROM ReviewReport rr
            JOIN ItemOrgan io ON rr.IC01 = io.IO01
            WHERE rr.RR003 = 0 AND rr.RV01 = @rv01";

        /// <inheritdoc/>
        public async Task<PersonalHealthExamModel> GetPersonalHealthExamAsync(PersonalExportRequest model)
        {
            // 1. 查詢健檢報告細項資料
            var healthReportItemList = await GetHealthReportItemListAsync(model);

            // 2. 取得最新健檢報告的預約序號
            var rv01 = healthReportItemList
                .Where(p => p.DateRank == 1)
                .FirstOrDefault()?.預約序號;

            // 3. 查詢醫師總評報告資料
            var reviewReportList = await GetReviewReportListAsync(rv01);

            // 4. 組合並回傳結果
            var personalHealthExamModel = IntergratePersonalHealthExam(healthReportItemList, reviewReportList);

            return IntergratePersonalHealthExam(healthReportItemList, reviewReportList);
        }

        /// <summary>
        /// 查詢個人健檢報告所需資料(套餐、病患資訊、細項、醫師)。
        /// </summary>
        /// <param name="model">傳入的Model。</param>
        /// <returns>IEnumerable<QueryHealthReportItemModel>。</returns>
        private async Task<IEnumerable<QueryHealthReportItemModel>> GetHealthReportItemListAsync(PersonalExportRequest model)
        {
            // 讀取嵌入式SQL查詢語法
            string baseQuery = EmbeddedResourceHelper.GetSqlContent<PersonalExportRepository>("ArchitecturePractice.Repositories.SqlQueries", "PersonalHealthExamQuery.sql");

            // 依據Model內容，組合查詢條件
            var cteWhereQuerySb = new StringBuilder();
            var param = new DynamicParameters();

            // 使用擴充方法，加入匯出的共用條件
            model.AddBaseConditions(cteWhereQuerySb, param);

            if (!string.IsNullOrEmpty(model.PackageCode))
            {
                cteWhereQuerySb.Append(" AND (PD03 = @packageCode)");
                param.Add("@packageCode", model.PackageCode);
            }

            if (!string.IsNullOrEmpty(model.IdOrChartNumber))
            {
                cteWhereQuerySb.Append(" AND (r.PAT03 = @idOrChartNumber OR r.PAT07 = @idOrChartNumber)");
                param.Add("@idOrChartNumber", model.IdOrChartNumber);
            }

            string reportTypeQuery = model.ReportOption switch
            {
                PersonalHealthExamOption.不含前次 => "WHERE amd.DateRank = 1",
                PersonalHealthExamOption.包含前次 => "WHERE amd.DateRank <= 2",
                _ => throw new ArgumentException($"個人健檢報告不支援類型: {model.ReportOption}。"),
            };
            string healthReportItemQuery = baseQuery.Replace("/* CTE_WHERE_CLAUSE */", cteWhereQuerySb.ToString())
                                                    .Replace("/* WHERE_CLAUSE_BY_EXPORT_TYPE */", reportTypeQuery);

            // 送出查詢後返回結果
            return await _dbConnection.QueryAsync<QueryHealthReportItemModel>(healthReportItemQuery, param);
        }

        /// <summary>
        /// 查詢醫師總評報告資料。
        /// </summary>
        /// <param name="rv01">預約序號。</param>
        /// <returns>IEnumerable<QueryReviewReportModel>。</returns>
        private async Task<IEnumerable<QueryReviewReportModel>> GetReviewReportListAsync(int? rv01)
        {
            return await _dbConnection.QueryAsync<QueryReviewReportModel>(OrganReviewReportQuery, new { rv01 = rv01 });
        }

        /// <summary>
        /// 組合GetPersonalHealthExamAsync()與GetReviewReportListAsync()的結果。
        /// </summary>
        /// <param name="healthReportItems">健康檢查報告資料。</param>
        /// <param name="reviewReports">醫師總評報告資料。</param>
        /// <returns>PersonalHealthExamModel。</returns>
        private PersonalHealthExamModel IntergratePersonalHealthExam(IEnumerable<QueryHealthReportItemModel> healthReportItems, IEnumerable<QueryReviewReportModel> reviewReports)
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
