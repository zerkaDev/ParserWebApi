using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegram.Informer
{
    public class TelegramBot
    {
        private readonly TelegramBotClient _botClient;
        private DateTime _reshalaStart;
        private DateTime _reshalaStop;
        private const long IdInformationGroup = -823840745;
        private readonly ChatId InformationGroup = new(IdInformationGroup);
        public TelegramBot()
        {
            _botClient = new("5596060706:AAEZT88KO4tpmOMXcNESGJ1OSTOeXNsL6GI");
            _botClient.StartReceiving(Update, Error);

             SendInfoAboutStarted();
        }

        private async Task Update(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            if (message != null)
            {
            }
        }
        private Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            return _botClient.SendTextMessageAsync(InformationGroup, "я сломался(((((((");
            
            //throw new Exception("Telegram bot exception; error method was called");
        }
        #region Informational Messages
        private async void SendInfoAboutStarted()
        {
            string message = "";
            var stringBuilder = new StringBuilder(message);
            stringBuilder.AppendLine("Бот запущен!");
            stringBuilder.AppendLine($"Время инициализации объекта: {DateTime.Now}");
            await _botClient.SendTextMessageAsync(InformationGroup, stringBuilder.ToString());
        }
        public async Task SendMessageAboutDbInitializerStart()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"[{DateTime.Now}] Инициализация базы данных");
            await _botClient.SendTextMessageAsync(InformationGroup, stringBuilder.ToString());
        }
        public async Task SendMessageAboutDbInitializerStops()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"[{DateTime.Now}] Инициализация базы данных завершена");
            await _botClient.SendTextMessageAsync(InformationGroup, stringBuilder.ToString());
        }
        public async Task SendMessageAboutDbWasCreated()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"[{DateTime.Now}] База данных создана");
            await _botClient.SendTextMessageAsync(InformationGroup, stringBuilder.ToString());
        }
        public async Task SendMessageAboutReshalaStartsWorking()
        {
            _reshalaStart = DateTime.Now;
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"[{DateTime.Now}] Решала запущен...");
            await _botClient.SendTextMessageAsync(InformationGroup, stringBuilder.ToString());
        }
        public async Task SendMessageAboutReshalaStopsWorking()
        {
            _reshalaStop = DateTime.Now;
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"[{DateTime.Now}] Решала закончил работу. Общее время выполнения - {(_reshalaStop - _reshalaStart).ToString("m'm 's's'")} секунд");
            await _botClient.SendTextMessageAsync(InformationGroup, stringBuilder.ToString());
        }
        public async Task SendMessageAboutDbHasntValues()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"[{DateTime.Now}] База данных пуста, заполняю");
            await _botClient.SendTextMessageAsync(InformationGroup, stringBuilder.ToString());
        }
        #endregion
    }
}
