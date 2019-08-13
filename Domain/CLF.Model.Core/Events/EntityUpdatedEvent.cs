using CLF.Model.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Model.Core.Events
{
    public class EntityUpdatedEvent<T> where T : BaseEntity
    {
        public EntityUpdatedEvent(T entity)
        {
            Entity = entity;
        }
        public T Entity { get; }
    }
}
