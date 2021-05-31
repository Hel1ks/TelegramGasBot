using MediatR;

namespace GasApi.Dtos
{
    public class GetReadingsRequest : RequestBase, IRequest<GetReadingsResponse>
    {
    }
}
