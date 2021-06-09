using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace trainline.rates.common.Models.ResponseModels
{
    [ExcludeFromCodeCoverage]
    public class GetExchangeRatesResponseModel
    {
        
        public string Base {get; set;}
        public DateTime Date { get; set; }
        public long time_last_updated { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
