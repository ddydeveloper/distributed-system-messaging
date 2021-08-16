using System;
using Messages.Extensions;
using Messages.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.Config;
using Rebus.ServiceProvider;
using Serilog;
using Serilog.Events;
using Telegram.Bot;

namespace Messages
{
    public static class Config
    {
        public static IHostBuilder CreateHostBuilder(
            string[] args,
            string queueName,
            Func<ISubscribeService> subscribeRegister,
            Action<IServiceCollection> handleRegister)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) => ConfigureServices(
                    context,
                    services,
                    queueName,
                    subscribeRegister,
                    handleRegister))
                .UseSerilog();
        }

        private static void ConfigureServices(
            HostBuilderContext context,
            IServiceCollection services,
            string queueName,
            Func<ISubscribeService> subscribeRegister,
            Action<IServiceCollection> handleRegister)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(context.Configuration.GetSeq())
                .CreateLogger();

            services.ConfigureRebus(context, queueName, subscribeRegister, handleRegister);

            services.AddHostedService<Worker>();
            services.AddSingleton(provider => provider);
            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(context.Configuration.GetBotToken()));
        }

        private static void ConfigureRebus(
            this IServiceCollection services,
            HostBuilderContext context,
            string queueName,
            Func<ISubscribeService> subscribeRegister,
            Action<IServiceCollection> handleRegister)
        {
            services.AddRebus(configure => configure
                .Logging(l => l.Serilog())
                .Transport(t => t.UseRabbitMq(context.Configuration.GetRabbitMq(), queueName).Prefetch(1))
            );

            services.AddTransient(_ => subscribeRegister());
            handleRegister(services);
        }
    }
}