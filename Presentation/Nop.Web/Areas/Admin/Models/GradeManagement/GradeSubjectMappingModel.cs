using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.GradeManagement;

public partial record GradeSubjectMappingModel : BaseNopEntityModel
{
    #region Ctor

    public GradeSubjectMappingModel()
    {
        AvailableSections = new List<SelectListItem>();
        AvailableSubjects = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    public int GradeId { get; set; }

    public int SubjectId { get; set; }

    public string SubjectName { get; set; }

    public int? SectionId { get; set; }

    public string SectionName { get; set; }

    public decimal LabFee { get; set; }

    public bool Deleted { get; set; }

    #endregion

    [NopResourceDisplayName("Admin.Configuration.Grades.Subject.Subjects")]
    public IList<int> SelectedSubjectIds { get; set; }
    public IList<SelectListItem> AvailableSubjects { get; set; }


    [NopResourceDisplayName("Admin.Configuration.Grades.Subject.Sections")]
    public IList<int> SelectedSectionIds { get; set; }
    public IList<SelectListItem> AvailableSections { get; set; }
}