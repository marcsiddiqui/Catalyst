using FluentValidation;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.HolidaysNEvents;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Discounts;
using Nop.Web.Areas.Admin.Models.HolidaysNEvents;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Holidays
{
    public class HolidayValidator : BaseNopValidator<HolidayModel>
    {
        public HolidayValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Admin.Configuration.Holidays.Fields.Name.Required"));
            RuleFor(x => x.AcademicYearId).GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("Admin.Configuration.Holidays.Fields.AcademicYearId.Required"));
            RuleFor(x => x.DateFromUtc).NotNull().WithMessageAwait(localizationService.GetResourceAsync("Admin.Configuration.Holidays.Fields.DateFromUtc.Required"));
            RuleFor(x => x.DateToUtc).NotNull().WithMessageAwait(localizationService.GetResourceAsync("Admin.Configuration.Holidays.Fields.DateToUtc.Required"));

            SetDatabaseValidationRules<Holiday>();
        }
    }
}
