namespace MAS.DappertStorageTest.Cqrs
{
    using System.Collections.Generic;
    using System.Dynamic;

    public interface IFilterBuilder
    {
        (string, ExpandoObject) Build(IEnumerable<QueryFilterGroup> queryFilterGroup);
    }
}
