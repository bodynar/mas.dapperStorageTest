namespace MAS.DapperStorageTest.Models
{
    using System;

    public abstract class Entity
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
