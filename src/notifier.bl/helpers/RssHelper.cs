using notifier.bl.services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace notifier.bl.helpers
{
    public static class RssHelper
    {
        public static IList<SyndicationItem> GetLatestUpdates(ILogService logService, string rssUrl, DateTime checkDate)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(new MemoryStream(ValidationHelper.RssInByte(rssUrl))))
                {
                    var items = SyndicationFeed.Load(reader);
                     return items.Items.Where(x => x.PublishDate.UtcDateTime > checkDate).ToList();
                }
            }
            catch (Exception ex)
            {
                logService?.InsertLog(ex, enums.LogLevel.RSS_READ_ERROR);
                return new List<SyndicationItem>();
            }
        }
    }
}
