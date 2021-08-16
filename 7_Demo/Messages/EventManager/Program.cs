using EventManager.Models;
using EventManager.Services;
using Events;
using Messages.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Serilog;
using Serilog.Events;
using Telegram.Bot;

namespace EventManager
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices)
                .UseSerilog();
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(context.Configuration.GetSeq())
                .CreateLogger();

            services.AddRebus(context);
            services.AddHostedService<Worker>();

            services.AddSingleton(new DatabaseOptions(context.Configuration.GetSql()));
            services.AddSingleton<IBotService, BotService>();
            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(context.Configuration.GetBotToken()));
            services.AddSingleton<IDbService, DbService>();
        }

        private static void AddRebus(this IServiceCollection services, HostBuilderContext context)
        {
            services.AddRebus(configure => configure
                .Logging(l => l.Serilog())
                .Transport(t => t.UseRabbitMq(context.Configuration.GetRabbitMq(), "publisher_queue"))
                .Routing(r =>
                {
                    r.TypeBased()
                        .Map<SpeakerApplied>($"{nameof(SpeakerApplied)}_queue")
                        .Map<SpeakerPrepared>($"{nameof(SpeakerPrepared)}_queue")
                        .Map<SponsorConfirmed>($"{nameof(SponsorConfirmed)}_queue")
                        .Map<MeetupAnnounced>($"{nameof(MeetupAnnounced)}_queue")
                        .Map<MeetupIsOver>($"{nameof(MeetupIsOver)}_queue")
                        .Map<FeedbackFormRequested>($"{nameof(FeedbackFormRequested)}_queue")
                        .Map<SpeakerFeedbackProvided>($"{nameof(SpeakerFeedbackProvided)}_queue")
                        .Map<OrganizerFeedbackProvided>($"{nameof(OrganizerFeedbackProvided)}_queue");
                })
            );
        }
    }
}