using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Service.Core.Events
{
    public interface IConsumer<T>
    {
        void HandleEvent(T eventMessage);
    }
}
