namespace MAS.DapperStorageTest.Models
{
    public class PassengerOrder : Entity
    {
        public virtual Passenger Passenger { get; set; }

        public virtual Order Order { get; set; }
    }
}
