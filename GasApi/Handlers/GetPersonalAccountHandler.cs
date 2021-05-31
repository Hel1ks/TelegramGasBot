using AutoMapper;
using GasApi.Data;
using GasApi.Data.Entities;
using GasApi.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GasApi.Handlers
{
    public class GetPersonalAccountHandler : IRequestHandler<GetPersonalAccountRequest, GetPersonalAccountResponse>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetPersonalAccountHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<GetPersonalAccountResponse> Handle(GetPersonalAccountRequest request, CancellationToken cancellationToken)
        {
            UserDataEntity entity;

            try
            {
                entity = await this.repository.GetByPersonalAccountNumber(request.PersonalAccountNumber);
            }
            catch
            {
                return new GetPersonalAccountResponse()
                {
                    ResponseCode = ResponseCodeEnum.Error.ToString()
                };
            }

            if (entity == null)
            {
                return new GetPersonalAccountResponse()
                {
                    ResponseCode = ResponseCodeEnum.NotFound.ToString()
                };
            }

            return this.mapper.Map<GetPersonalAccountResponse>(entity);
        }
    }
}
