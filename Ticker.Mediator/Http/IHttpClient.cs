using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticker.Domain.AppSettings;

namespace Ticker.Mediator.Http
{
    public interface IHttpClient<T> where T : ApiConfigurationOptions
    {
    }
}
