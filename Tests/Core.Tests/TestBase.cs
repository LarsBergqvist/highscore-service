using System;
using Core.Repositories;
using Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Core.Helpers;

namespace Core.Tests
{
    public class TestBase
    {
        protected readonly IHighScoreRepository repo;
        public TestBase()
        {
            repo = new FakeHighScoreRepository(new GameResultHelper());
            Setup(repo);
        }

        protected IServiceProvider serviceProvider;
        protected IMediator Mediator => serviceProvider.GetService<IMediator>();

        protected void Setup(IHighScoreRepository repository)
        {
            var serviceCollection = new ServiceCollection()
                .AddCoreServices()
                .AddSingleton(repository)
                ;

            serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
