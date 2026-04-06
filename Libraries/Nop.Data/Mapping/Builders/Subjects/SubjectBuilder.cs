using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Subjects;

namespace Nop.Data.Mapping.Builders.Subjects;

public partial class SubjectBuilder : NopEntityBuilder<Subject>
{
    #region Methods

    public override void MapEntity(CreateTableExpressionBuilder table)
    {
    }

    #endregion
}
