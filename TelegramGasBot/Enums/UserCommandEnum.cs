namespace TelegramGasBot.Enums
{
    public enum UserCommandEnum
    {
        Unknown,
        BeginChat,
        StartAndRegisterUserInDb,
        SelectPersonalAccountsTabNoPersonalAccountAdded,
        ReturnToMenuTabNoPersonalAccountAdded,
        SelectAddPersonalAccountTabNoPersonalAccountAdded,
        AddPersonalAccountTabNoPersonalAccountAdded,
        InvalidPersonalAccountFormatNoPersonalAccountAdded,
        SelectReadindsTab,
        SelectPaymentsTab,
        SelectPersonalAccountsTab,
        ReturnToMenuTab,
        SelectDeletePersonalAccountTab,
        SelectAddPersonalAccountTab,
        AddPersonalAccountTab,
        InvalidPersonalAccountFormat,
        DeletePersonalAccountTab,
        SelectSendReadingsTab,
        SendReadingsTab,
        InvalidReadingsFormatTab,
    }
}
