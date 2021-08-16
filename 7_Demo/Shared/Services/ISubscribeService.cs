using System.Threading.Tasks;
using Rebus.Bus;

namespace Messages.Services
{
    public interface ISubscribeService
    {
        public Task Subscribe(IBus bus);
    }
}