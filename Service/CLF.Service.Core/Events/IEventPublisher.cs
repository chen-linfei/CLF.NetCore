using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Service.Core.Events
{
    public partial interface IEventPublisher
    {
        void Publish<TEvent>(TEvent @event);
    }
}
