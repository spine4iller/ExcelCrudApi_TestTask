using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class ExcelDataRow
    {       
        public int idRow { get; set; }
        public string[] cells { get; set; } = new string[20];
    }
    public class RowInfo : ExcelDataRow
    {
        public DateTime dateCreated { get; set; }
    }
}
