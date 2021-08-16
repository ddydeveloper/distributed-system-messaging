using System;
using System.Collections.Generic;
using Events;
using Messages;
using Messages.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.ServiceProvider;

namespace Audience
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            static ISubscribeService GetSubscribeService()
            {
                return new SubscribeService(new List<Type>
                {
                    typeof(FeedbackFormRequested),
                    typeof(MeetupAnnounced),
                    typeof(MeetupIsOver)
                });
            }

            static void HandlerRegister(IServiceCollection services)
            {
                services.AutoRegisterHandlersFromAssemblyOf<FeedbackFormRequestedHandler>();
            }

            Config.CreateHostBuilder(
                    args,
                    "audience_queue",
                    GetSubscribeService,
                    HandlerRegister)
                .Build()
                .Run();
        }
    }
}