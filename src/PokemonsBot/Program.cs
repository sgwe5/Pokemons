using PokemonsBot.Core.Bot;
using PokemonsBot.Core.Bot.Commands.CommandHandler;
using PokemonsBot.Core.Settings;
using PokemonsBot.TransferClient.RabbitMQ;
using PokemonsDomain.MessageBroker.Models;
using PokemonsDomain.MessageBroker.Sender;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

var builder = WebApplication.CreateBuilder();

if (builder.Environment.EnvironmentName == Environments.Development)
    builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddSingleton<BotClient>();

builder.Services.AddSingleton<ICommandHandler, CommandHandler>();

builder.Services.AddSingleton<IBrokerSender>(option => 
    new RabbitMqSender(builder.Configuration.GetConnectionString("RabbitMQ") 
                       ?? throw new ArgumentException("Broker path cannot be null"),
        option.GetService<ILogger<RabbitMqSender>>()!));

builder.Services.Configure<BotOption>(builder.Configuration.GetSection("BotOption"));

var app = builder.Build();

var commandHandler = app.Services.GetRequiredService<ICommandHandler>();

commandHandler.RegisterCommand(async context =>
{
    long refId = 0;
    var text = context.Update.Message!.Text ?? "";
    var query = text.Split(' ');
    if (query.Length > 1)
        if (!long.TryParse(query[1], out refId)) refId = 0;
    
    var data = new CreateUserModel
    {
        Hash = "",
        Name = context.Update.Message.Chat.FirstName,
        Surname = context.Update.Message.Chat.LastName,
        RefId = refId,
        Username = context.Update.Message.Chat.Username,
        UserId = context.ChatId
    };
    
    /*
    var photos = await context.Client.GetUserProfilePhotosAsync(context.ChatId, 
        cancellationToken: context.StoppingToken);
    if (photos.TotalCount > 0)
    {
        var currentPhoto = photos.Photos[0][0];
        var file = await context.Client.GetFileAsync(currentPhoto.FileId);
        await using var fileSavingStream = 
    }
    */

    var broker = app.Services.GetService<IBrokerSender>()!;

    await broker.Send(data);
            
    await context.Client.SendTextMessageAsync(context.Update.Message.Chat.Id,
        $"Welcome to pokemons",
        replyMarkup: new InlineKeyboardMarkup([
            InlineKeyboardButton.WithWebApp("open", new WebAppInfo
            {
                Url = builder.Configuration["BotOption:WebAppLink"] 
                      ?? throw new ArgumentException("Link cannot be null")
            }), 
        ]),
        cancellationToken: context.StoppingToken);
}).AddFilter(command => command.StartsWith("start"));

commandHandler.RegisterCommand(async context =>
{
    
}).AddFilter(command => command.StartsWith("referrals"));

app.Run();