using System.Threading.Tasks;
using Events;
using JetBrains.Annotations;
using Messages.Extensions;
using Microsoft.Extensions.Configuration;
using Rebus.Handlers;
using Telegram.Bot;

namespace Producer
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
                $"Speaker {message.SpeakerFullName} is applied, let's find a sponsor!");
        }
    }

    [UsedImplicitly]
    public class OrganizerFeedbackProvidedHandler : IHandleMessages<OrganizerFeedbackProvided>
    {
        private readonly ITelegramBotClient _bot;
        private readonly IConfiguration _config;

        public OrganizerFeedbackProvidedHandler(IConfiguration config, ITelegramBotClient bot)
        {
            _config = config;
            _bot = bot;
        }

        public async Task Handle(OrganizerFeedbackProvided message)
        {
            await _bot.SendTextMessageAsync(_config.GetChannel(), message.Notes);
        }
    }
}