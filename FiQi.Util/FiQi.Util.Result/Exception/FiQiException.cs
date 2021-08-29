using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiQi.Util.Result
{
    /// <summary>
    /// FiQi自定义异常
    /// </summary>
    public class FiQiException : Exception
    {
        public IFiQiResult Result { get; set; }
        public FiQiException(int code, string message)
            : base(message)
        {
            Result = IFiQiResult.Create(code, message);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
