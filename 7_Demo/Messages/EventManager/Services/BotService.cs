using System;
using System.Threading.Tasks;
using Events;
using Microsoft.Extensions.Logging;
using Rebus.Bus;

namespace EventManager.Services
{
    public interface IBotService
    {
        Task SpeakerApplied(long chatId, string speakerFullName, string draftTopicName);

        Task SpeakerPrepared(long chatId, string speakerFullName, string speakerInfo, string topicName);

        Task SponsorConfirmed(long chatId, string name, string address);

        Task MeetupAnnounced(long chatId, string topicName, string speakerInfo, string speakerFullName, string address,
            DateTime dateTime);

        Task MeetupIsOver(long chatId, DateTime dateTime);

        Task FeedbackFormRequested(long chatId, string speakerFullName, string topicName, string url);

        Task SpeakerFeedbackProvided(long chatId, string speakerFullName, string notes);

        Task OrganizerFeedbackProvided(long chatId, string notes);
    }

    public class BotService : IBotService
    {
        private readonly IBus _bus;
        private readonly IDbService _dbService;
        private readonly ILogger<BotService> _logger;

        public BotService(IBus bus, ILogger<BotService> logger, IDbService dbService)
        {
            _bus = bus;
            _logger = logger;
            _dbService = dbService;
        }

        public async Task SpeakerApplied(long chatId, string speakerFullName, string draftTopicName)
        {
            await _dbService.InsertContent($"{nameof(SpeakerApplied)} {speakerFullName} {draftTopicName}");

            _logger.LogInformation($"Received message: {nameof(SpeakerApplied)}, chatId: {chatId}.");
            await _bus.Publish(new SpeakerApplied(speakerFullName, draftTopicName));
        }

        public async Task SpeakerPrepared(long chatId, string speakerFullName, string speakerInfo, string topicName)
        {
            await _dbService.InsertContent($"{nameof(SpeakerPrepared)} {speakerFullName} {speakerInfo} {topicName}");

            _logger.LogInformation($"Received message: {nameof(SpeakerPrepared)}, chatId: {chatId}.");
            await _bus.Publish(new SpeakerPrepared(speakerFullName, topicName, speakerInfo));
        }

        public async Task SponsorConfirmed(long chatId, string name, string address)
        {
            await _dbService.InsertContent($"{nameof(SponsorConfirmed)} {name} {address}");

            _logger.LogInformation($"Received message: {nameof(SponsorConfirmed)}, chatId: {chatId}.");
            await _bus.Publish(new SponsorConfirmed(name, address));
        }

        public async Task MeetupAnnounced(long chatId, string topicName, string speakerInfo, string speakerFullName,
            string address,
            DateTime dateTime)
        {
            await _dbService.InsertContent(
                $"{nameof(MeetupAnnounced)} {topicName} {speakerInfo} {speakerFullName} {address} {dateTime}");

            _logger.LogInformation($"Received message: {nameof(MeetupAnnounced)}, chatId: {chatId}.");
            await _bus.Publish(new MeetupAnnounced(topicName, speakerInfo, speakerFullName, address, dateTime));
        }

        public async Task MeetupIsOver(long chatId, DateTime dateTime)
        {
            if ((dateTime - DateTime.Now).Days < 0) throw new InvalidOperationException("Meetup is in the past.");

            await _dbService.InsertContent($"{nameof(MeetupIsOver)} {dateTime}");

            _logger.LogInformation($"Received message: {nameof(MeetupIsOver)}, chatId: {chatId}.");
            await _bus.Publish(new MeetupIsOver(dateTime));
        }

        public async Task FeedbackFormRequested(long chatId, string speakerFullName, string topicName, string url)
        {
            await _dbService.InsertContent($"{nameof(FeedbackFormRequested)} {speakerFullName} {topicName} {url}");

            _logger.LogInformation($"Received message: {nameof(FeedbackFormRequested)}, chatId: {chatId}.");
            await _bus.Publish(new FeedbackFormRequested(speakerFullName, topicName, url));
        }

        public async Task SpeakerFeedbackProvided(long chatId, string speakerFullName, string notes)
        {
            await _dbService.InsertContent($"{nameof(SpeakerFeedbackProvided)} {speakerFullName} {notes}");

            _logger.LogInformation($"Received message: {nameof(SpeakerFeedbackProvided)}, chatId: {chatId}.");
            await _bus.Publish(new SpeakerFeedbackProvided(speakerFullName, notes));
        }

        public async Task OrganizerFeedbackProvided(long chatId, string notes)
        {
            await _dbService.InsertContent($"{nameof(OrganizerFeedbackProvided)} {notes}");

            _logger.LogInformation($"Received message: {nameof(OrganizerFeedbackProvided)}, chatId: {chatId}.");
            await _bus.Publish(new OrganizerFeedbackProvided(notes));
        }
    }
}