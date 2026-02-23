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
        public async Task<IEnumerable<QueryHealthReportItemModel>> GetHealthReportItemListAsync(PersonalExportRequest model)
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

        /// <inheritdoc/>
        public async Task<IEnumerable<QueryReviewReportModel>> GetReviewReportListAsync(int? rv01)
        {
            return await _dbConnection.QueryAsync<QueryReviewReportModel>(OrganReviewReportQuery, new { rv01 = rv01 });
        }
    }
}
