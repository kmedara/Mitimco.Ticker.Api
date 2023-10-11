using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker.Domain.Ticker.AlphaCalculationStrategy
{
    public class CAPMFormulaStrategy : IAlphaCalculationStrategy
    {
        public decimal Beta(decimal[] historical, decimal[] benchmark)
        {
            if (historical?.Length != benchmark?.Length)
                throw new ArgumentException("Data sets are not of equal length");

            return Covariance(historical, benchmark) / Variance(historical);
        }

        /// <summary>
        /// Data should be in ascending order
        /// </summary>
        /// <param name="historicalReturns"></param>
        /// <param name="benchMarkReturns"></param>
        /// <param name="riskFreeRate"></param>
        /// <returns></returns>
        public decimal Calculate(decimal[] historicalReturns, decimal[] benchMarkReturns, decimal riskFreeRate)
        {

                       
            var beta = Beta(historicalReturns, benchMarkReturns);

            var market  = CumulativeReturn(benchMarkReturns);

            var expected = ExpectedReturn(riskFreeRate, beta, market);

            var actual = CumulativeReturn(historicalReturns);

            return actual - expected;   
        }

        private decimal ExpectedReturn(decimal riskFreeRate, decimal beta, decimal cumulativeReturn) =>
            riskFreeRate + beta * (cumulativeReturn - riskFreeRate);
        
            //Expected Return (ER) = Risk-Free Rate + Beta * (Market Return - Risk-Free Rate)

        

        /// <summary>
        /// Cumulative return is the total change in the investment price over a set time - an aggregate/scalar return, not annualized<br/>
        /// Data should be in ascending order
        /// </summary>
        /// <param name="riskFreeRate"></param>
        /// <param name="beta"></param>
        /// <param name="returns"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private decimal CumulativeReturn(decimal[] returns) =>
            (returns.Last() - returns.First()) / returns.First();

        /// <summary>
        /// https://www.investopedia.com/terms/c/covariance.asp
        /// </summary>
        /// <param name="historical">returns</param>
        /// <param name="benchmark">returns</param>
        /// <returns></returns>
        public decimal Covariance(decimal[] historical, decimal[] benchmark)
        {
            var hAvg = historical.Average();
            var bAvg = benchmark.Average();
            decimal covariance = 0m;

            for(int i = 0; i < benchmark.Length; i++)
            {
                covariance += (historical[i] - hAvg) * (benchmark[i] - bAvg);
            }

            return covariance / (historical.Length - 1);
        }

        public decimal SqRt(decimal[] historical)
        {
            var va = (double)Variance(historical);
            return (decimal)Math.Sqrt(va);
        }

        /// <summary>
        /// https://www.calculatorsoup.com/calculators/statistics/variance-calculator.php
        /// </summary>
        /// <param name="returns">returns</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public decimal Variance(decimal[] returns)
        {
            //find mean
            var mean = returns.Average();

            //find squared difference from mean for each data value. (subtract mean from each and square result)
            ///find the sum of all squared differences
            var variance = 0m;
            for(int i = 0;i < returns.Length; i++)
            {
                variance += (returns[i] - mean) * (returns[i] - mean);
            }

            ///variance is sum of squared differences divided by number of data points (use - 1 in denominator for sampling)

            return variance / (returns.Length - 1);
        }
    }
}
