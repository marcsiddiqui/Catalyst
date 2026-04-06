using Nop.Core.Domain.Subjects;
using Nop.Services.Subjects;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Subjects;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

public partial class SubjectModelFactory : ISubjectModelFactory
{
    #region Fields

    protected readonly ISubjectService _subjectService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;

    #endregion

    #region Ctor

    public SubjectModelFactory(ISubjectService subjectService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory)
    {
        _subjectService = subjectService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
    }

    #endregion

    #region Utilities

    

    #endregion

    #region Methods

    public virtual Task<SubjectSearchModel> PrepareSubjectSearchModelAsync(SubjectSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    public virtual async Task<SubjectListModel> PrepareSubjectListModelAsync(SubjectSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get subjects
        var subjects = (await _subjectService.GetAllSubjectsAsync()).ToPagedList(searchModel);

        //prepare list model
        var model = await new SubjectListModel().PrepareToGridAsync(searchModel, subjects, () =>
        {
            //fill in model values from the entity
            return subjects.SelectAwait(async subject =>
            {
                var subjectModel = subject.ToModel<SubjectModel>();

                return subjectModel;
            });
        });

        return model;
    }

    public virtual async Task<SubjectModel> PrepareSubjectModelAsync(SubjectModel model, Subject subject, bool excludeProperties = false)
    {
        Func<SubjectLocalizedModel, int, Task> localizedModelConfiguration = null;

        if (subject != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = subject.ToModel<SubjectModel>();
            }

            //define localized model configuration action
            localizedModelConfiguration = async (locale, languageId) =>
            {
                locale.Name = await _localizationService.GetLocalizedAsync(subject, entity => entity.Name, languageId, false, false);


            };
        }

        //set default values for the new model
        if (subject == null)
        {
            
        }

        //prepare localized models
        if (!excludeProperties)
            model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);


//{{PrepareStoreCode}}

        return model;
    }

    #endregion
}