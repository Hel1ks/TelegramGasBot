using System.Linq;
using System.Text.RegularExpressions;
using TelegramGasBot.Constants;
using TelegramGasBot.Enums;
using TelegramGasBot.Services.Account.Models;

namespace TelegramGasBot.Services.Command
{
    public class CommandService : ICommandService
    {
        public UserCommandEnum GetCommand(string message, AccountModel account)
        {
            var userState = account?.State?.StateEnum ?? UserStateEnum.NotAddedInDbYet;

            return userState switch
            {
                UserStateEnum.NotAddedInDbYet => message switch
                {
                    MenuItemsConstants.Start => UserCommandEnum.StartAndRegisterUserInDb,
                    null => UserCommandEnum.BeginChat,
                    _ => UserCommandEnum.BeginChat
                },
                UserStateEnum.MenuTabNoPersonalAccountAdded => message switch
                {
                    MenuItemsConstants.PersonalAccounts => UserCommandEnum.SelectPersonalAccountsTabNoPersonalAccountAdded,
                    _ => UserCommandEnum.Unknown
                },
                UserStateEnum.PersonalAccountsTabNoPersonalAccountAdded => message switch
                {
                    MenuItemsConstants.AddPersonalAccount => UserCommandEnum.SelectAddPersonalAccountTabNoPersonalAccountAdded,
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTabNoPersonalAccountAdded,
                    _ => UserCommandEnum.Unknown
                },
                UserStateEnum.AddingPersonalAccountNoPersonalAccountAdded => message switch
                {
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTabNoPersonalAccountAdded,
                    _ => Regex.IsMatch(message, UserInputPatterns.PersonalAccountPattern) ?
                        UserCommandEnum.ConfirmPersonalAccountTabNoPersonalAccountAdded :
                        UserCommandEnum.InvalidPersonalAccountFormatNoPersonalAccountAdded
                },
                UserStateEnum.MenuTab => message switch
                {
                    MenuItemsConstants.Readings => UserCommandEnum.SelectReadindsTab,
                    MenuItemsConstants.Payments => UserCommandEnum.SelectPaymentsTab,
                    MenuItemsConstants.PersonalAccounts => UserCommandEnum.SelectPersonalAccountsTab,
                    _ => UserCommandEnum.Unknown
                },
                UserStateEnum.PaymentsTab => message switch
                {
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTab,
                    _ => UserCommandEnum.Unknown
                },
                UserStateEnum.ReadingsTab=> message switch
                {
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTab,
                    _ => account.PersonalAccounts.Any(a => Regex.IsMatch(message, string.Format(UserInputPatterns.SendReadingsPattern, a.PersonalAccountNumber))) ?
                        UserCommandEnum.SelectSendReadingsTab :
                        UserCommandEnum.Unknown
                },
                UserStateEnum.PersonalAccountsTab => message switch
                {
                    MenuItemsConstants.AddPersonalAccount => UserCommandEnum.SelectAddPersonalAccountTab,
                    MenuItemsConstants.DeletePersonalAccount => UserCommandEnum.SelectDeletePersonalAccountTab,
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTab,
                    _ => UserCommandEnum.Unknown
                },
                UserStateEnum.DeletingPersonalAccountTab => message switch
                {
                    MenuItemsConstants.PersonalAccounts => UserCommandEnum.SelectPersonalAccountsTab,
                    _ => account.PersonalAccounts.Any(a => Regex.IsMatch(message, string.Format(UserInputPatterns.DeleteSpecificPersonalAccountPattern, a.PersonalAccountNumber))) ? 
                        UserCommandEnum.DeletePersonalAccountTab : 
                        UserCommandEnum.Unknown
                },
                UserStateEnum.AddingPersonalAccountTab => message switch
                {
                    MenuItemsConstants.PersonalAccounts => UserCommandEnum.SelectPersonalAccountsTab,
                    _ => Regex.IsMatch(message, UserInputPatterns.PersonalAccountPattern) ? 
                        UserCommandEnum.ConfirmPersonalAccountTab : 
                        UserCommandEnum.InvalidPersonalAccountFormat
                },
                UserStateEnum.SendReadingsTab => message switch
                {
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTab,
                    _ => Regex.IsMatch(message, UserInputPatterns.MeterReadingPattern) ?
                        UserCommandEnum.SendReadingsTab:
                        UserCommandEnum.InvalidReadingsFormatTab
                },
                UserStateEnum.СonfirmAddingPersonalAccountTab => message switch
                {
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTab,
                    _ => Regex.IsMatch(message, UserInputPatterns.MeterNumberPattern) ?
                        UserCommandEnum.SavePersonalAccount :
                        UserCommandEnum.InvalidMeterNumberTab
                },
                UserStateEnum.СonfirmAddingPersonalAccountNoPersonalAccountAdded => message switch
                {
                    MenuItemsConstants.MainMenu => UserCommandEnum.ReturnToMenuTabNoPersonalAccountAdded,
                    _ => Regex.IsMatch(message, UserInputPatterns.MeterNumberPattern) ?
                        UserCommandEnum.SavePersonalAccountNoPersonalAccountAdded :
                        UserCommandEnum.InvalidMeterNumberTabNoPersonalAccountAdded
                },
                _ => UserCommandEnum.Unknown
            };
        }
    }
}
