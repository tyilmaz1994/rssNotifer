using Microsoft.Extensions.Hosting;
using notifier.bl.const_;
using notifier.bl.helpers;
using notifier.bl.services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace notifier.bl.hostedServices
{
    public class TelegramInterfaceHostedService : IHostedService
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IUserService _userService;
        private readonly ITelegramGroupService _telegramGroupService;
        private readonly ILogService _logService;
        private readonly IUserRssService _userRssService;

        public TelegramInterfaceHostedService(ITelegramBotClient telegramBotClient, IUserService userService
            , ITelegramGroupService telegramGroupService, ILogService logService
            , IUserRssService userRssService)
        {
            _telegramBotClient = telegramBotClient;
            _userService = userService;
            _telegramGroupService = telegramGroupService;
            _logService = logService;
            _userRssService = userRssService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _telegramBotClient.OnMessage += Command_Received;
            _telegramBotClient.StartReceiving();
            return Task.CompletedTask;
        }

        private void Command_Received(object sender, MessageEventArgs e)
        {
            Task.Run(() =>
            {
                var requestUser = _userService.AddUserIfNotExist(new dal.entities.User
                {
                    Username = e.Message.From.Username,
                    TelegramId = e.Message.From.Id,
                    Firstname = e.Message.From.FirstName,
                    Lastname = e.Message.From.LastName,
                });

                if (e.Message.Type == MessageType.Text)
                {
                    if (e.Message.Text.Trim().StartsWith(TelegramBotCommands.INSERT_CHANNEL) && e.Message.Text != TelegramBotCommands.INSERT_CHANNEL)
                    {
                        try
                        {
                            ChatId chatId = new ChatId(e.Message.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
                            Chat chat = _telegramBotClient.GetChatAsync(chatId).Result;
                            var addedGroup = _telegramGroupService.AddGroupIfNotExist(new dal.entities.TelegramGroup
                            {
                                ChatId = chat.Id,
                                UserId = requestUser.Id,
                                Firstname = chat.FirstName,
                                Description = chat.Description,
                                Lastname = chat.LastName,
                                Title = chat.Title,
                                Username = chat.Username,
                            });
                        }
                        catch (Exception ex)
                        {
                            _logService.InsertLog(ex, enums.LogLevel.TELEGRAM);
                            _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, ex.Message);
                        }
                    }
                    else if (e.Message.Text.Trim().StartsWith(TelegramBotCommands.INSERT_RSS) && e.Message.Text != TelegramBotCommands.INSERT_RSS)
                    {
                        string rss = e.Message.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Last();

                        if (ValidationHelper.IsRss(rss, _logService))
                        {
                            var userRss = _userRssService.AddRssIfNotExist(new dal.entities.UserRss
                            {
                                Url = rss,
                                UserId = requestUser.Id,
                            });
                        }
                        else
                        {
                            _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, TelegramBotCommands.RSS_IS_NOT_VALID);
                        }
                    }
                    else if (e.Message.Text == TelegramBotCommands.HELP || e.Message.Text == TelegramBotCommands.START)
                    {
                        _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, TelegramBotCommands.HELP_INFORMATION);
                    }
                }
                else if (e.Message.Type == MessageType.GroupCreated || e.Message.Type == MessageType.ChatMembersAdded)
                {
                    var addedGroup = _telegramGroupService.AddGroupIfNotExist(new dal.entities.TelegramGroup
                    {
                        ChatId = e.Message.Chat.Id,
                        UserId = requestUser.Id,
                        Firstname = e.Message.Chat.FirstName,
                        Lastname = e.Message.Chat.LastName,
                        Description = e.Message.Chat.Description,
                        Title = e.Message.Chat.Title,
                        Username = e.Message.Chat.Username,
                    });
                }
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _telegramBotClient.OnMessage -= Command_Received;
            _telegramBotClient.StopReceiving();
            return Task.CompletedTask;
        }
    }
}