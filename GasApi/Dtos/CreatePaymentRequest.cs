using MediatR;

namespace GasApi.Dtos
{
    public class CreatePaymentRequest : RequestBase, IRequest<CreatePaymentResponse>
    {
        public PaymentDto Payment { get; set; }
    }
}
