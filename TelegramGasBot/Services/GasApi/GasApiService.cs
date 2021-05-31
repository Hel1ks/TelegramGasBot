using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TelegramGasBot.Services.GasApi.Dtos;

namespace TelegramGasBot.Services.GasApi
{
    public class GasApiService : IGasApiService
    {
        private readonly HttpClient httpClient;

        public GasApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<CreateReadingResponseDto> CreateReadingAsync(CreateReadingRequestDto requestDto) =>
            await ApiCall<CreateReadingResponseDto, CreateReadingRequestDto>(requestDto, GasApiEndpointsEnum.CreateReading);

        public async Task<GetPaymentsResponseDto> GetPaymentsAsync(GetPaymentsRequestDto requestDto) =>
            await ApiCall<GetPaymentsResponseDto, GetPaymentsRequestDto>(requestDto, GasApiEndpointsEnum.GetPayments);

        public async Task<GetPersonalAccountResponseDto> GetPersonalAccountAsync(GetPersonalAccountRequestDto requestDto) =>
            await ApiCall<GetPersonalAccountResponseDto, GetPersonalAccountRequestDto>(requestDto, GasApiEndpointsEnum.GetPersonalAccount);

        public async Task<GetReadingsResponseDto> GetReadingsAsync(GetReadingsRequestDto requestDto) =>
            await ApiCall<GetReadingsResponseDto, GetReadingsRequestDto>(requestDto, GasApiEndpointsEnum.GetReadings);

        private async Task<TResponse> ApiCall<TResponse, TRequest>(TRequest requestDto, GasApiEndpointsEnum endpointEnum) where TResponse : class
        {
            var endpoint = endpointEnum.ToString();

            try
            {
                var response = await httpClient.PostAsJsonAsync(endpoint, requestDto);

                var dto = await response.Content.ReadFromJsonAsync<TResponse>();

                return dto;
            }
            catch
            {
                return null;
            }
        }
    }
}
