using System;

namespace MAS.DappertStorageTest.Cqrs.Infrastructure
{
    public static class FilterUtils
    {
        public static string GetSqlOperator(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            if (field == null)
            {
                return string.Empty;
            }

            var comparisonOperatorAttributes =
                field.GetCustomAttributes(typeof(SqlOperatorAttribute), false) as SqlOperatorAttribute[];

            if (comparisonOperatorAttributes != null
                && comparisonOperatorAttributes.Length > 0)
            {
                return comparisonOperatorAttributes[0].Operator;
            }

            return string.Empty;
        }
    }
}
