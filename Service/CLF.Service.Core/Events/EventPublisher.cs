using CLF.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLF.Service.Core.Events
{
    public partial class EventPublisher : IEventPublisher
    {
        public virtual void Publish<TEvent>(TEvent @event)
        {
            //获取所有清除缓的对象
            var consumers = EngineContext.Current.ResolveAll<IConsumer<TEvent>>().ToList();

            foreach (var consumer in consumers)
            {
                try
                {
                    consumer.HandleEvent(@event);
                }
                catch (Exception exception)
                {
                    try
                    {
                        throw exception;
                    }
                    catch { }
                }
            }
        }
    }
}
