namespace MAS.DapperStorageTest.Models
{
    using System;

    [EntityMarker]
    public class Drive : Entity
    {
        public DateTime StartAt { get; set; }

        public DateTime CalculatedEndAt { get; set; }

        public DateTime? FactEndAt { get; set; }

        public bool IsCompleted
            => FactEndAt.HasValue;

        public virtual Passenger Passenger { get; set; }

        public virtual Driver Driver { get; set; }

        public virtual Car Car { get; set; }
    }
}
