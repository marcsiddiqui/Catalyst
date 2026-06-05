using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Students;

namespace Nop.Data.Mapping.Builders.Students;

public partial class StudentBuilder : NopEntityBuilder<Student>
{
    #region Methods

    public override void MapEntity(CreateTableExpressionBuilder table)
    {
    }

    #endregion
}
