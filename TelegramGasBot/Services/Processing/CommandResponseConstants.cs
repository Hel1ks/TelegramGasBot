namespace TelegramGasBot.Services.Processing
{
    public static class CommandResponseConstants
    {
        public static class BeginChat
        {
            public const string DefaultResponse = "Вітаємо! Для початку роботи з ботом натисніть розпочати";
        }

        public static class StartAndRegisterUserInDb
        {
            public const string DefaultResponse = "Для початку роботи з ботом додайте особовий рахунок. Перейдіть у Особові рахунки -> Додати";
        }

        public static class SelectPersonalAccountsTabNoPersonalAccountAdded
        {
            public const string DefaultResponse = "Для початку роботи з ботом додайте особовий рахунок. Оберіть додати особовий рахунок";
        }

        public static class ReturnToMenuTabNoPersonalAccountAdded
        {
            public const string DefaultResponse = "Для початку роботи з ботом додайте особовий рахунок. Перейдіть у Особові рахунки -> Додати";
        }

        public static class SelectAddPersonalAccountTabNoPersonalAccountAdded
        {
            public const string DefaultResponse = "Введіть особовий рахунок у форматі ХХХХХХХХХ, де Х - цифри";
        }

        public static class InvalidPersonalAccountFormatNoPersonalAccountAdded
        {
            public const string DefaultResponse = "Формат особового рахунку неправильний. Спробуйте ще раз";
        }

        public static class ReturnToMenuTab
        {
            public const string DefaultResponse = "Оберіть команду зі списку меню";
        }

        public static class SelectAddPersonalAccountTab
        {
            public const string DefaultResponse = "Введіть особовий рахунок у форматі ХХХХХХХХХ, де Х - цифри";
        }

        public static class SelectDeletePersonalAccountTab
        {
            public const string DefaultResponse = "Оберіть особовий рахунок, який бажаєте видалити";
        }

        public static class InvalidPersonalAccountFormat
        {
            public const string DefaultResponse = "Формат особового рахунку неправильний. Спробуйте ще раз";
        }

        public static class InvalidReadingsFormatTab
        {
            public const string DefaultResponse = "Формат показів лічильника неправильний. Спробуйте ще раз";
        }

        public static class Unknown
        {
            public const string DefaultResponse = "Невідома команда. Оберіть команду зі списку меню";
        }

        public static class SendReadingsTab
        {
            public const string DefaultResponse = "Показник {0} по особовому рахунку {1} успішно прийнято";

            public const string ErrorResponse = "Помилка обробки. Покази не передані, будь ласка спробуйте пізніше";
        }

        public static class SelectSendReadingsTab
        {
            public const string DefaultResponse = "Введіть показання лічильника особового рахунку {0} у форматі XXXX.XX";
        }

        public static class DeletePersonalAccountTab
        {
            public const string OneAccountResponse = "Особовий рахунок {0} видалено. Для продовження роботи слід додати особовий рахунок";
            public const string ManyAccountResponse = "Особовий рахунок {0} видалено";
            public const string PersonalAccounts = "Ваші особові рахунки:";            
        }

        public static class ConfirmPersonalAccountTab
        {
            public const string PersonalAccountDuplicationResponse = "Особовий рахунок {0} вже додано";
            public const string ErrorResponse = "Сталася помилка.Будь ласка спробуйте ще раз пізніше";
            public const string PersonalAccountNotFoundResponse = "Особовий рахунок {0} не знайдено";
            public const string SuccessResponse = "Підтвердіть свою приналежність до особового рахунку {0}, вказавши серійний номер лічильника у форматі ХХХХХХХХ, де Х - цифри";
        }

        public static class SelectPersonalAccountsTab
        {
            public const string PersonalAccounts = "Ваші особові рахунки:";
        }

        public static class ConfirmPersonalAccountTabNoPersonalAccountAdded
        {
            public const string ErrorResponse = "Сталася помилка.Будь ласка спробуйте ще раз пізніше";
            public const string PersonalAccountNotFoundResponse = "Особовий рахунок {0} не знайдено";
            public const string SuccessResponse = "Підтвердіть свою приналежність до особового рахунку {0}, вказавши серійний номер лічильника у форматі ХХХХХХХХ, де Х - цифри";
        }

        public static class SelectReadindsTab
        {
            public const string Readings = "Покази";
            public const string Error = "Не вдалося отримати покази";
            public const string ReadingItem = "{0} - {1}м3";
        }

        public static class SelectPaymentsTab
        {
            public const string Payments = "Платежі";
            public const string Error = "Не вдалося отримати платежі";
            public const string PaymentItem = "{0} - {1}грн";
        }

        public static class SavePersonalAccount
        {
            public const string ErrorResponse = "Сталася помилка. Спробуйте будь-ласка пізніше";
            public const string MeterNumberDoNotMatchResponse = "Неправильний номер лічильника. Особовий рахунок {0} не було додано";
            public const string SuccessResponse = "Особовий рахунок {0} за адресою {1} успішно додано";
        }

        public static class SavePersonalAccountNoPersonalAccountAdded
        {
            public const string ErrorResponse = "Сталася помилка. Спробуйте будь-ласка пізніше";
            public const string MeterNumberDoNotMatchResponse = "Неправильний номер лічильника. Особовий рахунок {0} не було додано";
            public const string SuccessResponse = "Особовий рахунок {0} за адресою {1} успішно додано";
        }

        public static class InvalidMeterNumberTabNoPersonalAccountAdded
        {
            public const string DefaultResponse = "Формат номера лічильника неправильний";
        }

        public static class InvalidMeterNumberTab
        {
            public const string DefaultResponse = "Формат номера лічильника неправильний";
        }
    }
}
