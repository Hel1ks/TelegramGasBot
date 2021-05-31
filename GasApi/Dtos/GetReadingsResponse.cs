using System.Collections.Generic;

namespace GasApi.Dtos
{
    public class GetReadingsResponse : ResponseBase
    {
        public IEnumerable<ReadingDto> Readings { get; set; }
    }
}
