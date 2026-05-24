using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Admissions;

namespace Nop.Data.Mapping.Builders.Admissions;

public partial class AdmissionBuilder : NopEntityBuilder<Admission>
{
    #region Methods

    public override void MapEntity(CreateTableExpressionBuilder table)
    {
    }

    #endregion
}
