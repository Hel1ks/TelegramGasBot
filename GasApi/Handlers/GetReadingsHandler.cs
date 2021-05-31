using AutoMapper;
using GasApi.Data;
using GasApi.Data.Entities;
using GasApi.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GasApi.Handlers
{
    public class GetReadingsHandler : IRequestHandler<GetReadingsRequest, GetReadingsResponse>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetReadingsHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<GetReadingsResponse> Handle(GetReadingsRequest request, CancellationToken cancellationToken)
        {
            UserDataEntity entity;

            try
            {
                entity = await this.repository.GetByPersonalAccountNumber(request.PersonalAccountNumber);
            }
            catch
            {
                return new GetReadingsResponse()
                {
                    ResponseCode = ResponseCodeEnum.Error.ToString()
                };
            }

            if (entity == null)
            {
                return new GetReadingsResponse()
                {
                    ResponseCode = ResponseCodeEnum.NotFound.ToString()
                };
            }

            return this.mapper.Map<GetReadingsResponse>(entity);
        }
    }
}
