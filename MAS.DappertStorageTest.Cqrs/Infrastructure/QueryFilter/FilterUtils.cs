namespace MAS.DappertStorageTest.Cqrs.Infrastructure
{
    public static class FilterUtils
    {
        public static string GetComparisonOperator(this ComparisonType comparisonType)
        {
            var field = comparisonType.GetType().GetField(comparisonType.ToString());

            if (field == null)
            {
                return string.Empty;
            }

            var comparisonOperatorAttributes =
                field.GetCustomAttributes(typeof(ComparisonOperatorAttribute), false) as ComparisonOperatorAttribute[];

            if (comparisonOperatorAttributes != null
                && comparisonOperatorAttributes.Length > 0)
            {
                return comparisonOperatorAttributes[0].Operator;
            }

            return string.Empty;
        }
    }
}
