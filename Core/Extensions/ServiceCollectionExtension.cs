using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Core.Helpers;

namespace Core.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            return services
                      .AddScoped<IGameResultHelper, GameResultHelper>()
                      .AddMediatR(typeof(ServiceCollectionExtension))
            ;
        }
    }
}
