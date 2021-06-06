namespace GasApi.Dtos
{
    public class GetPersonalAccountResponse : ResponseBase
    {
        public string PersonalAccountNumber { get; set; }

        public string Address { get; set; }

        public string MeterNumber { get; set; }

        public decimal Balance { get; set; }
    }
}
