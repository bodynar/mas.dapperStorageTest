namespace MAS.DapperStorageTest.Models
{
    [EntityMarker]
    public class Passenger : Person
    {
        public double? Rating { get; set; }
    }
}
