using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Common.Infrastructure.Singleton
{
    public class SingletonList<T> : Singleton<IList<T>>
    {
        static SingletonList()
        {
            Singleton<IList<T>>.Instance = new List<T>();
        }
        public new static IList<T> Instance => Singleton<IList<T>>.Instance;
    }
}
