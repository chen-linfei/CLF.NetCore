using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CLF.Common.Configuration
{
    public partial class AppConfig
    {
        public string SqlServerConnectionString { get; set; }

        //redis
        public string RedisConnectionString { get; set; }
        public int? RedisDatabaseId { get; set; }
        public bool RedisEnabled { get; set; }
        public bool UseRedisForCaching { get; set; }

        //log
        public string LogFilePath { get; set; }
        public string LogEventLevel { get; set; }
        public string RollingInterval { get; set; }
    }


}
