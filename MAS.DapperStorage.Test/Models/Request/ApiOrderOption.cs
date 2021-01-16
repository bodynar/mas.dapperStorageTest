namespace MAS.DapperStorageTest.Models
{
    using MAS.DappertStorageTest.Cqrs;

    /// <summary>
    /// Order configuration
    /// </summary>
    public class ApiOrderOption
    {
        /// <summary>
        /// Name of column
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// Order direction
        /// </summary>
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
