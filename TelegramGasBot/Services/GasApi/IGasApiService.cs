using System.Threading.Tasks;
using TelegramGasBot.Services.GasApi.Dtos;

namespace TelegramGasBot.Services.GasApi
{
    public interface IGasApiService
    {
        Task<CreateReadingResponseDto> CreateReadingAsync(CreateReadingRequestDto requestDto);

        Task<GetPaymentsResponseDto> GetPaymentsAsync(GetPaymentsRequestDto requestDto);

        Task<GetPersonalAccountResponseDto> GetPersonalAccountAsync(GetPersonalAccountRequestDto requestDto);

        Task<GetReadingsResponseDto> GetReadingsAsync(GetReadingsRequestDto requestDto);
    }
}
