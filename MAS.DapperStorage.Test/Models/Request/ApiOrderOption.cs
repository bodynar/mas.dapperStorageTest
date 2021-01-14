namespace MAS.DapperStorageTest.Models
{
    using MAS.DappertStorageTest.Cqrs;

    public class ApiOrderOption
    {
        public string Column { get; set; }

        public OrderDirection Direction { get; set; }

        public static implicit operator OrderOption(ApiOrderOption apiOrderOption)
        {
            if (apiOrderOption == null)
            {
                return null;
            }

            return new OrderOption(apiOrderOption.Column, apiOrderOption.Direction);
        }
    }
}
