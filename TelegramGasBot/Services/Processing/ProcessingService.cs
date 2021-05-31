using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TelegramGasBot.Constants;
using TelegramGasBot.Enums;
using TelegramGasBot.Services.Account;
using TelegramGasBot.Services.Account.Models;
using TelegramGasBot.Services.GasApi;
using TelegramGasBot.Services.GasApi.Dtos;

namespace TelegramGasBot.Services.Processing
{
    public class ProcessingService : IProcessingService
    {
        private readonly IAccountService accountService;
        private readonly IGasApiService gasApiService;

        public ProcessingService(IAccountService accountService, IGasApiService gasApiService)
        {
            this.accountService = accountService;
            this.gasApiService = gasApiService;
        }

        public async Task<ProcessCommandResponseDto> ProcessCommandAsync(UserCommandEnum commandEnum, AccountModel accountDto, string message)
        {
            var methodName = commandEnum.ToString();
            var method = typeof(ProcessingService).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            var responseMessage = await (Task<string>)method.Invoke(this, new object[] { accountDto, message });

            var menuItems = this.GetMenuItems(accountDto);

            return new ProcessCommandResponseDto()
            {
                Message = responseMessage,
                MenuItems = menuItems
            };
        }

        private IEnumerable<string> GetMenuItems(AccountModel accountDto) =>
            accountDto?.State?.StateEnum switch
            {
                UserStateEnum.NotAddedInDbYet => new[] { MenuItemsConstants.Start },
                UserStateEnum.MenuTabNoPersonalAccountAdded => new[] { MenuItemsConstants.PersonalAccounts },
                UserStateEnum.PersonalAccountsTabNoPersonalAccountAdded => new[] { MenuItemsConstants.AddPersonalAccount, MenuItemsConstants.MainMenu },
                UserStateEnum.AddingPersonalAccountNoPersonalAccountAdded => new[] { MenuItemsConstants.MainMenu },
                UserStateEnum.MenuTab => new[] { MenuItemsConstants.Readings, MenuItemsConstants.Payments, MenuItemsConstants.PersonalAccounts },
                UserStateEnum.ReadingsTab => accountDto.PersonalAccounts.Select(a => string.Format(MenuItemsConstants.SendReadings, a.PersonalAccountNumber)).Append(MenuItemsConstants.MainMenu),
                UserStateEnum.PaymentsTab => new[] { MenuItemsConstants.MainMenu },
                UserStateEnum.PersonalAccountsTab => new[] { MenuItemsConstants.AddPersonalAccount, MenuItemsConstants.DeletePersonalAccount, MenuItemsConstants.MainMenu },
                UserStateEnum.AddingPersonalAccountTab => new[] { MenuItemsConstants.PersonalAccounts },
                UserStateEnum.DeletingPersonalAccountTab => accountDto.PersonalAccounts.Select(a => string.Format(MenuItemsConstants.DeleteSpecificPersonalAccount, a.PersonalAccountNumber)).Append(MenuItemsConstants.PersonalAccounts),
                UserStateEnum.SendReadingsTab => new[] { MenuItemsConstants.MainMenu },
                _ => new[] { MenuItemsConstants.Start }
            };

        #region Command Processing Methods

        private async Task<string> BeginChat(AccountModel accountDto, string message)
        {
            return CommandResponseConstants.BeginChat.DefaultResponse;
        }

        private async Task<string> StartAndRegisterUserInDb(AccountModel accountDto, string message)
        {
            accountDto.State = new StateModel()
            {
                StateEnum = UserStateEnum.MenuTabNoPersonalAccountAdded
            };

            await this.accountService.CreateAsync(accountDto);

            return CommandResponseConstants.StartAndRegisterUserInDb.DefaultResponse;
        }

        private async Task<string> SelectPersonalAccountsTabNoPersonalAccountAdded(AccountModel accountDto, string message)
        {
            accountDto.State.StateEnum = UserStateEnum.PersonalAccountsTabNoPersonalAccountAdded;

            await this.accountService.UpdateAsync(accountDto);

            return CommandResponseConstants.SelectPersonalAccountsTabNoPersonalAccountAdded.DefaultResponse;
        }

        private async Task<string> ReturnToMenuTabNoPersonalAccountAdded(AccountModel accountDto, string message)
        {
            accountDto.State.StateEnum = UserStateEnum.MenuTabNoPersonalAccountAdded;

            await this.accountService.UpdateAsync(accountDto);

            return CommandResponseConstants.ReturnToMenuTabNoPersonalAccountAdded.DefaultResponse;
        }

        private async Task<string> SelectAddPersonalAccountTabNoPersonalAccountAdded(AccountModel accountDto, string message)
        {
            accountDto.State.StateEnum = UserStateEnum.AddingPersonalAccountNoPersonalAccountAdded;

            await this.accountService.UpdateAsync(accountDto);

            return CommandResponseConstants.SelectAddPersonalAccountTabNoPersonalAccountAdded.DefaultResponse;
        }

        private async Task<string> AddPersonalAccountTabNoPersonalAccountAdded(AccountModel accountDto, string message)
        {
            var apiResponse = await gasApiService.GetPersonalAccountAsync(
               new GetPersonalAccountRequestDto()
               {
                   PersonalAccountNumber = message,
               });

            if (apiResponse == null || apiResponse.ResponseCode == ResponseCodeEnum.Error)
            {
                accountDto.State.StateEnum = UserStateEnum.PersonalAccountsTabNoPersonalAccountAdded;

                await this.accountService.UpdateAsync(accountDto);

                return CommandResponseConstants.AddPersonalAccountTabNoPersonalAccountAdded.ErrorResponse;
            }

            if (apiResponse.ResponseCode == ResponseCodeEnum.NotFound)
            {
                accountDto.State.StateEnum = UserStateEnum.PersonalAccountsTabNoPersonalAccountAdded;

                await this.accountService.UpdateAsync(accountDto);

                return string.Format(CommandResponseConstants.AddPersonalAccountTabNoPersonalAccountAdded.PersonalAccountNotFoundResponse, message);
            }

            accountDto.State.StateEnum = UserStateEnum.MenuTab;
            accountDto.PersonalAccounts = new[]
            {
                new PersonalAccountModel()
                {
                    PersonalAccountNumber = message,
                    Address = apiResponse.Address
                }
            };

            await this.accountService.UpdateAsync(accountDto);

            return string.Format(CommandResponseConstants.AddPersonalAccountTabNoPersonalAccountAdded.SuccessResponse, message);
        }

        private async Task<string> InvalidPersonalAccountFormatNoPersonalAccountAdded(AccountModel accountDto, string message)
        {
            accountDto.State.StateEnum = UserStateEnum.PersonalAccountsTabNoPersonalAccountAdded;

            await this.accountService.UpdateAsync(accountDto);

            return CommandResponseConstants.InvalidPersonalAccountFormatNoPersonalAccountAdded.DefaultResponse;
        }

        private async Task<string> SelectReadindsTab(AccountModel accountDto, string message)
        {
            accountDto.State.StateEnum = UserStateEnum.ReadingsTab;

            await this.accountService.UpdateAsync(accountDto);

            var telegramMessage = new StringBuilder(CommandResponseConstants.SelectReadindsTab.Readings + Environment.NewLine);

            foreach (var item in accountDto.PersonalAccounts)
            {
                telegramMessage.Append(Environment.NewLine + item.PersonalAccountNumber + Environment.NewLine + $"{item.Address} :" + Environment.NewLine);

                var apiResponse = await gasApiService.GetReadingsAsync(
                    new GetReadingsRequestDto()
                    {
                        PersonalAccountNumber = item.PersonalAccountNumber,
                    });

                if (apiResponse == null || apiResponse.ResponseCode != ResponseCodeEnum.Success)
                {
                    telegramMessage.Append(CommandResponseConstants.SelectReadindsTab.Error + Environment.NewLine);
                }
                else
                {
                    var timeZome = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");

                    foreach (var reading in apiResponse.Readings)
                    {
                        var date = TimeZoneInfo.ConvertTime(reading.DateTime, timeZome).ToString("MM.dd.yyyy HH:mm");

                        telegramMessage.Append(string.Format(CommandResponseConstants.SelectReadindsTab.ReadingItem, date, reading.Value) + Environment.NewLine);
                    }
                }
            }

            return telegramMessage.ToString();
        }

        private async Task<string> SelectPaymentsTab(AccountModel accountDto, string message)
        {
            accountDto.State.StateEnum = UserStateEnum.PaymentsTab;

            await this.accountService.UpdateAsync(accountDto);

            var telegramMessage = new StringBuilder(CommandResponseConstants.SelectPaymentsTab.Payments + Environment.NewLine);

            foreach (var item in accountDto.PersonalAccounts)
            {
                telegramMessage.Append(Environment.NewLine + item.PersonalAccountNumber + Environment.NewLine + $"{item.Address} :" + Environment.NewLine);

                var apiResponse = await gasApiService.GetPaymentsAsync(
                    new GetPaymentsRequestDto()
                    {
                        PersonalAccountNumber = item.PersonalAccountNumber,
                    });

                if (apiResponse == null || apiResponse.ResponseCode != ResponseCodeEnum.Success)
                {
                    telegramMessage.Append(CommandResponseConstants.SelectPaymentsTab.Error + Environment.NewLine);
                }
                else
                {
                    var timeZome = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");

                    foreach (var payment in apiResponse.Payments)
                    {
                        var date = TimeZoneInfo.ConvertTime(payment.DateTime, timeZome).ToString("MM.dd.yyyy HH:mm");

                        telegramMessage.Append(string.Format(CommandResponseConstants.SelectPaymentsTab.PaymentItem, date, payment.Amount) + Environment.NewLine);
                    }
                }
            }

            return telegramMessage.ToString();
        }

        private async Task<string> SelectPersonalAccountsTab(AccountModel accountDto, string message)
        {
            accountDto.State.StateEnum = UserStateEnum.PersonalAccountsTab;

            await this.accountService.UpdateAsync(accountDto);

            var telegramMessage = new StringBuilder(CommandResponseConstants.SelectPersonalAccountsTab.PersonalAccounts + Environment.NewLine);
            telegramMessage.AppendJoin(Environment.NewLine, accountDto.PersonalAccounts.Select(a => a.PersonalAccountNumber));

            return telegramMessage.ToString();
        }

        private async Task<string> ReturnToMenuTab(AccountModel accountDto, string message)
        {
            accountDto.State.StateEnum = UserStateEnum.MenuTab;

            await this.accountService.UpdateAsync(accountDto);

            return CommandResponseConstants.ReturnToMenuTab.DefaultResponse;
        }

        private async Task<string> SelectAddPersonalAccountTab(AccountModel accountDto, string message)
        {
            accountDto.State.StateEnum = UserStateEnum.AddingPersonalAccountTab;

            await this.accountService.UpdateAsync(accountDto);

            return CommandResponseConstants.SelectAddPersonalAccountTab.DefaultResponse;
        }

        private async Task<string> SelectDeletePersonalAccountTab(AccountModel accountDto, string message)
        {
            accountDto.State.StateEnum = UserStateEnum.DeletingPersonalAccountTab;

            await this.accountService.UpdateAsync(accountDto);

            return CommandResponseConstants.SelectDeletePersonalAccountTab.DefaultResponse;
        }

        private async Task<string> AddPersonalAccountTab(AccountModel accountDto, string message)
        {
            if (accountDto.PersonalAccounts.Any(a => a.PersonalAccountNumber == message))
            {
                return string.Format(CommandResponseConstants.AddPersonalAccountTab.PersonalAccountDuplicationResponse, message);
            }

            var apiResponse = await gasApiService.GetPersonalAccountAsync(
                   new GetPersonalAccountRequestDto()
                   {
                       PersonalAccountNumber = message,
                   });

            if (apiResponse == null || apiResponse.ResponseCode == ResponseCodeEnum.Error)
            {
                accountDto.State.StateEnum = UserStateEnum.PersonalAccountsTab;

                await this.accountService.UpdateAsync(accountDto);

                return CommandResponseConstants.AddPersonalAccountTab.ErrorResponse;
            }

            if (apiResponse.ResponseCode == ResponseCodeEnum.NotFound)
            {
                accountDto.State.StateEnum = UserStateEnum.PersonalAccountsTab;

                await this.accountService.UpdateAsync(accountDto);

                return string.Format(CommandResponseConstants.AddPersonalAccountTab.PersonalAccountNotFoundResponse, message);
            }

            accountDto.State.StateEnum = UserStateEnum.MenuTab;

            var newPersonalAccount = new PersonalAccountModel()
            {
                PersonalAccountNumber = message,
                Address = apiResponse.Address
            };

            accountDto.PersonalAccounts = accountDto.PersonalAccounts.Append(newPersonalAccount);

            await this.accountService.UpdateAsync(accountDto);

            return string.Format(CommandResponseConstants.AddPersonalAccountTab.SuccessResponse, message);
        }

        private async Task<string> InvalidPersonalAccountFormat(AccountModel accountDto, string message)
        {
            accountDto.State.StateEnum = UserStateEnum.PersonalAccountsTab;

            await this.accountService.UpdateAsync(accountDto);

            return CommandResponseConstants.InvalidPersonalAccountFormat.DefaultResponse;
        }

        private async Task<string> DeletePersonalAccountTab(AccountModel accountDto, string message)
        {
            if (accountDto.PersonalAccounts.Count() == 1)
            {
                accountDto.State.StateEnum = UserStateEnum.MenuTabNoPersonalAccountAdded;
                accountDto.PersonalAccounts = null;

                await this.accountService.UpdateAsync(accountDto);

                return string.Format(CommandResponseConstants.DeletePersonalAccountTab.OneAccountResponse, message);
            }
            else
            {
                var personalAccountNumber = message[9..];

                accountDto.State.StateEnum = UserStateEnum.PersonalAccountsTab;
                accountDto.PersonalAccounts = accountDto.PersonalAccounts.Where(a => a.PersonalAccountNumber != personalAccountNumber);

                await this.accountService.UpdateAsync(accountDto);

                var telegramMessage = new StringBuilder(string.Format(CommandResponseConstants.DeletePersonalAccountTab.ManyAccountResponse, message));
                telegramMessage.Append(Environment.NewLine + CommandResponseConstants.DeletePersonalAccountTab.PersonalAccounts + Environment.NewLine);
                telegramMessage.AppendJoin(Environment.NewLine, accountDto.PersonalAccounts.Select(a => a.PersonalAccountNumber));

                return telegramMessage.ToString();
            }
        }

        private async Task<string> SelectSendReadingsTab(AccountModel accountDto, string message)
        {
            var personalAccountNumberToSendReadings = message[14..];

            accountDto.State.StateEnum = UserStateEnum.SendReadingsTab;
            accountDto.State.StateItemValue = personalAccountNumberToSendReadings;

            await this.accountService.UpdateAsync(accountDto);

            return string.Format(CommandResponseConstants.SelectSendReadingsTab.DefaultResponse, personalAccountNumberToSendReadings);
        }

        private async Task<string> SendReadingsTab(AccountModel accountDto, string message)
        {
            accountDto.State.StateEnum = UserStateEnum.MenuTab;
            var personalAccountNumberToSendReadings = accountDto.State.StateItemValue;

            var value = decimal.Parse(message, CultureInfo.InvariantCulture);

            var apiResponse = await gasApiService.CreateReadingAsync(
                new CreateReadingRequestDto()
                {
                    PersonalAccountNumber = personalAccountNumberToSendReadings,
                    Reading = new ReadingDto()
                    {
                        Value = value,
                        DateTime = DateTime.Now
                    }
                });

            await this.accountService.UpdateAsync(accountDto);

            if (apiResponse == null || apiResponse.ResponseCode != ResponseCodeEnum.Success)
            {
                return CommandResponseConstants.SendReadingsTab.ErrorResponse;
            }

            return string.Format(CommandResponseConstants.SendReadingsTab.DefaultResponse, message, personalAccountNumberToSendReadings);
        }

        private async Task<string> InvalidReadingsFormatTab(AccountModel accountDto, string message)
        {
            accountDto.State.StateEnum = UserStateEnum.ReadingsTab;

            await this.accountService.UpdateAsync(accountDto);

            return CommandResponseConstants.InvalidReadingsFormatTab.DefaultResponse;
        }

        private async Task<string> Unknown(AccountModel accountDto, string message)
        {
            return CommandResponseConstants.Unknown.DefaultResponse;
        }

        #endregion
    }
}