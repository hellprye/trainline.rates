using System.Diagnostics.CodeAnalysis;

namespace trainline.rates.common.Models.ResponseModels
{
    [ExcludeFromCodeCoverage]
    public class CalculatePriceResponseModel
    {
        public decimal Price { get; set; }
        public string CurrencyType { get; set; }
    }
}
