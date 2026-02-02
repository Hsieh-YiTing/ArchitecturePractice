using ArchitecturePractice.Core.ExportReport.DropdownModel;
using ArchitecturePractice.Core.ExportReport.Interface;
using Dapper;
using System.Data;

namespace ArchitecturePractice.Repositories.ExportReport
{
    public class ExportReportDropdownRepository(IDbConnection dbConnection) : IExportReportDropdownRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;

        /// <summary>
        /// 公司選單ID、公司名稱查詢語法。
        /// </summary>
        private const string GetCompanyItemsQuery = @"
            SELECT 
            CM01 AS Id, 
            CM02 AS CompanyName
            FROM Company
            WHERE CM003 != 1
            ORDER BY CM01";

        /// <summary>
        /// 依據公司ID查詢一級單位ID、一級單位名稱查詢語法。
        /// </summary>
        private const string GetRolesByCompanyIdQuery = @"
            SELECT 
            CR01 AS Id,
            CR02 AS RoleName
            FROM CompanyRole
            WHERE CM01 = @CompanyId 
            AND CR03 = 1
            AND CR003 != 1
            ORDER BY CR01";

        /// <inheritdoc />
        public async Task<IEnumerable<CompanyForDropDownModel>> GetCompanyItemsAsync()
        {
            return await _dbConnection.QueryAsync<CompanyForDropDownModel>(GetCompanyItemsQuery);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RoleForDropDownModel>> GetRoleItemsByCompanyIdAsync(int companyId)
        {
            return await _dbConnection.QueryAsync<RoleForDropDownModel>(GetRolesByCompanyIdQuery, new { CompanyId = companyId });
        }
    }
}
