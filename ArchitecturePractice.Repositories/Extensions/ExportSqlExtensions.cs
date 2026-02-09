using ArchitecturePractice.Core.ExportReport.ExportModel;
using Dapper;
using System.Text;

namespace ArchitecturePractice.Repositories.Extensions
{
    /// <summary>
    /// 共用的匯出欄位Model的擴充方法。
    /// </summary>
    internal static class ExportSqlExtensions
    {
        /// <summary>
        /// 匯出共用欄位的SQL條件語法。
        /// </summary>
        /// <param name="model">擴充方法。</param>
        /// <param name="sb">建構SQL字串容器。</param>
        /// <param name="param">Dapper提供的動態參數容器。</param>
        public static void AddBaseConditions(this ExportRequestBaseModel model, StringBuilder sb, DynamicParameters param)
        {
            if (model.ExamineStartDate != default && model.ExamineEndDate != default)
            {
                sb.Append("AND (r.RV02 >= @ExamineStartDate AND r.RV02 <= @ExamineEndDate)");
                param.Add("@ExamineStartDate", model.ExamineStartDate);
                param.Add("@ExamineEndDate", model.ExamineEndDate);
            }

            if (model.SelectedCompanyId is not null)
            {
                if (model.SelectedCompanyId == 0)
                {
                    sb.Append(" AND RV05 = 0");
                }
                else
                {
                    sb.Append(" AND RV05 = 1");
                    sb.Append(" AND (r.CM01 = @companyValue)");
                    param.Add("@companyValue", model.SelectedCompanyId);
                }
            }

            if (model.SelectedRoleId is not null)
            {
                sb.Append(" AND (r.RV15 = @roleValue)");
                param.Add("@roleValue", model.SelectedRoleId);
            }
        }
    }
}
