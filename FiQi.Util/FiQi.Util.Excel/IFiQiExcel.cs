using NPOI.SS.UserModel;
using System.IO;
using System.Threading.Tasks;

namespace FiQi.Util.Excel
{
    public interface IFiQiExcel
    {
        /// <summary>
        /// 插入一个页签数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelSheet"></param>
        /// <param name="workbook"></param>
        /// <returns></returns>
        Task SetSheet<T>(ExcelSheet<T> excelSheet, IWorkbook workbook);

        /// <summary>
        /// 对象导出到excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelSheet">页签</param>
        /// <param name="name">excel文件名</param>
        /// <returns></returns>
        Task<string> OutputExcel<T>(ExcelSheet<T> excelSheet, string name = null);

        /// <summary>
        /// 导出到excel stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelSheet">页签</param>
        /// <param name="name">excel文件名</param>
        /// <returns></returns>
        Task<byte[]> OutputExcelStream<T>(ExcelSheet<T> excelSheet);

        /// <summary>
        /// 两个页签导出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<string> OutputExcel<T, K>(ExcelSheet<T> sheet1, ExcelSheet<K> sheet2, string name = null);

        Task<byte[]> OutputExcelStream<T, K>(ExcelSheet<T> sheet1, ExcelSheet<K> sheet2);
    }
}
