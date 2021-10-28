using Core.Repositories;
using Core.Settings;
using Infrastructure.MongoDB;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddOptions<RepositorySettings>().Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("RepositorySettings").Bind(settings);
            });

            services            
                     .AddTransient<IHighScoreRepository, HighScoreRepository>()
                     .AddTransient<IMongoDBContext, MongoDBContext>()
            ;

            return services;

        }
    }
}
