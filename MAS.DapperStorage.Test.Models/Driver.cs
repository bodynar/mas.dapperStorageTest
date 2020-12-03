namespace MAS.DapperStorageTest.Models
{
    [EntityMarker]
    public class Driver : Person
    {
        public double Experience { get; set; }

        public double Rating { get; set; }

        public bool IsSmoker { get; set; }

        public bool IsTalkative { get; set; }

        public bool IsMusicLover { get; set; }
    }
}
