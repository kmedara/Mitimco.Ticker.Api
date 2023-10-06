using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker.Domain
{
    /// <summary>
    /// Call Plot() after instantiation to plot linear regression 
    /// </summary>
    public sealed class LinearRegression
    {
        private readonly double[] _x;

        private readonly double[] _y;

        /// <summary>The r^2 value of the line.  Used to give an idea of the accuracy given the input values</summary>
        public double RSquared { get; private set; }
        /// <summary>The y-intercept value of the line (i.e. y = ax + b, yIntercept is b).</summary>
        public double YIntercept { get; private set; }
        /// <summary>The slop of the line (i.e. y = ax + b, slope is a).</summary>
        public double Slope { get; private set; }

        public LinearRegression(double[] x, double[] y)
        {

            if (x.Length != y.Length)
                throw new Exception("Input values should be with the same length.");

            _x = x;
            _y = y;

        }

        public double CalculatePrediction(double input)
        {
            return (input * Slope) + YIntercept;
        }


        public void Plot()
        {
            double sumOfX = 0;
            double sumOfY = 0;
            double sumOfXSq = 0;
            double sumOfYSq = 0;
            double sumCodeviance = 0;

            for (var i = 0; i < _x.Length; i++)
            {
                var x = _x[i];
                var y = _y[i];
                sumCodeviance += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }

            var count = _x.Length;
            var variance = sumOfXSq - ((sumOfX * sumOfX) / count);
            var ssY = sumOfYSq - ((sumOfY * sumOfY) / count);

            var rNumerator = (count * sumCodeviance) - (sumOfX * sumOfY);
            var rDenom = (count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY));
            var covariance = sumCodeviance - ((sumOfX * sumOfY) / count);

            var meanX = sumOfX / count;
            var meanY = sumOfY / count;
            var dblR = rNumerator / Math.Sqrt(rDenom);

            RSquared = dblR * dblR;
            YIntercept = meanY - ((covariance / variance) * meanX);
            Slope = covariance / variance;
        }
    }
}
