using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Microsoft.VisualBasic;
using System.Threading;
using StringBot.Services;

namespace StringBot.Controllers
{
    public class TextMessageController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;
        public Calculator _calculator;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
            _calculator = new Calculator();
        }

        public async Task Handle(Message message, CancellationToken ct)
        {

            switch (message.Text)
            {
                case "/start":

                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"Длина строки", $"len"),
                        InlineKeyboardButton.WithCallbackData($"Калькулятор", $"calc")
                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Наш бот считает длину строки и складывает в ней числа.</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}Выберете метод, которым хотите воспользоваться.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;
                default:
                    string userMethodType = _memoryStorage.GetSession(message.Chat.Id).MethodType; // Здесь получим язык из сессии пользователя

                    switch (userMethodType)
                    {
                        case "len":
                            await _telegramClient.SendTextMessageAsync(message.From.Id, $"Длина сообщения: {message.Text.Length} знаков");
                            break;
                        default:

                            int summ = _calculator.Sum(message.Text);
               
                            await _telegramClient.SendTextMessageAsync(message.From.Id, $"Сумма чисел: {summ}");
                            break;
                    }
                    break;

            }
        }
    }
}
