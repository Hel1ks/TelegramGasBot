using MediatR;

namespace GasApi.Dtos 
{
    public class GetPaymentsRequest : RequestBase, IRequest<GetPaymentsResponse>
    {
    }
}
