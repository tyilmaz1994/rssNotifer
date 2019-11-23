using notifier.bl.const_;
using notifier.dal.entities;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace notifier.bl.helpers
{
    public static class TelegramHelper
    {
        /// <summary>
        /// Creates menu that has one row.
        /// </summary>
        /// <param name="menuNames">what items menu has it ?</param>
        /// <returns>Telegram menu</returns>
        public static InlineKeyboardMarkup CreateMenu(string[] menuNames)
        {
            InlineKeyboardButton[][] keyboardButtons = new InlineKeyboardButton[1][];

            keyboardButtons[0] = new InlineKeyboardButton[menuNames.Length];

            for (int i = 0; i < menuNames.Length; i++)
                keyboardButtons[0][i] = InlineKeyboardButton.WithCallbackData(menuNames[i]);

            return new InlineKeyboardMarkup(keyboardButtons);
        }

        public static void SendStartMenu(this ITelegramBotClient telegramBotClient, long chatId)
        {
            telegramBotClient.SendTextMessageAsync(chatId, TelegramBotCommands.MENU_INFORMATION, replyMarkup: CreateMenu(TelegramMenu.BUTTONS_SUBSCRIBE_UNSUBSCRIBE), parseMode: ParseMode.Html);
        }

        public static void SendStartMenu(this ITelegramBotClient telegramBotClient, long chatId, int msgId)
        {
            telegramBotClient.EditMessageTextAsync(chatId, msgId, TelegramBotCommands.MENU_INFORMATION, replyMarkup: CreateMenu(TelegramMenu.BUTTONS_SUBSCRIBE_UNSUBSCRIBE), parseMode: ParseMode.Html);
        }

        public static InlineKeyboardMarkup ToMenu(this IList<UserRss> list, string key)
        {
            InlineKeyboardButton[][] keyboardButtons = new InlineKeyboardButton[list.Count + 1][];

            for (int i = 0; i < list.Count; i++)
            {
                keyboardButtons[i] = new InlineKeyboardButton[1];
                keyboardButtons[i][0] = InlineKeyboardButton.WithCallbackData(list[i].Url, string.Concat(key, " ", list[i].Id));
            }

            keyboardButtons[list.Count] = new InlineKeyboardButton[1];
            keyboardButtons[list.Count][0] = InlineKeyboardButton.WithCallbackData(TelegramBotCommands.CALLBACK_HOME);

            return new InlineKeyboardMarkup(keyboardButtons);
        }

        public static InlineKeyboardMarkup ToMenu(this IList<TelegramGroup> list, string key)
        {
            InlineKeyboardButton[][] keyboardButtons = new InlineKeyboardButton[list.Count + 1][];

            for (int i = 0; i < list.Count; i++)
            {
                keyboardButtons[i] = new InlineKeyboardButton[1];
                keyboardButtons[i][0] = InlineKeyboardButton.WithCallbackData(string.Concat(list[i].Title, (string.IsNullOrEmpty(list[i].Username) ? "" : ", @"), list[i].Username), string.Concat(key, " ", list[i].Id));
            }

            keyboardButtons[list.Count] = new InlineKeyboardButton[1];
            keyboardButtons[list.Count][0] = InlineKeyboardButton.WithCallbackData(TelegramBotCommands.CALLBACK_HOME);

            return new InlineKeyboardMarkup(keyboardButtons);
        }

        public static bool CallbackIsEqualTo(this string[] callbackData, string key)
        {
            return callbackData.Length > 1 && callbackData.First() == key;
        }

        public static string RemoveFirstChar(this string str)
        {
            return str.Remove(0, 1);
        }
    }
}