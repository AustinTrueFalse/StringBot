using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using StringBot.Services;

namespace StringBot.Controllers
{
    public class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {

            string len = " Длина строки";
            string calc = " Калькулятор";

            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).MethodType = callbackQuery.Data;

            // Генерим информационное сообщение
            string methodText = callbackQuery.Data switch
            {
                "len" => len,
                "calc" => calc,
                _ => String.Empty
            };

            // Отправляем в ответ уведомление о выборе

            switch (methodText)
            {
                case " Длина строки":
                    await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                    $"<b>Метод - {methodText}.{Environment.NewLine}</b>" +
                    $"<b>Введите строку, чтобы посчитать кол-во ее символов</b>" +
                    $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
                    break;
                default:
                    await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                    $"<b>Метод - {methodText}.{Environment.NewLine}</b>" +
                    $"<b>Введите числа через пробел, чтобы посчитать их сумму</b>" +
                    $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
                    break;

            }

        }
    }
}
