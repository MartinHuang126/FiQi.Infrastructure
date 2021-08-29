using System;
#nullable enable

namespace FiQi.Util.Result
{
    /// <summary>
    /// 统一返回结果
    /// </summary>
    public interface IFiQiResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string? Message { get; set; }


        public static FiQiResult Create(int code, string? message)
        {
            return new FiQiResult(code, message);
        }

        public static FiQiResult<T> Create<T>(int code, string? message, T data)
        {
            return new FiQiResult<T>(code, message, data);
        }

        public static FiQiPageResult<T> Create<T>(int code, string? message, T data, PageResultInfo pageInfo)
        {
            return new FiQiPageResult<T>(code, message, data, pageInfo);
        }

        public static FiQiResult Success(string? message)
        {
            return new FiQiResult((int)ResultCode.Success, message);
        }

        public static FiQiResult Error(string? message)
        {
            return new FiQiResult((int)ResultCode.Error, message);
        }

        public static FiQiResult<T> Success<T>(string? message, T data)
        {
            return new FiQiResult<T>((int)ResultCode.Success, message, data);
        }

        public static FiQiResult<T> Error<T>(string? message, T data)
        {
            return new FiQiResult<T>((int)ResultCode.Error, message, data);
        }

        public static FiQiPageResult<T> Success<T>(string? message, T data, PageResultInfo pageInfo)
        {
            return new FiQiPageResult<T>((int)ResultCode.Success, message, data, pageInfo);
        }

        public static FiQiPageResult<T> Error<T>(string? message, T data, PageResultInfo pageInfo)
        {
            return new FiQiPageResult<T>((int)ResultCode.Error, message, data, pageInfo);
        }
    }

    /// <summary>
    /// 分页请求
    /// </summary>
    public interface IFiQiPageRequst<T>
    {
        public PageInfo PageInfo { get; set; }
    }
}
