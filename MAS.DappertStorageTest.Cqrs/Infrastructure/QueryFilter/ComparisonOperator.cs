namespace MAS.DappertStorageTest.Cqrs
{
    using System;

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class SqlOperatorAttribute : Attribute
    {
        public string Operator { get; }

        public SqlOperatorAttribute(string @operator)
        {
            Operator = @operator;
        }
    }
}
