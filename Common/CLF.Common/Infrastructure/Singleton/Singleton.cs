﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Common.Infrastructure.Singleton
{
    public class Singleton<T> : BaseSingleton
    {
        private static T instance;
        public static T Instance
        {
            get => instance;
            set
            {
                instance = value;
                AllSingletons[typeof(T)] = value;
            }
        }
    }
}
