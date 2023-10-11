using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker.Domain.Ticker.AlphaCalculationStrategy
{
    public interface IAlphaCalculationStrategy
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="historicalData">Historical price points (not returns)</param>
        /// <param name="benchMarkData">>Historical price points (not returns)</param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="riskFreeRate"></param>
        /// <returns></returns>
        public decimal Calculate(decimal[] historicalReturns, decimal[] benchMarkReturns, decimal riskFreeRate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="historical">Historical Returns</param>
        /// <param name="benchmark">Benchmark Returns</param>
        /// <returns></returns>
        public decimal Beta(decimal[] historical, decimal[] benchmark);

        public decimal Covariance(decimal[] historical, decimal[] benchmark);

        public decimal Variance(decimal[] historical);

        public decimal SqRt(decimal[] historical);
    }
}
