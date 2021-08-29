using System;
#nullable enable
namespace FiQi.Util.Result
{
    /// <summary>
    /// 统一返回结果
    /// </summary>
    public class FiQiResult : IFiQiResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string? Message { get; set; }

        public FiQiResult(int code, string? message)
        {
            Code = code;
            Message = message;
        }
    }

    /// <summary>
    /// 统一返回结果
    /// </summary>
    public class FiQiResult<T> : FiQiResult
    {
        /// <summary>
        /// 返回的数据
        /// </summary>
        public T Data { get; set; }

        public FiQiResult(int code, string? message, T t)
            : base(code, message)
        {
            Data = t;
        }
    }

    /// <summary>
    /// 统一返回分页结果
    /// </summary>
    public class FiQiPageResult<T> : FiQiResult<T>
    {
        public PageResultInfo PageInfo { get; set; } = new PageResultInfo();

        public FiQiPageResult(int code, string? message, T t, PageResultInfo pageInfo) : base(code, message, t)
        {
            PageInfo = pageInfo;
        }
    }

    /// <summary>
    /// 统一请求分页结果
    /// </summary>
    public class FiQiPageReuset<T> : IFiQiPageRequst<T>
    {
        public PageInfo PageInfo { get; set; } = new PageInfo();
        public T? Query { get; set; } = default;

        public FiQiPageReuset(PageInfo pageInfo, T? t)
        {
            PageInfo = pageInfo;
            Query = t;
        }
    }
}
