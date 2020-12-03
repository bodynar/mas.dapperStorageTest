namespace MAS.DapperStorageTest.Models
{
    [EntityMarker]
    public class Order : Entity
    {
        public virtual Passenger Passenger { get; set; }

        public virtual Drive Drive { get; set; }

        public double Price { get; set; }

        public string Target { get; set; }

        public string Destination { get; set; }
    }
}
