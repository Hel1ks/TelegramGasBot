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
    public class CreatePaymentHandler : IRequestHandler<CreatePaymentRequest, CreatePaymentResponse>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public CreatePaymentHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<CreatePaymentResponse> Handle(CreatePaymentRequest request, CancellationToken cancellationToken)
        {
            UserDataEntity entity;

            try
            {
                entity = await this.repository.GetByPersonalAccountNumber(request.PersonalAccountNumber);
            }
            catch
            {
                return new CreatePaymentResponse()
                {
                    ResponseCode = ResponseCodeEnum.Error.ToString()
                };
            }

            if (entity == null)
            {
                return new CreatePaymentResponse()
                {
                    ResponseCode = ResponseCodeEnum.NotFound.ToString()
                };
            }

            var newPaymentEntity = this.mapper.Map<PaymentEntity>(request.Payment);

            if (entity.Payments == null)
            {
                entity.Payments = new[] { newPaymentEntity };
            }
            else
            {
                entity.Payments = entity.Payments.Append(newPaymentEntity);
            }

            entity.Balance += request.Payment.Amount;

            try
            {
                await this.repository.Update(entity);
            }
            catch
            {
                return new CreatePaymentResponse()
                {
                    ResponseCode = ResponseCodeEnum.Error.ToString()
                };
            }

            return new CreatePaymentResponse()
            {
                ResponseCode = ResponseCodeEnum.Success.ToString()
            };
        }
    }
}
