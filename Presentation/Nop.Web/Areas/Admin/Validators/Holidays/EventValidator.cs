using FluentValidation;
using Nop.Core.Domain.HolidaysNEvents;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.HolidaysNEvents;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Holidays
{
    public class EventValidator : BaseNopValidator<EventModel>
    {
        public EventValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Admin.Configuration.Events.Fields.Name.Required"));
            RuleFor(x => x.AcademicYearId).GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("Admin.Configuration.Events.Fields.AcademicYearId.Required"));
            RuleFor(x => x.StartDateUtc).NotNull().WithMessageAwait(localizationService.GetResourceAsync("Admin.Configuration.Events.Fields.StartDateUtc.Required"));
            RuleFor(x => x.EndDateUtc).NotNull().WithMessageAwait(localizationService.GetResourceAsync("Admin.Configuration.Events.Fields.EndDateUtc.Required"));

            SetDatabaseValidationRules<Event>();
        }
    }
}
