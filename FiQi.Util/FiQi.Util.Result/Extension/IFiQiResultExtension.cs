using System;
#nullable enable

namespace FiQi.Util.Result
{
    /// <summary>
    /// 统一返回结果扩展
    /// </summary>
    public static class IFiQiResultExtension
    {
        public static bool IsSuccess(this IFiQiResult fiQiResult)
        {
            if (fiQiResult is null)
            {
                throw new FiQiException((int)ResultCode.Error, "fiQiResult is null");
            }
            return fiQiResult.Code == (int)ResultCode.Success;
        }
    }
}
