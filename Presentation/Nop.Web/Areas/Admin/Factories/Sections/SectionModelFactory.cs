using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;
using Nop.Core.Domain.GradeManagement;
using Nop.Services.GradeManagement;
using Nop.Web.Areas.Admin.Models.GradeManagement;

namespace Nop.Web.Areas.Admin.Factories;

public partial class SectionModelFactory : ISectionModelFactory
{
    #region Fields

    protected readonly ISectionService _sectionService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;

    #endregion

    #region Ctor

    public SectionModelFactory(ISectionService sectionService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory)
    {
        _sectionService = sectionService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
    }

    #endregion

    #region Utilities

    

    #endregion

    #region Methods

    public virtual Task<SectionSearchModel> PrepareSectionSearchModelAsync(SectionSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    public virtual async Task<SectionListModel> PrepareSectionListModelAsync(SectionSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get sections
        var sections = (await _sectionService.GetAllSectionsAsync()).ToPagedList(searchModel);

        //prepare list model
        var model = await new SectionListModel().PrepareToGridAsync(searchModel, sections, () =>
        {
            //fill in model values from the entity
            return sections.SelectAwait(async section =>
            {
                var sectionModel = section.ToModel<SectionModel>();

                return sectionModel;
            });
        });

        return model;
    }

    public virtual async Task<SectionModel> PrepareSectionModelAsync(SectionModel model, Section section, bool excludeProperties = false)
    {
        Func<SectionLocalizedModel, int, Task> localizedModelConfiguration = null;

        if (section != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = section.ToModel<SectionModel>();
            }

            //define localized model configuration action
            localizedModelConfiguration = async (locale, languageId) =>
            {
                locale.Name = await _localizationService.GetLocalizedAsync(section, entity => entity.Name, languageId, false, false);


            };
        }

        //set default values for the new model
        if (section == null)
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