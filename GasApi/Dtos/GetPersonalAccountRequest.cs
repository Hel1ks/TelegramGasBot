using MediatR;

namespace GasApi.Dtos
{
    public class GetPersonalAccountRequest : RequestBase, IRequest<GetPersonalAccountResponse>
    {
    }
}
