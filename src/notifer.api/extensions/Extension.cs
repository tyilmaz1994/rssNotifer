using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using notifier.bl.services;
using notifier.dal.context;
using notifier.dal.persistence;
using notifier.dal.repos;

namespace notifer.api.extensions
{
    public static class Extension
    {
        /// <summary>
        /// Registers context of MongoDB on startup
        /// </summary>
        public static void RegisterMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<NotifierDbContext>(
                configuration.GetSection(nameof(NotifierDbContext)));

            services.AddSingleton<INotifierDbContext>(sp =>
                sp.GetRequiredService<IOptions<NotifierDbContext>>().Value);
        }

        /// <summary>
        /// Registers all services of dal and bl layer on startup
        /// </summary>
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepo<>), typeof(AbstractRepo<>));
            services.AddTransient(typeof(ILogService), typeof(LogService));
        }
    }
}