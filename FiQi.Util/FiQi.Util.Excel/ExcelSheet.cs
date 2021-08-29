using System;
using System.Collections.Generic;

namespace FiQi.Util.Excel
{
    public class ExcelSheet<T>
    {
        /// <summary>
        /// 页签名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string[] Title { get; set; }

        /// <summary>
        /// 内容列表
        /// </summary>
        public List<T> List { get; set; }
    }
}
