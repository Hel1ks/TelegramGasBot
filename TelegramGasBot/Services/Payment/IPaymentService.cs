using System.Threading.Tasks;

namespace TelegramGasBot.Services.Payment
{
    public interface IPaymentService
    {
        Task<PaymentModel> GetPaymentByIdAsync(string id);

        Task<string> CreateAsync(PaymentModel payment);

        Task UpdateAsync(PaymentModel payment);
    }
}
