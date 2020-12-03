namespace MAS.DapperStorageTest.Infrastructure.Cqrs
{
    public interface IQueryProcessor
    {
        TResult Execute<TResult>(IQuery<TResult> query);
    }
}
