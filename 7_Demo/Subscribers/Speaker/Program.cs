using System;
using System.Collections.Generic;
using Events;
using Messages;
using Messages.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.ServiceProvider;

namespace Speaker
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            static ISubscribeService GetSubscribeService()
            {
                return new SubscribeService(new List<Type>
                {
                    typeof(SpeakerFeedbackProvided)
                });
            }

            static void HandlerRegister(IServiceCollection services)
            {
                services.AutoRegisterHandlersFromAssemblyOf<SpeakerFeedbackProvidedHandler>();
            }

            Config.CreateHostBuilder(
                    args,
                    "speaker_queue",
                    GetSubscribeService,
                    HandlerRegister)
                .Build()
                .Run();
        }
    }
}