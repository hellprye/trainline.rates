using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using trainline.rates.common;
using trainline.rates.common.handlers;
using trainline.rates.common.Models.ResponseModels;
using trainline.rates.services.Interfaces;
using static trainline.rates.common.Enums;

namespace trainline.rates.services
{
    /// <summary>
    /// Exchange calculation service
    /// </summary>
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private readonly ILogger<CurrencyConverterService> _logger;
        private readonly ICurrencyCalculator _calculator;
        private readonly IHttpHandler _handler;
        private readonly IConfiguration _config;

        public CurrencyConverterService(ILogger<CurrencyConverterService> logger, ICurrencyCalculator calculator, IHttpHandler handler, IConfiguration config)
        {
            _logger = logger;
            _calculator = calculator;
            _handler = handler;
            _config = config;
        }

        /// <summary>
        /// Calculate the price for the desired currency
        /// </summary>
        /// <param name="originalPrice">Original price to exchange</param>
        /// <param name="sourceCurrency">Original currency type</param>
        /// <param name="targetRateType">Desired currency Type</param>
        /// <returns>The new calculated price</returns>
        public async Task<decimal> CalculatePrice(decimal originalPrice, RateTypes sourceCurrency, RateTypes targetRateType)
        {
            if (originalPrice <= 0)
                throw new ArgumentException($"Original price argument is not a valid value : {originalPrice}");

            try
            {
                // Initialize headers
                var headers = new System.Collections.Generic.Dictionary<string, string>();
                headers.Add("Accept", "application/json");
                _handler.SetHeaders(headers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to set http headers. {ex.Message}");
                return -1;
            }

            var uri = "";

            try
            {
                // Create Uri
                uri = _config.GetSection("BaseUrl").Value + sourceCurrency.ToString() + ".json";
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occured while trying to construct the uri. {ex.StackTrace}");
                return -1;
            }

            // Get async to exchange rates api
            var getExchangeRateResponse = await _handler.GetAsync(uri);

            if (!getExchangeRateResponse.IsSuccessStatusCode)
            {
                _logger.LogError($"An error occured when calling currency converter service. Error Code: {getExchangeRateResponse.StatusCode}");
                return -1;
            }

            try
            {
                // convert json response to response model
                var exchangeRates = JsonConvert.DeserializeObject<GetExchangeRatesResponseModel>(await getExchangeRateResponse.Content.ReadAsStringAsync());

                var targetExchangeRate = exchangeRates.Rates[targetRateType.ToString()];

                // Calculate the new price
                // TODO: I know the calculated value is not to 2 decimal places and that is primarily because I do not know what the business criteria for rounding should be and this may even be different depending on which department/client is using this functionality.
                return _calculator.CalculatePrice(originalPrice, targetExchangeRate);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while calling CalculatePrice. {ex.StackTrace}");
                return -1;
            }            
        }
    }
}
