using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.AcademicYears;

public partial record AcademicYearGradeSectionMappingModel : BaseNopEntityModel
{
    #region Ctor

    public AcademicYearGradeSectionMappingModel()
    {
        AvailableGrades = new List<SelectListItem>();
        AvailableSections = new List<SelectListItem>();

        SelectedGradeIds = [];
        SelectedSectionIds = [];
    }

    #endregion

    #region Properties

    public int AcademicYearId { get; set; }

    public int GradeId { get; set; }

    public string GradeName { get; set; }

    public int SectionId { get; set; }

    public string SectionName { get; set; }

    public int ExamTermCount { get; set; }

    public bool Deleted { get; set; }

    #endregion

    [NopResourceDisplayName("Admin.Configuration.Grades.Subject.Subjects")]
    public IList<int> SelectedGradeIds { get; set; }
    public IList<SelectListItem> AvailableGrades { get; set; }


    [NopResourceDisplayName("Admin.Configuration.Grades.Subject.Sections")]
    public IList<int> SelectedSectionIds { get; set; }
    public IList<SelectListItem> AvailableSections { get; set; }
}