namespace MAS.DappertStorageTest.Cqrs.Infrastructure
{
    using System;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Cqrs;

    public abstract class BaseCommandHandler<TCommand> : BaseCqrsHandler, ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        protected Type CommandType { get; }

        public BaseCommandHandler(IDbConnectionFactory dbConnectionFactory, IDbAdapter dbAdapter)
            : base(dbConnectionFactory, dbAdapter)
        {
            CommandType = typeof(TCommand);
        }

        public BaseCommandHandler(IDbConnectionFactory dbConnectionFactory, IDbAdapter dbAdapter, IFilterBuilder filterBuilder)
            : base(dbConnectionFactory, dbAdapter, filterBuilder)
        {
            CommandType = typeof(TCommand);
        }

        public abstract void Handle(TCommand command);
    }
}
