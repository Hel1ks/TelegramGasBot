using System.Collections.Generic;

namespace GasApi.Dtos
{
    public class GetPaymentsResponse : ResponseBase
    {
        public IEnumerable<PaymentDto> Payments { get; set; }
    }
}
