using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class DataService
    {
        DataModel ctx = new DataModel();
        public List<ExcelDataRow> getRowsFromExcel(string path)
        {                       

            IWorkbook wb;
            var fs = new FileStream(path, FileMode.Open);
            try
            {
                wb = new XSSFWorkbook(fs);
            }
            catch (Exception)
            {
                fs.Close();
                fs = new FileStream(path, FileMode.Open);
                wb = new HSSFWorkbook(fs);
            }
            var rows = new List<ExcelDataRow>();
            for (int sheetN = 0; sheetN < 2; sheetN++)
            {
                var sheet = wb.GetSheetAt(sheetN);

                var nextRow = sheet.GetRow(1);
                if (nextRow == null)
                    return null;

                var dataRow = new ExcelDataRow();
                for (int i = 0; i < 20; i++)
                    dataRow.cells[i] = getValueFromCell(nextRow.GetCell(i));
                rows.Add(dataRow);
            }

            return rows;
        }

        public string edit(ExcelDataRow row)
        {
            var dbRow = ctx.ExcelDataTables.Find(row.idRow);
            dbRow.Data = JsonConvert.SerializeObject(row);
            ctx.SaveChanges();
            return "edited.";
        }

        public string delete(int id)
        {
            var item = ctx.ExcelDataTables.Find(id);
            if (item == null)
                return "row not found.";
                ctx.ExcelDataTables.Remove(item);
            ctx.SaveChanges();
            return "deleted";
        }

        public List<RowInfo> listAll()
        {
           var all = ctx.ExcelDataTables.OrderByDescending(d => d.dateCreate).ToList();
            var list = new List<RowInfo>();
            foreach(var item in all)
            {
               var row = JsonConvert.DeserializeObject<ExcelDataRow>(item.Data);
                list.Add(new RowInfo { idRow = row.idRow, cells = row.cells, dateCreated = item.dateCreate });
            }
            return list;
        }

        private string getValueFromCell(ICell cell)
        {
            if (cell == null)
                return null;

            cell.SetCellType(CellType.String);

            switch (cell.CellType)
            {
                case CellType.Numeric:
                    return ((int)cell.NumericCellValue).ToString();
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Unknown:
                case CellType.Blank:
                case CellType.Boolean:
                case CellType.Error:
                case CellType.Formula:
                default:
                    return null;
            }
        }

        public void saveRowsFromExcel(string fileLocation)
        {
           var rows = getRowsFromExcel(fileLocation);
            ctx.ExcelDataTables.AddRange(
                rows.Select(r => new ExcelDataTable { Data = JsonConvert.SerializeObject(r), dateCreate = DateTime.Now }));
            ctx.SaveChanges();
        }
    }
}
