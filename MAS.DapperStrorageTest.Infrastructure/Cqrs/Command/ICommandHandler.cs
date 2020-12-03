namespace MAS.DapperStorageTest.Infrastructure.Cqrs
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        void Handle(TCommand command);
    }
}
