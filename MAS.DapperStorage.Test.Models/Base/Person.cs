namespace MAS.DapperStorageTest.Models.Base
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
