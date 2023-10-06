using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker.Api.Utility
{
    /// <summary>
    /// Internal function routes
    /// </summary>
    internal static class Routes
    {
        public static class Ticker
        {
            private const string _prefix = "{ticker}";
            public static class Returns {

                public const string Get = $"{_prefix}/returns";
            }

            public static class Alpha
            {

                public const string Get = $"{_prefix}/alpha";
            }
        }

    }
}
