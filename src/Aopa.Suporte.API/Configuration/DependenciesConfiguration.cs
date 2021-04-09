using Aopa.Suporte.Business.Application.Interfaces;
using Aopa.Suporte.Business.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Aopa.Suporte.API.Configuration
{
    public static class DependenciesConfiguration
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            /* Services */
            services.AddScoped<ICriptografiaService, CriptografiaService>();

            /* Facades */

            /* Repository */
        }
    }
}
