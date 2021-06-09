using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using trainline.rates.common;
using trainline.rates.common.handlers;
using trainline.rates.services;
using Xunit;

namespace trainline.rates.tests
{
    [ExcludeFromCodeCoverage]
    public class CurrencyConverterServiceTests
    {
        private Mock<IConfiguration> configMock;
        private Mock<IHttpHandler> httpHandlerMock;
        private Mock<ICurrencyCalculator> currencyCalculatorMock;
        private CurrencyConverterService service;

        private string responseJson = "{ \"base\": \"USD\", \"date\": \"2021-04-07\", \"time_last_updated\": 1617753602, \"rates\": { \"GBP\": 1, \"EUR\": 1.168852, \"USD\": 1.384935 } }";

        private void SetupConfig(string baseUrl, HttpResponseMessage message, decimal expectedCalcValue, bool missingConfig = false)
        {
            configMock = new Mock<IConfiguration>();
            httpHandlerMock = new Mock<IHttpHandler>();
            currencyCalculatorMock = new Mock<ICurrencyCalculator>();

            if(!missingConfig)
            {
                configMock.Setup(x =>
                    x.GetSection(It.Is<string>(s => s.Equals("BaseUrl"))).Value).Returns(baseUrl);
            }

            httpHandlerMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(message);

            currencyCalculatorMock.Setup(x => x.CalculatePrice(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedCalcValue);

            service = new CurrencyConverterService(new Logger<CurrencyConverterService>(new LoggerFactory()), currencyCalculatorMock.Object, httpHandlerMock.Object, configMock.Object);
        }

        // successful call
        [Fact]
        public async Task CalculatePriceSuccessful()
        {
            // Arrange
            var message = new HttpResponseMessage();
            message.StatusCode = System.Net.HttpStatusCode.OK;
            message.Content = new StringContent(responseJson);

            SetupConfig("test.com", message, 13.84935M);

            // Act
            var result = await service.CalculatePrice(10, Enums.RateTypes.GBP, Enums.RateTypes.USD);

            // Assert
            Assert.Equal(13.84935M, result);
        }

        // original price is less than 0
        [Fact]
        public async Task InvalidOriginalPriceValueError()
        {
            // Arrange
            var message = new HttpResponseMessage();
            message.StatusCode = System.Net.HttpStatusCode.OK;
            message.Content = new StringContent(responseJson);

            SetupConfig("test.com", message, 13.84935M);

            // Act
            var result = await Assert.ThrowsAsync<ArgumentException>(() => service.CalculatePrice(-1, Enums.RateTypes.GBP, Enums.RateTypes.USD));

            // Assert
            Assert.Contains("Original price argument is not a valid value", result.Message);
        }

        // response from api unsucessful
        [Fact]
        public async Task InvalidResponseFromRatesApi()
        {
            // Arrange
            var message = new HttpResponseMessage();
            message.StatusCode = System.Net.HttpStatusCode.NotFound;
            message.Content = new StringContent(responseJson);

            SetupConfig("test.com", message, 13.84935M);

            // Act
            var result = await service.CalculatePrice(10, Enums.RateTypes.GBP, Enums.RateTypes.USD);

            // Assert
            Assert.Equal(-1, result);
        }

        // calculation return value less than 0
        [Fact]
        public async Task InvalidValueFromCalculator()
        {
            // Arrange
            var message = new HttpResponseMessage();
            message.StatusCode = System.Net.HttpStatusCode.OK;
            message.Content = new StringContent(responseJson);

            SetupConfig("test.com", message, -1);

            // Act
            var result = await service.CalculatePrice(10, Enums.RateTypes.GBP, Enums.RateTypes.USD);

            // Assert
            Assert.Equal(-1, result);
        }

        // config is null or doesnt exist
        [Fact]
        public async Task MissingConfigExcepetion()
        {
            // Arrange
            var message = new HttpResponseMessage();
            message.StatusCode = System.Net.HttpStatusCode.OK;
            message.Content = new StringContent(responseJson);

            SetupConfig("test.com", message, 10, true);

            // Act
            var result = await service.CalculatePrice(10, Enums.RateTypes.GBP, Enums.RateTypes.USD);

            // Assert
            Assert.Equal(-1, result);
        }

        // Failed to deserialize json
        [Fact]
        public async Task DeserializationError()
        {
            // Arrange
            var message = new HttpResponseMessage();
            message.StatusCode = System.Net.HttpStatusCode.OK;
            message.Content = new StringContent("{\"test\" : \"hello\", \"test2\" : \"world\"}");

            SetupConfig("test.com", message, 10, false);

            // Act
            var result = await service.CalculatePrice(10, Enums.RateTypes.GBP, Enums.RateTypes.USD);

            // Assert
            Assert.Equal(-1, result);
        }
    }
}
