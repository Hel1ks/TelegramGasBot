using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramGasBot.Constants;
using TelegramGasBot.Enums;
using TelegramGasBot.Services.Account;
using TelegramGasBot.Services.Account.Models;

namespace TelegramGasBot.Services.Command
{
    public class CommandService : ICommandService
    {
        private readonly IAccountService accountService;

        public CommandService(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public async Task<GetCommandDto> GetCommandAsync(Update request)
        {
            var chatId = request?.Message?.Chat?.Id;
            var text = request?.Message?.Text;

            if (request.PreCheckoutQuery != null)
            {
                chatId = request.PreCheckoutQuery.From.Id;
                text = request.PreCheckoutQuery.Id;

                var account = await this.accountService.GetAccountByChatIdAsync(chatId);

                return new GetCommandDto()
                {
                    AccountDto = account,
                    Message = text,
                    CommandEnum = UserCommandEnum.PaymentApprovedTab
                };
            }

            if (request.Message.SuccessfulPayment != null)
            {
                chatId = request.Message.Chat.Id;
                text = request.Message.SuccessfulPayment.InvoicePayload;

                var account = await this.accountService.GetAccountByChatIdAsync(chatId);

                return new GetCommandDto()
                {
                    AccountDto = account,
                    Message = text,
                    CommandEnum = UserCommandEnum.PaymentSuccessfullTab
                };
            }

            var accountDto = await this.accountService.GetAccountByChatIdAsync(chatId) ?? new AccountModel() { ChatId = chatId };

            var userState = accountDto?.State?.StateEnum ?? UserStateEnum.NotAddedInDbYet;

            var command = userState switch
            {
                UserStateEnum.NotAddedInDbYet => text switch
                {
                    MenuItemsConstants.Start => UserCommandEnum.StartAndRegisterUserInDb,
                    null => UserCommandEnum.BeginChat,
                    _ => UserCommandEnum.BeginChat
                },
                UserStateEnum.MenuTabNoPersonalAccountAdded => text switch
                {
                    MenuItemsConstants.PersonalAccounts => UserCommandEnum.SelectPersonalAccountsTabNoPersonalAccountAdded,
                    _ => UserCommandEnum.Unknown
                },
                UserStateEnum.PersonalAccountsTabNoPersonalAccountAdded => text switch
                {
                    MenuItemsConstants.AddPersonalAccount => UserCommandEnum.SelectAddPersonalAccountTabNoPersonalAccountAdded,
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTabNoPersonalAccountAdded,
                    _ => UserCommandEnum.Unknown
                },
                UserStateEnum.AddingPersonalAccountNoPersonalAccountAdded => text switch
                {
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTabNoPersonalAccountAdded,
                    _ => Regex.IsMatch(text, UserInputPatterns.PersonalAccountPattern) ?
                        UserCommandEnum.ConfirmPersonalAccountTabNoPersonalAccountAdded :
                        UserCommandEnum.InvalidPersonalAccountFormatNoPersonalAccountAdded
                },
                UserStateEnum.MenuTab => text switch
                {
                    MenuItemsConstants.Readings => UserCommandEnum.SelectReadindsTab,
                    MenuItemsConstants.Payments => UserCommandEnum.SelectPaymentsTab,
                    MenuItemsConstants.PersonalAccounts => UserCommandEnum.SelectPersonalAccountsTab,
                    _ => UserCommandEnum.Unknown
                },
                UserStateEnum.PaymentsTab => text switch
                {
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTab,
                    _ => accountDto.PersonalAccounts.Any(a => Regex.IsMatch(text, string.Format(UserInputPatterns.PayPattern, a.PersonalAccountNumber))) ?
                         UserCommandEnum.EnterPaymentAmountTab :
                         UserCommandEnum.Unknown
                },
                UserStateEnum.ReadingsTab => text switch
                {
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTab,
                    _ => accountDto.PersonalAccounts.Any(a => Regex.IsMatch(text, string.Format(UserInputPatterns.SendReadingsPattern, a.PersonalAccountNumber))) ?
                        UserCommandEnum.SelectSendReadingsTab :
                        UserCommandEnum.Unknown
                },
                UserStateEnum.PersonalAccountsTab => text switch
                {
                    MenuItemsConstants.AddPersonalAccount => UserCommandEnum.SelectAddPersonalAccountTab,
                    MenuItemsConstants.DeletePersonalAccount => UserCommandEnum.SelectDeletePersonalAccountTab,
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTab,
                    _ => UserCommandEnum.Unknown
                },
                UserStateEnum.DeletingPersonalAccountTab => text switch
                {
                    MenuItemsConstants.PersonalAccounts => UserCommandEnum.SelectPersonalAccountsTab,
                    _ => accountDto.PersonalAccounts.Any(a => Regex.IsMatch(text, string.Format(UserInputPatterns.DeleteSpecificPersonalAccountPattern, a.PersonalAccountNumber))) ?
                        UserCommandEnum.DeletePersonalAccountTab :
                        UserCommandEnum.Unknown
                },
                UserStateEnum.AddingPersonalAccountTab => text switch
                {
                    MenuItemsConstants.PersonalAccounts => UserCommandEnum.SelectPersonalAccountsTab,
                    _ => Regex.IsMatch(text, UserInputPatterns.PersonalAccountPattern) ?
                        UserCommandEnum.ConfirmPersonalAccountTab :
                        UserCommandEnum.InvalidPersonalAccountFormat
                },
                UserStateEnum.SendReadingsTab => text switch
                {
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTab,
                    _ => Regex.IsMatch(text, UserInputPatterns.MeterReadingPattern) ?
                        UserCommandEnum.SendReadingsTab :
                        UserCommandEnum.InvalidReadingsFormatTab
                },
                UserStateEnum.СonfirmAddingPersonalAccountTab => text switch
                {
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTab,
                    _ => Regex.IsMatch(text, UserInputPatterns.MeterNumberPattern) ?
                        UserCommandEnum.SavePersonalAccount :
                        UserCommandEnum.InvalidMeterNumberTab
                },
                UserStateEnum.СonfirmAddingPersonalAccountNoPersonalAccountAdded => text switch
                {
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTabNoPersonalAccountAdded,
                    _ => Regex.IsMatch(text, UserInputPatterns.MeterNumberPattern) ?
                        UserCommandEnum.SavePersonalAccountNoPersonalAccountAdded :
                        UserCommandEnum.InvalidMeterNumberTabNoPersonalAccountAdded
                },
                UserStateEnum.EneteringPaymentAmountTab => text switch
                {
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTab,
                    _ => Regex.IsMatch(text, UserInputPatterns.PaymentAmountPattern) ?
                        UserCommandEnum.CompletePaymentTab :
                        UserCommandEnum.InvalidPaymentAmountFormatTab
                },
                UserStateEnum.CompletePaymentTab => text switch
                {
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTab,
                    _ => UserCommandEnum.Unknown
                },
                _ => UserCommandEnum.Unknown
            };

            return new GetCommandDto()
            {
                AccountDto = accountDto,
                CommandEnum = command,
                Message = text
            };
        }
    }
}
