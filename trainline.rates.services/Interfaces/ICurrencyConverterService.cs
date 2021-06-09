using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static trainline.rates.common.Enums;

namespace trainline.rates.services.Interfaces
{
    public interface ICurrencyConverterService
    {
        Task<decimal> CalculatePrice(decimal originalPrice, RateTypes sourceCurrency, RateTypes targetRateType);
    }
}
