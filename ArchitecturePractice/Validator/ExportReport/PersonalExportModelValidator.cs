using ArchitecturePractice.Core.ExportReport.ExportModel.PersonalHealthExam;
using FluentValidation;

namespace ArchitecturePractice.Validator.ExportReport
{
    /// <summary>
    /// 個人健檢匯出資料驗證器，繼承ExportBaseModelValidator，包含個人健檢特有欄位的驗證規則。
    /// </summary>
    public class PersonalExportModelValidator : ExportBaseModelValidator<PersonalExportRequest>
    {
        public PersonalExportModelValidator()
        {
            RuleFor(m => m.ReportOption)
                .IsInEnum()
                .WithMessage("請選擇有效的報表類型。");

            RuleFor(m => m.PackageCode)
                .MaximumLength(10)
                .WithMessage("套餐代碼不可超過10個字元。");

            RuleFor(m => m.IdOrChartNumber)
                .NotEmpty()
                .WithMessage("身分證或病歷號為必填欄位。")
                .MaximumLength(20)
                .WithMessage("身分證或病歷號不可超過20個字元。");
        }
    }
}
