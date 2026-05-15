using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.HolidaysNEvents;

public partial record AcademicYearGradeSectionEventMappingModel : BaseNopEntityModel
{
    public AcademicYearGradeSectionEventMappingModel()
    {
        AvailableAcademicYearGradeSectionMappings = new List<SelectListItem>();
        SelectedAcademicYearClassSectionMappingIds = new List<int>();
    }

    public int EventId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.AcademicYearGradeSectionEventMapping.Fields.AcademicYearClassSectionMappingId")]
    public int AcademicYearClassSectionMappingId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.AcademicYearGradeSectionEventMapping.Fields.AcademicYearClassSectionMappingId")]
    public IList<int> SelectedAcademicYearClassSectionMappingIds { get; set; }

    public string AcademicYearClassSectionMappingName { get; set; }

    public IList<SelectListItem> AvailableAcademicYearGradeSectionMappings { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.AcademicYearGradeSectionEventMapping.Fields.Amount")]
    public decimal Amount { get; set; }
}
