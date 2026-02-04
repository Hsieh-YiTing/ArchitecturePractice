using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArchitecturePractice.ViewModel
{
    /// <summary>
    /// 公司選單VM。
    /// </summary>
    public class CompanyForDropDownVm
    {
        /// <summary>
        /// 公司選單項目集合。
        /// </summary>
        public required IEnumerable<SelectListItem> CompanyItemList { get; set; }
    }
}
