using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
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
        private readonly IScheduleSubscribeService _userSubscribeService;
        
        public TelegramInterfaceHostedService(ITelegramBotClient telegramBotClient, IUserService userService
            , ITelegramGroupService telegramGroupService, ILogService logService
            , IUserRssService userRssService, IScheduleSubscribeService userSubscribeService)
        {
            _telegramBotClient = telegramBotClient;
            _userService = userService;
            _telegramGroupService = telegramGroupService;
            _logService = logService;
            _userRssService = userRssService;
            _userSubscribeService = userSubscribeService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _telegramBotClient.OnMessage += Command_Received;
            _telegramBotClient.OnCallbackQuery += Menu_Callbacks;
            _telegramBotClient.StartReceiving();
            return Task.CompletedTask;
        }

        private void Menu_Callbacks(object sender, CallbackQueryEventArgs e)
        {
            Task.Run(() =>
            {
                string callbackData = e.CallbackQuery.Data;
                string[] callbackDataSplitted = callbackData.Split(" ");

                var rssUser = _userService.Get(x => x.TelegramId == e.CallbackQuery.From.Id);

                if (callbackData == TelegramBotCommands.CALLBACK_SUBSCRIBE)
                {
                    //will be rearanged with mongodb aggregation in order to query one time.
                    var userRssList = _userRssService.GetList(x => x.UserId == rssUser.Id);
                    var userGroupCount = _telegramGroupService.GetCollection().Count(x => x.UserId == rssUser.Id);
                    var userSubscribeList = _userSubscribeService.GetList(x => x.UserId == rssUser.Id);

                    userRssList = userRssList.Where(x => (userSubscribeList.Count(y => y.RssId == x.Id) < userGroupCount)).ToList();

                    var customRssMenu = userRssList.ToMenu(TelegramBotCommands.KEY_RSS_SUBSCRIBE);

                    _telegramBotClient.EditMessageTextAsync(e.CallbackQuery.From.Id, e.CallbackQuery.Message.MessageId, TelegramBotCommands.SUBSCRIBE_INFORMATION,
                        replyMarkup: customRssMenu);
                }
                else if (callbackData == TelegramBotCommands.CALLBACK_UNSUBSCRIBE)
                {
                    //will be rearanged with mongodb aggregation in order to query one time.
                    var userRssList = _userRssService.GetList(x => x.UserId == rssUser.Id);
                    var userSubscribeList = _userSubscribeService.GetList(x => x.UserId == rssUser.Id);

                    userRssList = userRssList.Where(x => userSubscribeList.Any(y => y.RssId == x.Id)).ToList();

                    var customRssMenu = userRssList.ToMenu(TelegramBotCommands.KEY_RSS_UNSUBSCRIBE);

                    _telegramBotClient.EditMessageTextAsync(e.CallbackQuery.From.Id, e.CallbackQuery.Message.MessageId, TelegramBotCommands.UNSUBSCRIBE_INFORMATION,
                        replyMarkup: customRssMenu);
                }
                else if (callbackData == TelegramBotCommands.CALLBACK_HOME)
                {
                    _telegramBotClient.SendStartMenu(e.CallbackQuery.From.Id, e.CallbackQuery.Message.MessageId);
                }
                else if (callbackDataSplitted.CallbackIsEqualTo(TelegramBotCommands.KEY_RSS_SUBSCRIBE))
                {
                    //will be rearanged with mongodb aggregation in order to query one time.
                    var userGroupList = _telegramGroupService.GetList(x => x.UserId == rssUser.Id);
                    var userSubscribeList = _userSubscribeService.GetList(x => x.UserId == rssUser.Id);
                    string rssId = callbackData.RemoveFirstChar().RemoveFirstChar();

                    userGroupList = userGroupList.Where(x => !userSubscribeList.Any(y => y.GroupId == x.Id && y.RssId == rssId)).ToList();

                    var customGroupMenu = userGroupList.ToMenu(string.Concat(TelegramBotCommands.KEY_RSS_SUBSCRIBE_GROUP, callbackData.RemoveFirstChar()));

                    _telegramBotClient.EditMessageTextAsync(e.CallbackQuery.From.Id, e.CallbackQuery.Message.MessageId, TelegramBotCommands.SUBSCRIBE_GROUP_INFORMATION,
                        replyMarkup: customGroupMenu, parseMode: ParseMode.Html);
                }
                else if (callbackDataSplitted.CallbackIsEqualTo(TelegramBotCommands.KEY_RSS_UNSUBSCRIBE))
                {
                    //will be rearanged with mongodb aggregation in order to query one time.
                    var userGroupList = _telegramGroupService.GetList(x => x.UserId == rssUser.Id);
                    var userSubscribeList = _userSubscribeService.GetList(x => x.UserId == rssUser.Id);
                    string rssId = callbackData.RemoveFirstChar().RemoveFirstChar();

                    userGroupList = userGroupList.Where(x => userSubscribeList.Any(y => y.GroupId == x.Id && y.RssId == rssId)).ToList();

                    var customGroupMenu = userGroupList.ToMenu(string.Concat(TelegramBotCommands.KEY_RSS_UNSUBSCRIBE_GROUP, " ", callbackData.RemoveFirstChar()));

                    _telegramBotClient.EditMessageTextAsync(e.CallbackQuery.From.Id, e.CallbackQuery.Message.MessageId, TelegramBotCommands.UNSUBSCRIBE_GROUP_INFORMATION,
                        replyMarkup: customGroupMenu);
                }
                else if (callbackDataSplitted.CallbackIsEqualTo(TelegramBotCommands.KEY_RSS_SUBSCRIBE_GROUP))
                {
                    string[] Ids = callbackData.RemoveFirstChar().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    //Index 0: RSS
                    //Index 1: GROUP
                    _userSubscribeService.Save(new dal.entities.UserSubscribe
                    {
                        UserId = rssUser.Id,
                        GroupId = Ids[1],
                        RssId = Ids[0],
                    });

                    _telegramBotClient.EditMessageTextAsync(e.CallbackQuery.From.Id, e.CallbackQuery.Message.MessageId, TelegramBotCommands.USER_SUBSCRIBE_INFORMATION
                        , replyMarkup: TelegramHelper.CreateMenu(TelegramMenu.BUTTONS_SUBSCRIBE_UNSUBSCRIBE));
                }
                else if (callbackDataSplitted.CallbackIsEqualTo(TelegramBotCommands.KEY_RSS_UNSUBSCRIBE_GROUP))
                {
                    string[] Ids = callbackData.RemoveFirstChar().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    //Index 0: RSS
                    //Index 1: GROUP
                    _userSubscribeService.Delete(x => x.RssId == Ids[0] && x.UserId == rssUser.Id && x.GroupId == Ids[1]);
                    _telegramBotClient.EditMessageTextAsync(e.CallbackQuery.From.Id, e.CallbackQuery.Message.MessageId, TelegramBotCommands.USER_UNSUBSCRIBE_INFORMATION
                        , replyMarkup: TelegramHelper.CreateMenu(TelegramMenu.BUTTONS_SUBSCRIBE_UNSUBSCRIBE));
                }
            });
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

                            if(addedGroup != null)
                                _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, TelegramBotCommands.CHANNEL_ADDED_INFORMATION, ParseMode.Html);
                        }
                        catch (Exception ex)
                        {
                            _logService.InsertLog(ex, enums.LogLevel.TELEGRAM);
                            _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, TelegramBotCommands.INSERT_CHANNEL_INFORMATION_ERROR, ParseMode.Html);
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

                            if(userRss != null)
                                _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, TelegramBotCommands.RSS_ADDED_INFORMATION, ParseMode.Html);
                        }
                        else
                        {
                            _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, TelegramBotCommands.RSS_IS_NOT_VALID);
                        }
                    }
                    else if(e.Message.Text == TelegramBotCommands.MENU)
                    {
                        _telegramBotClient.SendStartMenu(e.Message.Chat.Id);
                    }
                    else if (e.Message.Text == TelegramBotCommands.HELP || e.Message.Text == TelegramBotCommands.START)
                    {
                        _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, TelegramBotCommands.HELP_INFORMATION, ParseMode.Html);
                    }
                    else if (e.Message.Text == TelegramBotCommands.ABOUT_ME)
                    {
                        _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, TelegramBotCommands.ABOUT_ME_INFORMATION, ParseMode.Html);
                    }
                    else if (e.Message.Text == TelegramBotCommands.INSERT_RSS)
                    {
                        _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, TelegramBotCommands.INSERT_RSS_INFORMATION, ParseMode.Html);
                    }
                    else if (e.Message.Text == TelegramBotCommands.INSERT_CHANNEL)
                    {
                        _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, TelegramBotCommands.INSERT_CHANNEL_INFORMATION, ParseMode.Html);
                    }
                    else if (e.Message.Text.StartsWith('@'))
                    {
                        try
                        {
                            ChatId chatId = new ChatId(e.Message.Text.Trim());
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

                            if (addedGroup != null)
                                _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, TelegramBotCommands.CHANNEL_ADDED_INFORMATION, ParseMode.Html);
                        }
                        catch (Exception ex)
                        {
                            _logService.InsertLog(ex, enums.LogLevel.TELEGRAM);
                            _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, TelegramBotCommands.INSERT_CHANNEL_INFORMATION_ERROR, ParseMode.Html);
                        }
                    }
                    else if(ValidationHelper.IsRss(e.Message.Text, _logService))
                    {
                        var userRss = _userRssService.AddRssIfNotExist(new dal.entities.UserRss
                        {
                            Url = e.Message.Text.Trim(),
                            UserId = requestUser.Id,
                        });

                        if (userRss != null)
                            _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, TelegramBotCommands.RSS_ADDED_INFORMATION, ParseMode.Html);
                    }
                    else
                    {
                        _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, TelegramBotCommands., ParseMode.Html);
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
            _telegramBotClient.OnCallbackQuery -= Menu_Callbacks;
            _telegramBotClient.StopReceiving();
            return Task.CompletedTask;
        }
    }
}