using System;
using System.Collections.Generic;
using System.Text;
using trainline.rates.common;
using Xunit;

namespace trainline.rates.tests
{
    public class CurrencyCalculatorTests
    {
        // public decimal CalculatePrice(decimal originalPrice, decimal exchangeRate)
        private readonly CurrencyCalculator calc;

        public CurrencyCalculatorTests()
        {
            calc = new CurrencyCalculator();
        }

        [Fact]
        public void CalculatePriceSucessfully()
        {
            // Act
            var result = calc.CalculatePrice(1, 1.5M);
            // Assert
            Assert.Equal(1.5M, result);


            // Act
            result = calc.CalculatePrice(12.5M, 1.5M);
            // Assert
            Assert.Equal(18.75M, result);


            // Act
            result = calc.CalculatePrice(0.23M, 21.33M);
            // Assert
            Assert.Equal(4.9059M, result);


            // Act
            result = calc.CalculatePrice(0.75M, 0.225M);
            // Assert
            Assert.Equal(0.16875M, result);


            // Act
            result = calc.CalculatePrice(123456789, 55M);
            // Assert
            Assert.Equal(6790123395M, result);
        }

        [Fact]
        public void InvalidValuesException()
        {
            // Act
            var result = Assert.Throws<ArgumentException>(() => calc.CalculatePrice(-1, 10));

            // Assert
            Assert.Contains("Invalid argument in CalculatePrice: originalPrice", result.Message);
        }
    }
}
