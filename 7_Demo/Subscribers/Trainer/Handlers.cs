using System.Threading.Tasks;
using Events;
using Messages.Extensions;
using Microsoft.Extensions.Configuration;
using Rebus.Handlers;
using Telegram.Bot;

namespace Trainer
{
    public class SpeakerAppliedHandler : IHandleMessages<SpeakerApplied>
    {
        private readonly ITelegramBotClient _bot;
        private readonly IConfiguration _config;

        public SpeakerAppliedHandler(IConfiguration config, ITelegramBotClient bot)
        {
            _config = config;
            _bot = bot;
        }

        public async Task Handle(SpeakerApplied message)
        {
            await _bot.SendTextMessageAsync(
                _config.GetChannel(),
                $"There is a speaker to be prepared: {message.SpeakerFullName}.");
        }
    }
}