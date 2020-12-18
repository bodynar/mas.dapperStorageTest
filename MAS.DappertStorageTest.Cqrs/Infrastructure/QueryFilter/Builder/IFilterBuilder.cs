namespace MAS.DappertStorageTest.Cqrs
{
    using System.Dynamic;

    public interface IFilterBuilder
    {
        (string, ExpandoObject) Build(QueryFilterGroup queryFilterGroup);
    }
}
