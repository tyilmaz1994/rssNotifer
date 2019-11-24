namespace notifier.bl.const_
{
    public class TelegramBotCommands
    {
        public const string INSERT_RSS = "/rss";
        public const string INSERT_CHANNEL = "/channel";
        public const string MENU = "/menu";
        public const string HELP = "/help";
        public const string START = "/start";
        public const string ABOUT_ME = "/aboutme";

        public const string HELP_INFORMATION = "Hello. I'm RSS Reader.\nYou can get notification using my system from any RSS.\nI have four commands as follows:\n\n/rss with a parameter. I need rss links in order to notify you. You can add rss using /rss command. e.g.:\n/rss https://www.npr.org/rss/podcast.php?id=510298\nThis <b>command work with a parameter</b>. If you use without parameter, <b>nothing will be happened.</b>\n\n/channel with a parameter. I also need channels or groups in order to notify to any group or channel. Before using /channel command. You must invite me to your channel with <b>administrator privilege</b>.\nNext step is to use /channel command with a parameter. <b>The parameter is channel username</b> which the bot(its me: @RssServiceBot) is invited e.g.:\n/channel @channel_username\nYou can find your channel username from address bar. When you select your channel, link will be ends with <i>''im?p=@channel_username''</i>\n\nWhen the matter is come to <b>telegram groups</b>, its enough to invite me(@RssServiceBot) to your group. I can find out...\n\n/menu command. it's parameterless.\nThe last one thing is start to getting notification./menu command help with that.\n\n/help command is parameterless and guide you to use RSS Reader.";
        public const string MENU_INFORMATION = "Manage your subscriptions.\n<b>Subscribe:</b> list your added RSS' as button. If you choose one of them, then channels and groups will be listed in order to start receiving notification. <b>Chosen group/channel will be got notification</b>.\n<b>Unsubscribe:</b> if you want to stop receiving notification from rss, then choose which rss and which group/channel do you want to stop";
        public const string SUBSCRIBE_INFORMATION = "Select one of the below in order to get notification. If below list is empty, then you did not add any RSS. try /rss rss_link command.";
        public const string SUBSCRIBE_GROUP_INFORMATION = "<b>One last step:</b> selected channel/group gets notification every 5 min If any news written to RSS. If below list is empty, then you did not add any channel/group. try /channel @channel_username or invite me to any group with <b>administrator privilege</b>.";
        public const string UNSUBSCRIBE_INFORMATION = "Do you want to stop receiving notification? Choose one of the below. If below list is empty, then you did not subscribe any RSS.";
        public const string UNSUBSCRIBE_GROUP_INFORMATION = "the selected RSS sends notification to below groups/channels. If you want to stop sending RSS news, then choose one of the below.";
        public const string CHANNEL_ADDED_INFORMATION = "Channel is successfully added. <b>If you added any rss link</b>, go head start getting notification using /menu command.";
        public const string RSS_ADDED_INFORMATION = "RSS is successfuly added. Did you add any channel or group using /channel or inviting bot(@RssServiceBot) to telegram group ? If answer is <b>yes</b>, then you can start receiving notitification using /menu command.\n\n<b>If you did not invite me any group or channel, then you may not see your rss list in Subscribe menu</b>";
        public const string ABOUT_ME_INFORMATION = "contact me\n\n- @tunahanyilmaz\n- https://www.linkedin.com/in/tunahanyilmaz/\n- https://github.com/tyilmaz1994\n- tunahan.yilmaz94@gmail.com";
        public const string USER_SUBSCRIBE_INFORMATION = "Successfuly subscribed.";
        public const string USER_UNSUBSCRIBE_INFORMATION = "Successfuly unsubscribed.";
        public const string INSERT_RSS_INFORMATION = "Send me to rss link";
        public const string INSERT_CHANNEL_INFORMATION = "Send me to channel username with @ prefix";
        public const string INSERT_CHANNEL_INFORMATION_ERROR = "Channel is not found. Send me to channel username with @ prefix like @channel_username";
        public const string COMMAND_NOT_FOUND = "Command is not found.";


        public const string CALLBACK_SUBSCRIBE = "Subscribe";
        public const string CALLBACK_UNSUBSCRIBE = "Unsubscribe";
        public const string CALLBACK_HOME = "Home";

        public const string KEY_RSS_SUBSCRIBE = "0";
        public const string KEY_RSS_UNSUBSCRIBE = "1";
        public const string KEY_RSS_SUBSCRIBE_GROUP = "2";
        public const string KEY_RSS_UNSUBSCRIBE_GROUP = "3";

        public const string RSS_IS_NOT_VALID = "I cannot connect to given link. Did you type link correctly ? and is it RSS ? try it out yourself by connecting the link using web browser. If you can connect, then <b>report me</b>.";
    }
}