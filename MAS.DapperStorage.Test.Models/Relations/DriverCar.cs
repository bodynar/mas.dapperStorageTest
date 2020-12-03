namespace MAS.DapperStorageTest.Models
{
    using System;

    public class DriverCar : Entity
    {
        public virtual Driver Driver { get; set; }

        public virtual Car Car { get; set; }

        public DateTime UsedAt { get; set; }
    }
}
