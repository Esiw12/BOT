using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("5758951049:AAFD6BCcrdlqHSfWUxF7syESPZKvPAojcSU");

using CancellationTokenSource cts = new();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
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

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)

{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;
    var kol = "Blender модель 400-1500\r\n" + "JS 400-1500";
    var sost = "Здравствуй, мученник :)\r\n" + "TG @BronPluzy , @esiw_yar";
    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
{
    new KeyboardButton[] {"Примеры работ JS", "Примеры работ Blender", "Скидки", "Контакты","Цены"},
})
    {
        ResizeKeyboard = true
    };

    Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Делай заказ как можно скорее",
        replyMarkup: replyKeyboardMarkup,
        cancellationToken: cancellationToken);

    // Echo received message text
    Message sentMessage1 = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Чем быстрее оформишь заказ,тем быстрее работа будет выполнена",
        cancellationToken: cancellationToken);

    if (messageText == "Контакты") //4
    {
        await botClient.SendTextMessageAsync(chatId, sost, cancellationToken: cancellationToken);

        await botClient.SendPhotoAsync(
         chatId: chatId,
        photo: InputFile.FromUri("https://github.com/Esiw12/ZADACHI_ALG/blob/main/COMANDA.jpg?raw=true"),
        caption: "Friends",
        parseMode: ParseMode.Html,
        cancellationToken: cancellationToken);


    }
    if (messageText == "Скидки") //2
    {
        await botClient.SendTextMessageAsync(chatId, "Скидок нет,Я Даже с друзей беру по 1к за анимацию.", cancellationToken: cancellationToken);
    }
    if (messageText == "Примеры работ Blender") //1
    {

        await botClient.SendVideoAsync(
        chatId: chatId,
            video: InputFile.FromUri("https://github.com/Esiw12/ZADACHI_ALG/raw/main/Tank.mp4"),

        thumbnail: InputFile.FromUri("https://github.com/Esiw12/ZADACHI_ALG/raw/main/Tank.mp4"),
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
    if (messageText == "Примеры работ JS")
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
    if (messageText == "Цены") //3
    {
        await botClient.SendTextMessageAsync(chatId, kol, cancellationToken: cancellationToken);


    }


}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}