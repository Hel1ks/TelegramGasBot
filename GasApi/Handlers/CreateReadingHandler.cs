using AutoMapper;
using GasApi.Data;
using GasApi.Data.Entities;
using GasApi.Dtos;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GasApi.Handlers
{
    public class CreateReadingHandler : IRequestHandler<CreateReadingRequest, CreateReadingResponse>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public CreateReadingHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<CreateReadingResponse> Handle(CreateReadingRequest request, CancellationToken cancellationToken)
        {
            UserDataEntity entity;

            try
            {
                entity = await this.repository.GetByPersonalAccountNumber(request.PersonalAccountNumber);
            }
            catch
            {
                return new CreateReadingResponse()
                {
                    ResponseCode = ResponseCodeEnum.Error.ToString()
                };
            }

            if (entity == null)
            {
                return new CreateReadingResponse()
                {
                    ResponseCode = ResponseCodeEnum.NotFound.ToString()
                };
            }

            var newReadingEntity = this.mapper.Map<ReadingEntity>(request.Reading);

            if (entity.Readings == null)
            {
                entity.Readings = new[] { newReadingEntity };
            }
            else
            {
                entity.Readings = entity.Readings.Append(newReadingEntity);
            }

            try
            {
                await this.repository.Update(entity);
            }
            catch
            {
                return new CreateReadingResponse()
                {
                    ResponseCode = ResponseCodeEnum.Error.ToString()
                };
            }

            return new CreateReadingResponse()
            {
                ResponseCode = ResponseCodeEnum.Success.ToString()
            };
        }
    }
}
