using System;
using System.Collections.Generic;
using System.Text;

namespace notifier.bl.const_
{
    public class TelegramBotCommands
    {
        /// <summary>
        /// adds rss via telegrambotClient
        /// </summary>
        public const string INSERT_RSS = "/rss";

        /// <summary>
        /// add channel by channel username
        /// </summary>
        public const string INSERT_CHANNEL = "/channel";

        /// <summary>
        /// information about commands.
        /// eg: /rss /channel
        /// </summary>
        public const string HELP = "/help";
        public const string START = "/start";

        public const string HELP_INFORMATION = "/rss rss_link {adds rss to your list}\n/channel @channel_username {adds channel to bot list. bot must be admin of channel. you can find @channel_username from channel link e.g.:web.tele..org/#/im?p=@channel_username}";

        public const string RSS_IS_NOT_VALID = "RSS_IS_NOT_VALID";
    }
}
