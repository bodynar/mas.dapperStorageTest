namespace MAS.DapperStorageTest.Models
{
    using System;

    public abstract class Person : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
