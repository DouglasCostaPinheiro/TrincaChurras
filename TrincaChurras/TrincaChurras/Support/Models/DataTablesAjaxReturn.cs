using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrincaChurras.Support.Models
{
    public class DataTablesAjaxReturn
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<List<object>> data { get; set; } = new List<List<object>>();
    }
}