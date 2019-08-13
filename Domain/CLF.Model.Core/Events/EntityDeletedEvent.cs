using CLF.Model.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Model.Core.Events
{
    public class EntityDeletedEvent<T> where T : BaseEntity
    {
        public EntityDeletedEvent(T entity)
        {
            Entity = entity;
        }
        public T Entity { get; }
    }
}
