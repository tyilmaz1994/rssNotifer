using notifier.bl.const_;
using notifier.bl.helpers;
using notifier.bl.services;
using Quartz;
using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using Telegram.Bot;

namespace notifier.bl.hostedServices.jobs
{
    /// <summary>
    /// Gets RSS news over link and check that if any news is added to rss then notify user
    /// </summary>
    public class RssNewsJob : IJob
    {
        private readonly IUserRssService _userRssService;
        private readonly IUserService _userService;
        private readonly ITelegramGroupService _telegramGroupService;
        private readonly IUserSubscribeService _userSubscribeService;
        private readonly ILogService _logService;
        private readonly ITelegramBotClient _telegramBotClient;

        public RssNewsJob(IUserRssService userRssService, IUserSubscribeService userSubscribeService
            , ITelegramGroupService telegramGroupService, IUserService userService, ILogService logService
            , ITelegramBotClient telegramBotClient)
        {
            _userRssService = userRssService;
            _telegramGroupService = telegramGroupService;
            _userService = userService;
            _userSubscribeService = userSubscribeService;
            _logService = logService;
            _telegramBotClient = telegramBotClient;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var mappedData = context.MergedJobDataMap;
                var rss = _userRssService.Get(x => x.Id == mappedData.GetString(ScheduleConsts.RSS_ID));
                var subscription = _userSubscribeService.Get(x => x.Id == context.Trigger.Key.Name);

                var latestNews = RssHelper.GetLatestUpdates(_logService, rss.Url, subscription.CheckDate);

                Console.WriteLine(subscription.CheckDate);

                foreach (var item in latestNews)
                    Console.WriteLine(item.PublishDate.ToUniversalTime());

                if(latestNews.Any())
                {
                    var user = _userService.Get(x => x.Id == mappedData.GetString(ScheduleConsts.USER_ID));
                    var group = _telegramGroupService.Get(x => x.Id == mappedData.GetString(ScheduleConsts.GROUP_ID));

                    foreach (var item in latestNews)
                    {
                        string link;

                        if (item.Links.FirstOrDefault() == null)
                            link = ((TextSyndicationContent)item.Content).Text;
                        else
                            link = item.Links.FirstOrDefault().Uri.AbsoluteUri;

                        _telegramBotClient.SendTextMessageAsync(group.ChatId, link);

                        _logService.Save(new dal.entities.NotifierLog
                        {
                            LogLevel = (short)enums.LogLevel.RSS_SENT_INFO,
                            Message = string.Concat(rss.UserId, " ", group.ChatId),
                            StackTrace = link,
                        });
                    }

                    subscription.CheckDate = DateTime.Now;
                    _userSubscribeService.Save(subscription);
                }
            }
            catch (Exception ex)
            {
                _logService.InsertLog(ex, enums.LogLevel.RSS_READ_ERROR);
            }
        }
    }
}