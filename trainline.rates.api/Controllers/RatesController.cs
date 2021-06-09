using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using trainline.rates.common.Models.RequestModels;
using trainline.rates.common.Models.ResponseModels;
using trainline.rates.services.Interfaces;

namespace trainline.rates.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RatesController : ControllerBase
    {
        private readonly ILogger<RatesController> _logger;
        private readonly ICurrencyConverterService _currencyService;

        public RatesController(ILogger<RatesController> logger, ICurrencyConverterService currencyService)
        {
            _logger = logger;
            _currencyService = currencyService;
        }

        /// <summary>
        /// Calculate the given price to the desired currency
        /// </summary>
        /// <param name="requestModel">The incoming JSON request deserialized</param>
        /// <returns>The calculated price to the target currency and the target currency selected</returns>
        [HttpPost]
        [Route("CalculateExchange")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Get([FromBody] CalculatePriceRequestModel requestModel)
        {
            try
            {
                var result = await _currencyService.CalculatePrice(requestModel.Price, requestModel.SourceCurrency, requestModel.TargetCurrency);

                if(result < 0)
                {
                    _logger.LogError($"An error occured when calling the CalculateExchange request");
                    return NotFound();
                }

                var responseModel = new CalculatePriceResponseModel { Price = result, CurrencyType = requestModel.TargetCurrency.ToString() };

                return Ok(responseModel);
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occured when calling the CalculateExchange request : {ex.StackTrace}");
                return NotFound();
            }            
        }
    }
}
