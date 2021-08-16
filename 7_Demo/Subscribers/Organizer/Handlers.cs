using System.Threading.Tasks;
using Events;
using JetBrains.Annotations;
using Messages.Extensions;
using Microsoft.Extensions.Configuration;
using Rebus.Handlers;
using Telegram.Bot;

namespace Organizer
{
    public class SpeakerPreparedHandler : IHandleMessages<SpeakerPrepared>
    {
        private readonly ITelegramBotClient _bot;
        private readonly IConfiguration _config;

        public SpeakerPreparedHandler(IConfiguration config, ITelegramBotClient bot)
        {
            _config = config;
            _bot = bot;
        }

        public async Task Handle(SpeakerPrepared message)
        {
            await _bot.SendTextMessageAsync(
                _config.GetChannel(),
                $"Speaker {message.SpeakerFullName} is ready to present a topic \"{message.TopicName}\". We are about to announce the next meetup!");
        }
    }

    [UsedImplicitly]
    public class SponsorConfirmedHandler : IHandleMessages<SponsorConfirmed>
    {
        private readonly ITelegramBotClient _bot;
        private readonly IConfiguration _config;

        public SponsorConfirmedHandler(IConfiguration config, ITelegramBotClient bot)
        {
            _config = config;
            _bot = bot;
        }

        public async Task Handle(SponsorConfirmed message)
        {
            await _bot.SendTextMessageAsync(
                _config.GetChannel(),
                $"Sponsor {message.Name} ({message.Address}) confirmed sponsorship. Meetup can be announced!");
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