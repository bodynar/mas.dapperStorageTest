namespace MAS.DappertStorageTest.Cqrs
{
    using System.Collections.Generic;

    using MAS.DapperStorageTest.Infrastructure.Cqrs;

    public abstract class BaseCommand : ICommand
    {
        public ICollection<string> Warnings { get; set; } = new List<string>();
    }
}
