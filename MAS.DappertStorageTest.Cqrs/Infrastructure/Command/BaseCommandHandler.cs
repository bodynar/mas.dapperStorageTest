namespace MAS.DappertStorageTest.Cqrs.Infrastructure
{
    using System;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Cqrs;

    public abstract class BaseCommandHandler<TCommand> : BaseCqrsHandler, ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        protected Type CommandType { get; }

        public BaseCommandHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
            CommandType = typeof(TCommand);
        }

        public abstract void Handle(TCommand command);
    }
}
