using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace trainline.rates.common
{
    public class CurrencyCalculator : ICurrencyCalculator
    {
        /// <summary>
        /// Calculates the new price based on the original price and exchange rate of the desired currency.
        /// </summary>
        /// <param name="originalPrice">The original price to exchange</param>
        /// <param name="exchangeRate">The exchange rate of the desired currency</param>
        /// <returns>The new calculated price</returns>
        public decimal CalculatePrice(decimal originalPrice, decimal exchangeRate)
        {
            if (originalPrice <= 0 || exchangeRate <= 0)
                throw new ArgumentException($"Invalid argument in CalculatePrice: originalPrice = {originalPrice} exchangeRate = {exchangeRate}");

            return originalPrice * exchangeRate;
        }
    }
}
