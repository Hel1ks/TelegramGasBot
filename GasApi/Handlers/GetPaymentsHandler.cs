using AutoMapper;
using GasApi.Data;
using GasApi.Data.Entities;
using GasApi.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GasApi.Handlers
{
    public class GetPaymentsHandler : IRequestHandler<GetPaymentsRequest, GetPaymentsResponse>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetPaymentsHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<GetPaymentsResponse> Handle(GetPaymentsRequest request, CancellationToken cancellationToken)
        {
            UserDataEntity entity;

            try
            {
                entity = await this.repository.GetByPersonalAccountNumber(request.PersonalAccountNumber);
            }
            catch 
            {
                return new GetPaymentsResponse()
                {
                    ResponseCode = ResponseCodeEnum.Error.ToString()
                };
            }

            if (entity == null)
            {
                return new GetPaymentsResponse()
                {
                    ResponseCode = ResponseCodeEnum.NotFound.ToString()
                };
            }

            return this.mapper.Map<GetPaymentsResponse>(entity);
        }
    }
}
