using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrincaChurras.Support.Models
{
    public class ServerPagination
    {
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
    }
}