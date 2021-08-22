using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace FiQi.Redis
{
    /// <summary>
    /// FiQiredis参数
    /// </summary>
    public class FiQiRedisOptions : IOptions<FiQiRedisOptions>
    {
        /// <summary>
        /// 默认数据库
        /// </summary>
        public int DefaultDatabase { get; set; }

        /// <summary>
        /// 默认数据库名
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 环境，redis里面加前缀
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        public FiQiRedisOptions Value => this;
    }
}
