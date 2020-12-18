﻿namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure.Cqrs;
    using MAS.DapperStorageTest.Infrastructure.Models;

    // TODO: Add rows count
    // TODO: Add rows skip
    public class SelectQuery : IQuery<WrappedEntity>
    {
        public string EntityName { get; set; }

        public IEnumerable<string> Fields { get; }

        public Guid? EntityId { get; }

        public IDictionary<string, string> Filters { get; }

        public QueryFilterGroup FilterGroup { get; }

        public SelectQuery(string entityName, IEnumerable<string> fields, Guid? entityId)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            Fields = fields ?? Enumerable.Empty<string>();
            EntityId = entityId;
        }

        public SelectQuery(string entityName, IEnumerable<string> fields, QueryFilterGroup filterGroup)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            Fields = fields ?? Enumerable.Empty<string>();
            FilterGroup = filterGroup ?? throw new ArgumentNullException(nameof(filterGroup));
        }
    }
}
