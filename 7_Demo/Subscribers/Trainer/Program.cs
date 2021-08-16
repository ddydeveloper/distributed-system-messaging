using System;
using System.Collections.Generic;
using Events;
using Messages;
using Messages.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.ServiceProvider;

namespace Trainer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            static ISubscribeService GetSubscribeService()
            {
                return new SubscribeService(new List<Type>
                {
                    typeof(SpeakerApplied)
                });
            }

            static void HandlerRegister(IServiceCollection services)
            {
                services.AutoRegisterHandlersFromAssemblyOf<SpeakerAppliedHandler>();
            }

            Config.CreateHostBuilder(
                    args,
                    "trainer_queue",
                    GetSubscribeService,
                    HandlerRegister)
                .Build()
                .Run();
        }
    }
}