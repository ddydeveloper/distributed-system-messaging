using System;
using System.Threading.Tasks;
using Events;
using JetBrains.Annotations;
using Messages.Extensions;
using Microsoft.Extensions.Configuration;
using Rebus.Handlers;
using Telegram.Bot;

namespace Audience
{
    public class FeedbackFormRequestedHandler : IHandleMessages<FeedbackFormRequested>
    {
        private readonly ITelegramBotClient _bot;
        private readonly IConfiguration _configuration;

        public FeedbackFormRequestedHandler(IConfiguration configuration, ITelegramBotClient bot)
        {
            _configuration = configuration;
            _bot = bot;
        }

        public async Task Handle(FeedbackFormRequested message)
        {
            var text =
                $"Please provide your feedback in the form below ({message.SpeakerFullName} \"{message.TopicName}\")" +
                $"{Environment.NewLine}{message.Url}";

            await _bot.SendTextMessageAsync(_configuration.GetChannel(), text);
        }
    }

    [UsedImplicitly]
    public class MeetupAnnouncedHandler : IHandleMessages<MeetupAnnounced>
    {
        private readonly ITelegramBotClient _bot;
        private readonly IConfiguration _config;

        public MeetupAnnouncedHandler(IConfiguration config, ITelegramBotClient bot)
        {
            _config = config;
            _bot = bot;
        }

        public async Task Handle(MeetupAnnounced message)
        {
            var text = $"Meetup will take a place at {message.DateTime:g} ({message.Address})" +
                       $"{Environment.NewLine}{Environment.NewLine}{message.SpeakerFullName} ({message.SpeakerInfo}) " +
                       $"{Environment.NewLine}will present a topic \"{message.TopicName}\".";

                       await _bot.SendTextMessageAsync(_config.GetChannel(), text);
        }
    }

    [UsedImplicitly]
    public class MeetupIsOverHandler : IHandleMessages<MeetupIsOver>
    {
        private readonly ITelegramBotClient _bot;
        private readonly IConfiguration _config;

        public MeetupIsOverHandler(IConfiguration config, ITelegramBotClient bot)
        {
            _config = config;
            _bot = bot;
        }

        public async Task Handle(MeetupIsOver message)
        {
            await _bot.SendTextMessageAsync(
                _config.GetChannel(),
                "Meetup is over! Please, do not forget to provide your feedback.");
        }
    }
}