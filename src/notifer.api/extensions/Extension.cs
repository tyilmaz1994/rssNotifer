﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using notifer.api.factories;
using notifer.api.factories.scheduleListeners;
using notifier.bl.hostedServices;
using notifier.bl.hostedServices.jobs;
using notifier.bl.services;
using notifier.dal.context;
using notifier.dal.persistence;
using notifier.dal.repos;
using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;

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
        
        public static void RegisterHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<ScheduleHostedService>();
        }

        /// <summary>
        /// register jobs into IOC in order to inject services
        /// </summary>
        public static void RegisterScheduleComponents(this IServiceCollection services)
        {
            StdSchedulerFactory factory = new StdSchedulerFactory(new NameValueCollection { { "quartz.serializer.tpye", "binary" } });
            services.AddSingleton(x => factory.GetScheduler().Result);
            services.AddTransient<RssNewsJob>();
        }

        /// <summary>
        /// Scheduler Job injection to asp.net core IOC
        /// </summary>
        public static void SetScheduleFactory(this IApplicationBuilder app)
        {
            var scheduler = app.ApplicationServices.GetService<IScheduler>();
            scheduler.JobFactory = new ScheduleJobFactory(app.ApplicationServices);
            scheduler.ListenerManager.AddSchedulerListener(new ScheduleListener());
            scheduler.ListenerManager.AddJobListener(new JobListener());
            scheduler.Start().Wait();
        }
    }
}