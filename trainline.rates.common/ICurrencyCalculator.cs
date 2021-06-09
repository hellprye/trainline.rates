using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace trainline.rates.common
{
    public interface ICurrencyCalculator
    {
        decimal CalculatePrice(decimal originalPrice, decimal exchangeRate);
    }
}
