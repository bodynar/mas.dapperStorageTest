namespace MAS.DapperStorageTest.Models
{
    using System;

    [EntityMarker]
    public class Car : Entity
    {
        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public DateTime ManufacturedDate { get; set; }

        public double Kilometrage { get; set; }

        public int SeatCount { get; set; }

        public virtual CarType Type { get; set; }

        public bool HasBabyChair { get; set; }
    }
}
