using System;

namespace FiQi.Util.Result
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Index { get; set; } = 1;

        /// <summary>
        /// 每页数据量
        /// </summary>
        public int Size { get; set; } = 20;
    }

    /// <summary>
    /// 分页结果信息
    /// </summary>
    public class PageResultInfo : PageInfo
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int Count { get; set; }
    }
}
