#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using EventManager.Services;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace EventManager
{
    public class Worker : BackgroundService
    {
        private readonly ITelegramBotClient _bot;
        private readonly IBotService _botService;

        public Worker(ITelegramBotClient bot, IBotService botService)
        {
            _bot = bot;
            _botService = botService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bot.OnMessage += Bot_OnMessage;
            _bot.StartReceiving(cancellationToken: stoppingToken);

            while (!stoppingToken.IsCancellationRequested) await Task.Delay(1000, stoppingToken);
        }

        private async void Bot_OnMessage(object? sender, MessageEventArgs e)
        {
            if (e.Message.Text is null) return;

            var chatId = e.Message.Chat.Id;
            var text = e.Message.Text;
            var args = text.Split('|');
            var type = args[0];

            switch (type)
            {
                case "SpeakerApplied":
                    await _botService.SpeakerApplied(chatId, args[1], args[2]);
                    break;
                case "SpeakerPrepared":
                    await _botService.SpeakerPrepared(chatId, args[1], args[2], args[3]);
                    break;
                case "SponsorConfirmed":
                    await _botService.SponsorConfirmed(chatId, args[1], args[2]);
                    break;
                case "MeetupAnnounced":
                {
                    var timeParts = args[5].Split('-');
                    var dateTime = new DateTime(int.Parse(timeParts[2]), int.Parse(timeParts[1]),
                        int.Parse(timeParts[0]));
                    await _botService.MeetupAnnounced(chatId, args[1], args[2], args[3], args[4], dateTime);
                    break;
                }
                case "MeetupIsOver":
                {
                    var timeParts = args[1].Split('-');
                    var dateTime = new DateTime(int.Parse(timeParts[2]), int.Parse(timeParts[1]),
                        int.Parse(timeParts[0]));
                    await _botService.MeetupIsOver(chatId, dateTime);
                    break;
                }
                case "FeedbackFormRequested":
                    await _botService.FeedbackFormRequested(chatId, args[1], args[2], args[3]);
                    break;
                case "SpeakerFeedbackProvided":
                    await _botService.SpeakerFeedbackProvided(chatId, args[1], args[2]);
                    break;
                case "OrganizerFeedbackProvided":
                    await _botService.OrganizerFeedbackProvided(chatId, args[1]);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown message type {text}");
            }
        }
    }
}