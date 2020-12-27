namespace MAS.DapperStorageTest.Models
{
    using System.Collections.Generic;
    using System.Linq;

    using MAS.DappertStorageTest.Cqrs;

    public class ApiFilterGroup
    {
        public string EntityName { get; set; }

        public string Name { get; set; }

        public FilterJoinType FilterJoinType { get; set; }

        public IEnumerable<ApiFilter> Filters { get; set; } = Enumerable.Empty<ApiFilter>();

        public IEnumerable<ApiFilterGroup> InnerGroups { get; set; } = Enumerable.Empty<ApiFilterGroup>();

        public static implicit operator QueryFilterGroup(ApiFilterGroup apiFilterGroup)
        {
            return
                apiFilterGroup.InnerGroups.Any()
                ? new QueryFilterGroup(apiFilterGroup.EntityName, apiFilterGroup.Name, apiFilterGroup.FilterJoinType, apiFilterGroup.InnerGroups.Select(x => (QueryFilterGroup)x))
                : new QueryFilterGroup(apiFilterGroup.EntityName, apiFilterGroup.Name, apiFilterGroup.FilterJoinType, apiFilterGroup.Filters.Select(x => (QueryFilterItem)x));
        }
    }
}
