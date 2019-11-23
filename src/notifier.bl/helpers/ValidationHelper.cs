using notifier.bl.services;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;

namespace notifier.bl.helpers
{
    public static class ValidationHelper
    {
        public static bool IsRss(string url, ILogService logService)
        {
            try
            {
                SyndicationFeed syndicationFeed;

                using (XmlReader xr = XmlReader.Create(new MemoryStream(_rssInByte(url))))
                {
                    syndicationFeed = SyndicationFeed.Load(xr);
                }

                return true;
            }
            catch(Exception ex)
            {
                logService?.InsertLog(ex, enums.LogLevel.TELEGRAM);
                return false;
            }
        }

        private static byte[] _rssInByte(string url)
        {
            byte[] datas;
            using (WebClient wc = new WebClient())
            {
                datas = wc.DownloadData(url);

                for (int i = 0; i < datas.Length; i++)
                {
                    if (!datas[i].Equals((byte)XmlCharType.NewLineN) && !datas[i].Equals((byte)XmlCharType.NewLineR) && !datas[i].Equals((byte)XmlCharType.WhiteSpace))
                    {
                        datas = datas.Skip(i).ToArray();
                        break;
                    }
                }
            }

            return datas;
        }

        enum XmlCharType : byte
        {
            NewLineN = 13,
            WhiteSpace = 32,
            NewLineR = 10
        }
    }
}
