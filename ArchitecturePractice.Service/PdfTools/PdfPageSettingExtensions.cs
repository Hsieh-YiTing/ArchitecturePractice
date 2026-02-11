using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ArchitecturePractice.Services.PdfTools
{
    /// <summary>
    /// PDF的頁面設定擴充方法。
    /// </summary>
    internal static class PdfPageSettingExtensions
    {
        /// <summary>
        /// A4大小、邊界1公分、白色背景、預設字型為微軟正黑體、預設字型大小10。
        /// </summary>
        internal static void PageSetup(this PageDescriptor pageDescriptor)
        {
            pageDescriptor.Size(PageSizes.A4);
            pageDescriptor.Margin(1, Unit.Centimetre);
            pageDescriptor.PageColor(Colors.White);
            pageDescriptor.DefaultTextStyle(x => x.FontFamily("Microsoft JhengHei").FontSize(10));
        }
    }
}
