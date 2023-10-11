using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticker.Domain.Ticker.AlphaCalculationStrategy;

namespace Ticker.Domain.Ticker
{
    public interface ITickerCalculator
    {

        public void SetAlphaStrategy(IAlphaCalculationStrategy alphaStrategy);

        public decimal[] Returns(decimal[] timeSeries);

        public decimal Alpha(decimal[] historicalData, decimal[] benchMarkData, decimal riskFreeRate);

        public Tuple<Matrix<double>, Vector<double>> CreateDataMatrix(double[] stockReturns, double[] benchmarkReturns);


    }
}
