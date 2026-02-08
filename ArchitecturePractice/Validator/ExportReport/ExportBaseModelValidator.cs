using ArchitecturePractice.Core.ExportReport.ExportModel;
using FluentValidation;

namespace ArchitecturePractice.Validator.ExportReport
{
    /// <summary>
    /// ExportReport匯出基本欄位資料驗證器基底類別。
    /// </summary>
    public abstract class ExportBaseModelValidator<T> : AbstractValidator<T>
        where T : ExportRequestBaseModel
    {
        protected ExportBaseModelValidator()
        {
            RuleFor(m => m.ExamineEndDate)
                .GreaterThanOrEqualTo(m => m.ExamineStartDate)
                .WithMessage("健檢查詢結束日期不可小於起始日期。");

            RuleFor(m => m.SelectedCompanyId)
                .GreaterThan(0)
                .When(m => m.SelectedCompanyId.HasValue)
                .WithMessage("請選擇有效的公司。");

            When(m => m.SelectedRoleId.HasValue && m.SelectedRoleId > 0, () =>
            {
                RuleFor(m => m.SelectedCompanyId)
                    .NotNull()
                    .GreaterThan(0)
                    .WithMessage("請先選擇公司，才能選擇一級單位。");
            });

            RuleFor(m => m.ReportFormat)
                .IsInEnum()
                .WithMessage("請選擇報表匯出格式。");
        }
    }
}
