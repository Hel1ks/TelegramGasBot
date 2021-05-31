using System.Collections.Generic;

namespace TelegramGasBot.Services.Processing
{
    public class ProcessCommandResponseDto
    {
        public string Message { get; set; }

        public IEnumerable<string> MenuItems { get; set; }
    }
}
