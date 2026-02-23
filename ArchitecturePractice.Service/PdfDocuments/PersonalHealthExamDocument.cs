using ArchitecturePractice.Core.ExportReport.ReportSetting;
using ArchitecturePractice.Services.PdfTools;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ArchitecturePractice.Services.PdfDocuments
{
    /// <summary>
    /// 實作IDocument介面，以物件方式建構文件，組成個人健康檢查報告。
    /// </summary>
    internal class PersonalHealthExamDocument(PersonalHealthExamModel model, ReportConfig reportConfig) : IDocument
    {
        private readonly PersonalHealthExamModel _model = model;

        private readonly ReportConfig _reportConfig = reportConfig;

        #region 各PDF區塊的Header與欄位定義
        // 定義好頁首所需要的欄位清單，Value使用Func取值，使用時需要傳入PatientBasicInfo物件
        private readonly List<(string key, Func<PatientBasicInfo, string> value)> _headerPatientInfoItemList =
        [
            ( "姓名", m => m.姓名 ?? ""),
            ( "性別", m => m.性別 ?? ""),
            ( "出生日期", m => m.出生日期 ?? ""),
            ( "身份證字號", m => m.身份證字號 ?? ""),
            ( "病歷號", m => m.病歷號 ?? ""),
        ];

        // 定義好內容區塊所需要的病患基本資料欄位清單
        private readonly List<(string key, Func<PatientBasicInfo, string> value)> _contentPatientInfoList =
        [
            ( "檢查日期", m => m.檢查日期 ?? ""),
            ( "電話", m => m.電話 ?? ""),
            ( "住址", m => m.住址 ?? "")
        ];

        // 數字類型檢查項目的表格Header
        private readonly string[] _numericItemHeaderSet = ["項目", "檢查結果", "單位", "正常值"];

        // 非數字類型檢查項目的表格Header
        private readonly string[] _nonNumericItemHeaderSet = ["項目", "檢查結果", "單位"];

        // 醫師總評表格Header
        private readonly string[] _doctorReviewHeaderSet = ["器官", "診斷", "建議"];
        #endregion

        /// <summary>
        /// PDF文件組成。
        /// </summary>
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                // 頁面基本設定
                page.PageSetup();

                // 頁首設置
                page.Header()
                    .Element(ComposeHeader);

                // 報告內容
                page.Content()
                    .Element(ComposeContent);
            });
        }

        #region --- 區塊 1: 頁首 ---
        /// <summary>
        /// PDF頁首設計。
        /// </summary>
        private void ComposeHeader(IContainer container)
        {
            container.Layers(layers =>
            {
                // 第一層: 頁首內容
                layers.PrimaryLayer().Column(col =>
                {
                    col.Item()
                       .TitleContainerStyle()
                       .Text(_reportConfig.Organization?.HospitalName)
                       .Style(PdfStyleExtensions.TitleStyle);

                    col.Item()
                       .TitleContainerStyle()
                       .Text(_model.PatientBasicInfo.套餐名稱)
                       .Style(PdfStyleExtensions.TitleStyle);

                    col.Item()
                       .PaddingTop(10)
                       .Element(ComposePatientInfoHeader);
                });

                // 第二層：頁碼
                layers.Layer()
                      .AlignTop()
                      .AlignRight()
                      .Text(text =>
                      {
                          text.CurrentPageNumber();
                          text.Span("/");
                          text.TotalPages();
                      });
            });
        }

        /// <summary>
        /// 頁首內的病患資料。
        /// </summary>
        private void ComposePatientInfoHeader(IContainer container)
        {
            container.Table(table =>
            {
                // 定義出欄位寬度，共5組欄位，每組包含Key與Value
                table.ColumnsDefinition(col =>
                {
                    // 第1組: 姓名
                    col.ConstantColumn(40);
                    col.RelativeColumn(1);

                    // 第2組: 性別
                    col.ConstantColumn(40);
                    col.ConstantColumn(20);

                    // 第3組: 出生日期
                    col.ConstantColumn(60);
                    col.RelativeColumn(2);

                    // 第4組: 身份證字號
                    col.ConstantColumn(70);
                    col.RelativeColumn(2);

                    // 第5組: 病歷號
                    col.ConstantColumn(50);
                    col.RelativeColumn(2);
                });

                // 每組欄位內容寫入，使用擴充方法與自定義文字樣式設計Key與Value樣式
                foreach (var (Key, Value) in _headerPatientInfoItemList)
                {
                    table.Cell()
                         .PatientInfoKeyContainerStyle()
                         .Text(Key)
                         .Style(PdfStyleExtensions.KeyStyle);

                    table.Cell()
                         .PatientInfoValueContainerStyle()
                         .Text(Value(_model.PatientBasicInfo));
                }
            });
        }
        #endregion

        #region --- 區塊 2: 表格內容(一): 病患資訊、檢查項目 ---
        /// <summary>
        /// PDF內容設計。
        /// </summary>
        private void ComposeContent(IContainer container)
        {
            container.Column(col =>
            {
                // 1. 病人基本資料 (只顯示一次)
                col.Item()
                   .Element(ComposePatientInfo);

                // 2. 檢查項目
                col.Item()
                   .Element(ComposeInspectionItems);

                // 3. 醫師總評
                col.Item()
                   .TableItemContainerStyle()
                   .Element(ComposeDoctorReview);

                // 4. 頁尾資訊
                col.Item()
                   .TableItemContainerStyle()
                   .Element(ComposeFooter);
            });
        }

        /// <summary>
        /// 病人基本資料(只顯示一次)。
        /// </summary>
        private void ComposePatientInfo(IContainer container)
        {
            container.Table(table =>
            {
                // 定義出欄位寬度，共3組欄位，每組包含Key與Value
                table.ColumnsDefinition(col =>
                {
                    // 第一組: 檢查日期
                    col.ConstantColumn(60);
                    col.RelativeColumn(1);

                    // 第二組: 電話
                    col.ConstantColumn(40);
                    col.RelativeColumn(1);

                    // 第三組: 住址
                    col.ConstantColumn(40);
                    col.RelativeColumn(2);
                });

                // 每組欄位內容寫入，使用擴充方法與自定義文字樣式設計Key與Value樣式
                foreach (var (key, value) in _contentPatientInfoList)
                {
                    table.Cell()
                         .PatientInfoKeyContainerStyle()
                         .Text(key)
                         .Style(PdfStyleExtensions.KeyStyle);

                    table.Cell()
                         .PatientInfoValueContainerStyle()
                         .Text(value(_model.PatientBasicInfo));
                }
            });
        }

        /// <summary>
        /// 病患檢查項目，分為數字報告與非數字報告表格。
        /// </summary>
        private void ComposeInspectionItems(IContainer container)
        {
            // 根據檢查類別進行分組
            var groupedReportItems = _model.HealthReportItems.GroupBy(i => i.檢查類別 ?? "未分類");

            container.Column(col =>
            {
                foreach (var categoryGroup in groupedReportItems)
                {
                    // 將分組後的類別，再分為數字報告與非數字報告兩種
                    var numericItems = categoryGroup.Where(i => i.報告類型 == 1).ToList();
                    var nonNumericItems = categoryGroup.Where(i => i.報告類型 != 1).ToList();

                    // 數字報告表格
                    if (numericItems.Count != 0)
                    {
                        col.Item()
                           .TableItemContainerStyle()
                           .Element(c => ComposeNumericTable(c, categoryGroup.Key, numericItems));
                    }

                    // 非數字報告表格
                    if (nonNumericItems.Count != 0)
                    {
                        col.Item()
                           .TableItemContainerStyle()
                           .Element(c => ComposeNonNumericTable(c, categoryGroup.Key, nonNumericItems));
                    }
                }
            });
        }

        /// <summary>
        /// 抽出數字、非數字表格的共用邏輯。
        /// </summary>
        /// <param name="container">表格繪製工具。</param>
        /// <param name="groupName">每組檢查項目名稱。</param>
        /// <param name="headerList">每組表格的欄位清單。</param>
        /// <param name="reportItems">每組表格的檢查項目資料。</param>
        /// <param name="columnDef">委派方法，定義出表格的欄位結構。</param>
        /// <param name="tableContentRenderer">委派方法，定義出表格的渲染結構。</param>
        private void ComposeBaseTable(
            IContainer container,
            string groupName,
            string[] headerList,
            List<HealthReportItemModel> reportItems,
            Action<TableColumnsDefinitionDescriptor> columnDef,
            Action<TableDescriptor, HealthReportItemModel> tableContentRenderer)
        {
            container.Table(table =>
            {
                // 各自表格的欄位定義。
                table.ColumnsDefinition(columnDef);

                // 各自表格的標題列與欄位名稱。
                table.Header(header =>
                {
                    header.Cell()
                          .ColumnSpan((uint)headerList.Length)
                          .Text($"◆{groupName}")
                          .Style(PdfStyleExtensions.TableTitleStyle);

                    foreach (var headerItem in headerList)
                    {
                        header.Cell()
                              .TableKeyContainerStyle()
                              .Text(headerItem)
                              .Style(PdfStyleExtensions.KeyStyle);
                    }
                });

                // 各自表格的內容。
                foreach (var item in reportItems)
                {
                    tableContentRenderer(table, item);
                }
            });
        }

        /// <summary>
        /// 渲染數字類型的表格。
        /// </summary>
        private void ComposeNumericTable(IContainer container, string groupName, List<HealthReportItemModel> reportItems)
        {
            ComposeBaseTable(container, groupName, _numericItemHeaderSet, reportItems,
                col =>
                {
                    // 第一欄: 項目
                    col.RelativeColumn(3);

                    // 第二欄: 檢查結果
                    col.RelativeColumn(2);

                    // 第三欄: 單位
                    col.RelativeColumn(2);

                    // 第四欄: 正常值
                    col.RelativeColumn(2);
                },
                (table, item) =>
                {
                    // 判斷是否為異常值，並設定樣式
                    bool isAbnormal = _reportConfig.Rules?.AbnormalCodes.Contains(item.檢驗評值) == true;
                    var resultStyle = isAbnormal ? PdfStyleExtensions.AbnormalValueStyle : TextStyle.Default;

                    // 開始寫入每一列資料
                    table.Cell().TableValueContainerStyle().Text(item.項目);
                    table.Cell().TableValueContainerStyle().Text(item.結果).Style(resultStyle);
                    table.Cell().TableValueContainerStyle().Text(item.單位);
                    table.Cell().TableValueContainerStyle().Text(item.正常值);
                });
        }

        /// <summary>
        /// 渲染非數字類型的表格。
        /// </summary>
        private void ComposeNonNumericTable(IContainer container, string groupName, List<HealthReportItemModel> reportItems)
        {
            ComposeBaseTable(container, groupName, _nonNumericItemHeaderSet, reportItems,
                col =>
                {
                    // 第一欄: 項目
                    col.RelativeColumn(3);

                    // 第二欄: 檢查結果
                    col.RelativeColumn(2);

                    // 第三欄: 單位
                    col.RelativeColumn(2);
                },
                (table, item) =>
                {
                    // 判斷是否為異常值，並設定樣式
                    bool isAbnormal = _reportConfig.Rules?.AbnormalCodes.Contains(item.檢驗評值) == true;
                    var resultStyle = isAbnormal ? PdfStyleExtensions.AbnormalValueStyle : TextStyle.Default;

                    // 開始寫入每一列資料
                    table.Cell().TableValueContainerStyle().Text(item.項目);
                    table.Cell().TableValueContainerStyle().Text(item.結果).Style(resultStyle);
                    table.Cell().TableValueContainerStyle().Text(item.單位);
                });
        }
        #endregion

        #region --- 區塊 3: 表格內容(二): 醫師總評、頁尾資訊 ---
        /// <summary>
        /// 醫師總評表格。
        /// </summary>
        private void ComposeDoctorReview(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(col =>
                {
                    // 第一欄: 器官
                    col.RelativeColumn(15);

                    // 第二欄: 診斷
                    col.RelativeColumn(40);

                    // 第三欄: 建議
                    col.RelativeColumn(40);
                });

                // 表格標題列與欄位名稱
                table.Header(header =>
                {
                    header.Cell()
                          .ColumnSpan(3)
                          .Text("◆醫師總評")
                          .Style(PdfStyleExtensions.TableTitleStyle);

                    foreach (var headerItem in _doctorReviewHeaderSet)
                    {
                        header.Cell()
                              .TableKeyContainerStyle()
                              .Text(headerItem)
                              .Style(PdfStyleExtensions.KeyStyle);
                    }
                });

                // 表格內容
                foreach (var item in _model.OrganReviewReports)
                {
                    table.Cell().TableValueContainerStyle().Text(item.器官);
                    table.Cell().TableValueContainerStyle().Text(item.診斷);
                    table.Cell().TableValueContainerStyle().Text(item.建議);
                }
            });
        }

        /// <summary>
        /// 頁尾資訊設計。
        /// </summary>
        private void ComposeFooter(IContainer container)
        {
            container.Column(col =>
            {
                // 第一行頁尾顯示:【註】
                col.Item()
                   .Text(_reportConfig.RiskNote?.Title)
                   .Style(PdfStyleExtensions.KeyStyle);

                // 第二行頁尾顯示: 心血管疾病風險計算參考網址
                col.Item()
                   .Text(_reportConfig.RiskNote?.Content);

                // 第三行表格欄位定義
                col.Item()
                   .TableItemContainerStyle()
                   .Table(table =>
                   {
                       table.ColumnsDefinition(c =>
                       {
                           // 第一欄: 負責醫師、醫師章
                           c.RelativeColumn();

                           // 第二欄: 醫師證號
                           c.RelativeColumn();
                       });

                       // 負責醫師、醫師章欄位內容
                       table.Cell()
                            .DoctorInfoContainerStyle()
                            .Row(row =>
                            {
                                string doctorFormat = _reportConfig.DoctorDisplay?.NameFormat ?? "無負責醫師";

                                row.AutoItem()
                                   .Text(string.Format(doctorFormat, _model.DoctorInfo.負責醫師));

                                if (_model.DoctorInfo.醫師章授權 == 1)
                                {
                                    row.ConstantItem(80)
                                           .DoctorSealContainerStyle()
                                           .Text(text =>
                                           {
                                               text.Span("醫師")
                                                   .Style(PdfStyleExtensions.DoctorSealStyle);

                                               text.Span(" ");

                                               text.Span(_model.DoctorInfo.負責醫師)
                                                   .Style(PdfStyleExtensions.DoctorSealNameStyle);
                                           });
                                }
                            });

                       // 醫師證號欄位內容
                       table.Cell()
                            .DoctorInfoContainerStyle()
                            .Row(row =>
                            {
                                string licenseFormat = _reportConfig.DoctorDisplay?.LicenseFormat ?? "無醫師證號";

                                row.AutoItem()
                                   .Text(string.Format(licenseFormat, _model.DoctorInfo.醫師證號));
                            });
                   });

                // 第四行頁尾顯示: 醫院聯絡資訊
                col.Item()
                   .ContactInfoContainerStyle()
                   .Text(_reportConfig.Organization?.ContactInfo);
            });
        }
        #endregion
    }
}
