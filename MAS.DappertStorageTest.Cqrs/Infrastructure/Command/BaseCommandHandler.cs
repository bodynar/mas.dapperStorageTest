namespace MAS.DappertStorageTest.Cqrs.Infrastructure
{
    using System;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Cqrs;

    public abstract class BaseCommandHandler<TCommand> : BaseCqrsHandler, ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        protected Type CommandType { get; }

        public BaseCommandHandler(IResolver resolver)
            : base(resolver)
        {
            CommandType = typeof(TCommand);
        }

        public void Handle(TCommand command)
        {
            var isValidEntityName = IsValidEntityName(command.EntityName);

            if (isValidEntityName)
            {
                Proceed(command);
            } else
            {
                throw new DatabaseQueryException($"{command.EntityName} isn't presented in database.");
            }
        }

        public abstract void Proceed(TCommand command);
    }
}
