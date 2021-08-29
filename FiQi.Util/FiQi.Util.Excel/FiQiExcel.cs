using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FiQi.Util.Excel
{
    public class FiQiExcel : IFiQiExcel
    {
        readonly string _rootPath;
        readonly string _filePath;
        readonly string _fileExt;
        readonly string _folder;
        public FiQiExcel(string folder)
        {
            _filePath = "/FileOutput";
            _rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _filePath);
            if (!Directory.Exists(_rootPath))
            {
                Directory.CreateDirectory(_rootPath);
            }
            _fileExt = ".xlsx";
        }

        /// <summary>
        /// 对象导出到excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelSheet">页签</param>
        /// <param name="name">excel文件名</param>
        /// <returns></returns>
        public async Task<string> OutputExcel<T>(ExcelSheet<T> excelSheet, string name = null)
        {
            return await OutputExcelToFile(async (workbook) =>
            {
                await SetSheet(excelSheet, workbook);
            }, name);
        }

        /// <summary>
        /// 对象导出到excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelSheet">页签</param>
        /// <param name="name">excel文件名</param>
        /// <returns></returns>
        public async Task<string> OutputExcelToFile(Func<IWorkbook, Task> setSheet, string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            }

            string root = Path.Combine(_rootPath, _folder);
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            string fileName = $"{root}{name}{_fileExt}";
            IWorkbook workbook = new XSSFWorkbook();
            await setSheet(workbook);

            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                workbook.Write(fs);
                fs.Close();
            }

            return _filePath + _folder + name + _fileExt;
        }

        /// <summary>
        /// 对象导出到excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelSheet">页签</param>
        /// <param name="name">excel文件名</param>
        /// <returns></returns>
        public async Task<byte[]> OutputExcelToBytes(Func<IWorkbook, Task> setSheet)
        {
            IWorkbook workbook = new XSSFWorkbook();
            await setSheet(workbook);

            using MemoryStream stream = new MemoryStream();
            workbook.Write(stream);

            return stream.ToArray();
        }

        public async Task SetSheet<T>(ExcelSheet<T> excelSheet, IWorkbook workbook)
        {
            ISheet sheet = workbook.CreateSheet(excelSheet.Name);
            IRow Title;
            IRow rows;
            ICellStyle style = workbook.CreateCellStyle();//创建样式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直居中
            style.Alignment = HorizontalAlignment.Center;//设置居中
            var entitys = excelSheet.List;
            var title = excelSheet.Title;
            if (entitys?.Count <= 0)
            {
                return;
            }
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();
            //列宽
            int[] columSize = new int[entityProperties.Length];
            await Task.Run(() =>
            {
                for (int i = 0; i <= entitys.Count; i++)
                {
                    if (i == 0)
                    {
                        Title = sheet.CreateRow(0);
                        for (int k = 0; k < title.Length; k++)
                        {
                            Title.CreateCell(k).SetCellValue(title[k]);
                            //找列宽最大值
                            if (columSize[k] < Encoding.UTF8.GetBytes(Title.Cells[k].ToString()).Length)
                            {
                                columSize[k] = Encoding.UTF8.GetBytes(Title.Cells[k].ToString()).Length;
                            }
                            Title.Cells[k].CellStyle = style;
                        }
                        continue;
                    }
                    else
                    {
                        rows = sheet.CreateRow(i);
                        object entity = entitys[i - 1];
                        for (int j = 0; j < entityProperties.Length; j++)
                        {
                            object[] entityValues = new object[entityProperties.Length];
                            entityValues[j] = entityProperties[j].GetValue(entity);
                            TypeCode typeCode = Convert.GetTypeCode(entityValues[j]);
                            switch (typeCode)
                            {
                                case TypeCode.Boolean:
                                    rows.CreateCell(j).SetCellValue(Convert.ToBoolean(entityValues[j].ToString()));
                                    break;
                                case TypeCode.DateTime:
                                    rows.CreateCell(j).SetCellValue(Convert.ToDateTime(entityValues[j]));
                                    break;
                                case TypeCode.DBNull:
                                case TypeCode.Empty:
                                    rows.CreateCell(j);
                                    break;
                                case TypeCode.Object:
                                case TypeCode.Char:
                                case TypeCode.Single:
                                case TypeCode.String:
                                    rows.CreateCell(j).SetCellValue(entityValues[j].ToString());
                                    break;
                                case TypeCode.Byte:
                                case TypeCode.Decimal:
                                case TypeCode.Double:
                                case TypeCode.Int16:
                                case TypeCode.Int32:
                                case TypeCode.Int64:
                                case TypeCode.SByte:
                                case TypeCode.UInt16:
                                case TypeCode.UInt32:
                                case TypeCode.UInt64:
                                    rows.CreateCell(j).SetCellValue(Convert.ToDouble(entityValues[j]));
                                    break;
                                default:
                                    rows.CreateCell(j).SetCellValue(entityValues[j].ToString());
                                    break;
                            }
                            rows.Cells[j].CellStyle = style;
                            //找列宽最大值
                            if (columSize[j] < Encoding.UTF8.GetBytes(rows.Cells[j].ToString()).Length)
                            {
                                columSize[j] = Encoding.UTF8.GetBytes(rows.Cells[j].ToString()).Length;
                            }
                        }
                    }
                }
            });
            //设置列宽
            for (int columnNum = 0; columnNum < columSize.Length; columnNum++)
            {
                sheet.SetColumnWidth(columnNum, columSize[columnNum] * 278);
            }
        }

        /// <summary>
        /// 两个页签导出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<string> OutputExcel<T, K>(ExcelSheet<T> sheet1, ExcelSheet<K> sheet2, string name = null)
        {
            return await OutputExcelToFile(async (workbook) =>
            {
                await SetSheet(sheet1, workbook);
                await SetSheet(sheet2, workbook);
            }, name);
        }

        public async Task<byte[]> OutputExcelStream<T>(ExcelSheet<T> excelSheet)
        {
            return await OutputExcelToBytes(async (workbook) =>
            {
                await SetSheet(excelSheet,workbook);
            });
        }

        public async Task<byte[]> OutputExcelStream<T, K>(ExcelSheet<T> sheet1, ExcelSheet<K> sheet2)
        {
            return await OutputExcelToBytes(async (workbook) =>
            {
                await SetSheet(sheet1, workbook);
                await SetSheet(sheet2, workbook);
            });
        }
    }
}
