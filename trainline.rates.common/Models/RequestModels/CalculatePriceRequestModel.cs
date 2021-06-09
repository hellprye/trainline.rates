using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace trainline.rates.common.Models.RequestModels
{
    [ExcludeFromCodeCoverage]
    public class CalculatePriceRequestModel
    {
        public decimal Price { get; set; }
        public Enums.RateTypes SourceCurrency { get; set; }
        public Enums.RateTypes TargetCurrency { get; set; }
    }
}
