using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticker.Domain.Ticker.AlphaCalculationStrategy;

namespace Ticker.Domain.Ticker
{
    public abstract class BaseTickerCalculator : ITickerCalculator
    {
        public IAlphaCalculationStrategy? _alphaStrategy { get; private set; }


        #region alpha
        public decimal Alpha(decimal[] historicalData, decimal[] benchmarkData, decimal riskFreeRate)
        {

            if (_alphaStrategy != null)
                return this._alphaStrategy.Calculate(historicalData, benchmarkData, riskFreeRate);
            else 
                throw new NotImplementedException("No strategy set for alpha calculation");
        }
        #endregion

        /// <summary>
        /// Data must be in ascending order by date
        /// </summary>
        /// <param name="timeSeries"></param>
        /// <returns></returns>
        public decimal[] Returns(decimal[] timeSeries)
        {
            decimal[] results = new decimal[timeSeries.Length];

            for (int i = 1; i < timeSeries.Length; i++)
            {
                decimal previous = timeSeries[i - 1];
                decimal current = timeSeries[i];
                decimal returnPercentage = (current - previous) / previous;
                results[i - 1] = returnPercentage;
            }

            return results.Where(el => el != 0).ToArray();
        }
        public void SetAlphaStrategy(IAlphaCalculationStrategy strategy)
        {
            this._alphaStrategy = strategy;
        }

        public Tuple<Matrix<double>, Vector<double>> CreateDataMatrix(double[] stockReturns, double[] benchmarkReturns)
        {
            if (stockReturns.Length != benchmarkReturns.Length)
                throw new ArgumentException("Data arrays must have the same length.");

            var dataMatrix = Matrix<double>.Build.DenseOfColumns(new[] { Vector<double>.Build.Dense(stockReturns), Vector<double>.Build.Dense(benchmarkReturns) });
            var ones = Vector<double>.Build.Dense(stockReturns.Length, 1.0);

            return Tuple.Create(Matrix<double>.Build.DenseOfColumns(new[] { ones, dataMatrix.Column(1) }), dataMatrix.Column(0));
        }

    }
}
