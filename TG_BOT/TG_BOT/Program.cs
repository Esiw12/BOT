using System;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TG_BOT
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var botClient = new TelegramBotClient("5758951049:AAFD6BCcrdlqHSfWUxF7syESPZKvPAojcSU");

            using CancellationTokenSource cts = new();
            var kol = "Blender модель 400-1500\r\n" + "JS 400-1500";
            var sost = "Здравствуй, мученник :)\r\n" + "TG @BronPluzy , @esiw_yar";

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            cts.Cancel();




            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                if (update.Message is not { } message)
                    return;

                if (message.Text is not { } messageText)
                    return;

                var chatId = message.Chat.Id;
                Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");


                ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                {
                new KeyboardButton[] { "Цены " },
                new KeyboardButton[] { "Примеры работ" },
                new KeyboardButton[] {"Скидки"},
                new KeyboardButton[] {"Контакты"}
})
                {
                    ResizeKeyboard = true
                };




                if (messageText == "Контакты" || messageText == "4") //4
                {
                    await botClient.SendTextMessageAsync(chatId, sost, cancellationToken: cancellationToken);

                    await botClient.SendPhotoAsync(
                     chatId: chatId,
                    photo: InputFile.FromUri("https://github.com/Esiw12/ZADACHI_ALG/blob/main/COMANDA.jpg?raw=true"),
                    caption: "Friends",
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken);

                }
                if (messageText == "Скидки" || messageText == "2") //2
                {
                    await botClient.SendTextMessageAsync(chatId, "Скидок нет,Я Даже с друзей беру по 1к за анимацию.", cancellationToken: cancellationToken);
                }
                if (messageText == "Примеры работ Blender" || messageText == "1" || messageText == "Примеры работ") //1
                {

                    await botClient.SendVideoAsync(
                    chatId: chatId,
                        video: InputFile.FromUri("https://github.com/Esiw12/ZADACHI_ALG/blob/main/Tank.mp4"),

                    thumbnail: InputFile.FromUri("https://raw.githubusercontent.com/TelegramBots/book/master/src/2/docs/thumb-clock.jpg"),
                    supportsStreaming: true,
                    cancellationToken: cancellationToken);
                    await botClient.SendMediaGroupAsync(
                    chatId: chatId,
                    media: new IAlbumInputMedia[]
                     {
                      new InputMediaPhoto(
                        InputFile.FromUri("https://github.com/Esiw12/ZADACHI_ALG/blob/main/photo_2023-09-21_15-53-52.jpg?raw=true")),
                        new InputMediaPhoto(
                         InputFile.FromUri("https://github.com/Esiw12/ZADACHI_ALG/blob/main/photo_2023-10-07_16-03-36.jpg?raw=true")),
                     },
                        cancellationToken: cancellationToken); //АЛЬбом


                }
                if (messageText == "Примеры работ JS " || messageText == "5" || messageText == "Примеры работ")
                {
                    await botClient.SendMediaGroupAsync(
                    chatId: chatId,
                media: new IAlbumInputMedia[]
                {
                    new InputMediaPhoto(
                    InputFile.FromUri("https://github.com/Esiw12/ZADACHI_ALG/blob/main/photo_2023-10-11_19-42-01.jpg?raw=true")),
                    new InputMediaPhoto(
                    InputFile.FromUri("https://github.com/Esiw12/ZADACHI_ALG/blob/main/photo_2023-10-11_19-46-48.jpg?raw=true")),
                },
                cancellationToken: cancellationToken);
                }
                if (messageText == "Цены" || messageText == "3") //3
                {
                    await botClient.SendTextMessageAsync(chatId, kol, cancellationToken: cancellationToken);


                }
            }

            static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }
        }
    }
}
