using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Telegram.Bot;
using TelegramGasBot.Configuration;
using TelegramGasBot.Services.Account;
using TelegramGasBot.Services.Command;
using TelegramGasBot.Services.GasApi;
using TelegramGasBot.Services.Payment;
using TelegramGasBot.Services.Processing;
using TelegramGasBot.Services.Telegram;

namespace TelegramGasBot.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static void AddTelegramBot(this IServiceCollection services)
        {
            var botSettings = services.BuildServiceProvider().GetRequiredService<TelegramBotSettings>();

            var telegramBotClient = new TelegramBotClient(botSettings.ApiToken);

            telegramBotClient.SetWebhookAsync(botSettings.WebhookUrl);

            services.AddTransient<ITelegramBotClient>(client => telegramBotClient);
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<TelegramBotDatabaseSettings>>().Value);
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<TelegramBotSettings>>().Value);

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ICommandService, CommandService>();
            services.AddTransient<ITelegramService, TelegramService>();
            services.AddTransient<IProcessingService, ProcessingService>();
            services.AddTransient<IGasApiService, GasApiService>();
            services.AddTransient<IPaymentService, PaymentService>();
        }

        public static void AddGasApiHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            var gasApiSettings = configuration.GetSection(nameof(GasApiSettings)).Get<GasApiSettings>();

            services.AddHttpClient<IGasApiService, GasApiService>(c =>
            {
                c.BaseAddress = new Uri(gasApiSettings.ApiUrl);
            });
        }
    }
}
