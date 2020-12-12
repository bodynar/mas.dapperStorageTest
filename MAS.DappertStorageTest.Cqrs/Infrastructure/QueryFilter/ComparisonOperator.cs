namespace MAS.DappertStorageTest.Cqrs
{
    using System;

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class ComparisonOperatorAttribute : Attribute
    {
        public string Operator { get; }

        public ComparisonOperatorAttribute(string @operator)
        {
            Operator = @operator;
        }
    }
}
