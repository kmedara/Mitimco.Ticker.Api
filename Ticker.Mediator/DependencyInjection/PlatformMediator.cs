using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker.Mediator.DependencyInjection
{
    internal sealed class PlatformMediator : MediatR.Mediator, IPlatformMediator
    {
        public PlatformMediator(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public PlatformMediator(IServiceProvider serviceProvider, INotificationPublisher publisher) : base(serviceProvider, publisher)
        {
        }
    }
}
