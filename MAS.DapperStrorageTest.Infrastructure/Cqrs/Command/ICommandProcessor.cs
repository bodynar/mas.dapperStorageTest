namespace MAS.DapperStorageTest.Infrastructure.Cqrs
{
    public interface ICommandProcessor
    {
        void Execute<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}
