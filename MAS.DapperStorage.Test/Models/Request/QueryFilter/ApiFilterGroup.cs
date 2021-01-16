namespace MAS.DapperStorageTest.Models
{
    using System.Collections.Generic;
    using System.Linq;

    using MAS.DappertStorageTest.Cqrs;

    /// <summary>
    /// Filter group configuration
    /// </summary>
    public class ApiFilterGroup
    {
        /// <summary>
        /// Name of entity (table name in database)
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Name of group
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of boolean joining
        /// </summary>
        public FilterJoinType FilterJoinType { get; set; }

        /// <summary>
        /// Inner filters
        /// </summary>
        public IEnumerable<ApiFilter> Filters { get; set; } = Enumerable.Empty<ApiFilter>();

        /// <summary>
        /// Inner filter groups
        /// </summary>
        public IEnumerable<ApiFilterGroup> InnerGroups { get; set; } = Enumerable.Empty<ApiFilterGroup>();

        public static implicit operator QueryFilterGroup(ApiFilterGroup apiFilterGroup)
        {
            if (apiFilterGroup == null)
            {
                return null;
            }

            return
                apiFilterGroup.InnerGroups.Any()
                ? new QueryFilterGroup(apiFilterGroup.EntityName, apiFilterGroup.Name, apiFilterGroup.FilterJoinType, apiFilterGroup.InnerGroups.Select(x => (QueryFilterGroup)x))
                : new QueryFilterGroup(apiFilterGroup.EntityName, apiFilterGroup.Name, apiFilterGroup.FilterJoinType, apiFilterGroup.Filters.Select(x => (QueryFilterItem)x));
        }
    }
}
