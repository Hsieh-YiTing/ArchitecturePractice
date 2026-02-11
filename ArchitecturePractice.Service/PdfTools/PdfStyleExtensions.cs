using QuestPDF.Elements.Table;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ArchitecturePractice.Services.PdfTools
{
    /// <summary>
    /// PDF樣式的擴充方法。
    /// </summary>
    internal static class PdfStyleExtensions
    {
        /// <summary>
        /// 標題文字樣式。
        /// </summary>
        internal static readonly TextStyle TitleStyle = TextStyle.Default.FontSize(20).Bold();

        /// <summary>
        /// 表格Key文字樣式。
        /// </summary>
        internal static readonly TextStyle KeyStyle = TextStyle.Default.Bold();

        /// <summary>
        /// 檢查項目的標題文字樣式。
        /// </summary>
        internal static readonly TextStyle TableTitleStyle = TextStyle.Default.FontSize(14).Bold();

        /// <summary>
        /// 異常值文字樣式。
        /// </summary>
        internal static readonly TextStyle AbnormalValueStyle = TextStyle.Default.FontColor(Colors.Red.Medium);

        /// <summary>
        /// 印章文字共同樣式。
        /// </summary>
        private static readonly TextStyle SealBase = TextStyle.Default.FontColor(Colors.Red.Darken3);

        /// <summary>
        /// 印章中的"醫師"文字樣式。
        /// </summary>
        internal static readonly TextStyle DoctorSealStyle = SealBase.FontSize(10);

        /// <summary>
        /// 印章中的"醫師姓名"文字樣式。
        /// </summary>
        internal static readonly TextStyle DoctorSealNameStyle = SealBase.FontSize(12);

        /// <summary>
        /// 標題容器樣式。
        /// </summary>
        internal static IContainer TitleContainerStyle(this IContainer container)
        {
            return container.AlignCenter();
        }

        /// <summary>
        /// 病患資訊以及表格共用的容器樣式。
        /// </summary>
        private static IContainer BaseContainerStyle(this IContainer container)
        {
            return container.Border(1)
                            .BorderColor(Colors.Black)
                            .AlignMiddle()
                            .Padding(3);
        }

        /// <summary>
        /// 病患資訊Key容器樣式。
        /// </summary>
        internal static IContainer PatientInfoKeyContainerStyle(this IContainer container)
        {
            return container.BaseContainerStyle()
                            .Background(Colors.Grey.Lighten4)
                            .AlignCenter();
        }

        /// <summary>
        /// 病患資訊Value容器樣式。
        /// </summary>
        internal static IContainer PatientInfoValueContainerStyle(this IContainer container)
        {
            return container.BaseContainerStyle()
                            .AlignCenter();
        }

        /// <summary>
        /// 表格Key容器樣式。
        /// </summary>
        internal static IContainer TableKeyContainerStyle(this IContainer container)
        {
            return container.BaseContainerStyle()
                            .Background(Colors.Grey.Lighten4)
                            .AlignLeft();
        }

        /// <summary>
        /// 表格Value容器樣式。
        /// </summary>
        internal static IContainer TableValueContainerStyle(this IContainer container)
        {
            return container.BaseContainerStyle()
                            .AlignLeft();
        }

        /// <summary>
        /// 表格標題容器樣式。
        /// </summary>
        internal static IContainer TableTitleContainerStyle(this ITableCellContainer container)
        {
            return container.PaddingBottom(3);
        }

        /// <summary>
        /// 檢查項目的容器樣式。
        /// </summary>
        internal static IContainer TableItemContainerStyle(this IContainer container)
        {
            return container.PaddingTop(10);
        }

        /// <summary>
        /// 醫生資訊表格容器樣式。
        /// </summary>
        internal static IContainer DoctorInfoContainerStyle(this IContainer container)
        {
            return container.BaseContainerStyle()
                            .AlignLeft();
        }

        /// <summary>
        /// 醫師章容器樣式。
        /// </summary>
        internal static IContainer DoctorSealContainerStyle(this IContainer container)
        {
            return container.PaddingLeft(5)
                            .Border(1)
                            .CornerRadius(5)
                            .BorderColor(Colors.Red.Darken3)
                            .AlignMiddle()
                            .AlignCenter();
        }

        /// <summary>
        /// 醫院聯絡資訊容器樣式。
        /// </summary>
        internal static IContainer ContactInfoContainerStyle(this IContainer container)
        {
            return container.PaddingTop(10)
                            .AlignMiddle()
                            .AlignCenter();
        }
    }
}
