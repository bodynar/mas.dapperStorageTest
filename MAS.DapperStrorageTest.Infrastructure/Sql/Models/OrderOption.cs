namespace MAS.DapperStorageTest.Infrastructure.Sql
{
    using System;

    public class OrderOption
    {
        public string Column { get; }

        public OrderDirection OrderDirection { get; }

        public OrderOption(string column, OrderDirection orderDirection)
        {
            Column = column ?? throw new ArgumentNullException(nameof(column));
            OrderDirection = orderDirection;
        }
    }
}
