using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;

namespace Nop.Data.Mapping.Builders.Customers;

public partial class CustomerSessionBuilder : NopEntityBuilder<CustomerSession>
{
    #region Methods

    public override void MapEntity(CreateTableExpressionBuilder table)
    {
    }

    #endregion
}
