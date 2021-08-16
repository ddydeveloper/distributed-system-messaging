using System;
using System.Collections.Generic;
using Events;
using Messages;
using Messages.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.ServiceProvider;

namespace Organizer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            static ISubscribeService GetSubscribeService()
            {
                return new SubscribeService(new List<Type>
                {
                    typeof(SpeakerPrepared),
                    typeof(SponsorConfirmed),
                    typeof(OrganizerFeedbackProvided)
                });
            }

            static void HandlerRegister(IServiceCollection services)
            {
                services.AutoRegisterHandlersFromAssemblyOf<SpeakerPreparedHandler>();
            }

            Config.CreateHostBuilder(
                    args,
                    "organizer_queue",
                    GetSubscribeService,
                    HandlerRegister)
                .Build()
                .Run();
        }
    }
}