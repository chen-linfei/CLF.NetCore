using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Common.Infrastructure.Mapper
{
    public interface IOrderedMapperProfile
    {
        int Order { get; }
    }
}
