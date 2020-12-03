namespace MAS.DapperStorageTest.Models
{
    public class DriverOrder : Entity
    {
        public virtual Driver Driver { get; set; }

        public virtual Order Order { get; set; }
    }
}
