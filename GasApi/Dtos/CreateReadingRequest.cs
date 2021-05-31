using MediatR;

namespace GasApi.Dtos
{
    public class CreateReadingRequest : RequestBase, IRequest<CreateReadingResponse>
    {
        public ReadingDto Reading { get; set; }
    }
}
