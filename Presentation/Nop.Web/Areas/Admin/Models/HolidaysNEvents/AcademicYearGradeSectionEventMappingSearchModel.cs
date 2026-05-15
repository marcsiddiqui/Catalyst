using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.HolidaysNEvents;

public partial record AcademicYearGradeSectionEventMappingSearchModel : BaseSearchModel
{
    public int EventId { get; set; }
}
