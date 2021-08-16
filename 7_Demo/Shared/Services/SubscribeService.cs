using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rebus.Bus;

namespace Messages.Services
{
    public class SubscribeService : ISubscribeService
    {
        private readonly IEnumerable<Type> _messagesTypes;

        public SubscribeService(IEnumerable<Type> messagesTypes)
        {
            _messagesTypes = messagesTypes;
        }

        public async Task Subscribe(IBus bus)
        {
            foreach (var messageType in _messagesTypes) await bus.Subscribe(messageType);
        }
    }
}