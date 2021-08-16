using System.Threading.Tasks;
using Events;
using Messages.Extensions;
using Microsoft.Extensions.Configuration;
using Rebus.Handlers;
using Telegram.Bot;

namespace Speaker
{
    public class SpeakerFeedbackProvidedHandler : IHandleMessages<SpeakerFeedbackProvided>
    {
        private readonly ITelegramBotClient _bot;
        private readonly IConfiguration _config;

        public SpeakerFeedbackProvidedHandler(IConfiguration config, ITelegramBotClient bot)
        {
            _config = config;
            _bot = bot;
        }

        public async Task Handle(SpeakerFeedbackProvided message)
        {
            await _bot.SendTextMessageAsync(
                _config.GetChannel(),
                $"{message.SpeakerFullName}, {message.Notes})");
        }
    }
}