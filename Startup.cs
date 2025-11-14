using DotNetNuke.DependencyInjection;
using DotNetNuke.Wiki.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetNuke.Wiki
{
    /// <summary>
    /// Runs on DNN startup to register services with the dependency injection container.
    /// </summary>
    public class Startup : IDnnStartup
    {
        /// <inheritdoc/>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDNNUtils, DNNUtils>();
        }
    }
}