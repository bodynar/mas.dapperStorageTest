namespace MAS.DapperStorageTest
{
    using System;

    using MAS.DapperStorageTest.Infrastructure;

    using SimpleInjector;

    internal class Resolver : IResolver
    {
        private Container Container { get; }

        public Resolver(Container container)
        {
            Container = container;
        }

        public TService Resolve<TService>()
            where TService : class
        {
            return Container.GetInstance<TService>();
        }

        public object GetInstance(Type serviceType)
        {
            return Container.GetInstance(serviceType);
        }
    }
}
