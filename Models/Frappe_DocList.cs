using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ERPNext_PowerPlay.Models
{
    public class Frappe_DocList
    {
        public class data
        {
            public int ID { get; set; }
            public string name { get; set; }
            public string customer { get; set; }
            public string posting_date { get; set; }
            public int docstatus { get; set; }
            public string status { get; set; }
            public string etr_invoice_number { get; set; }
            public int exempt_from_sales_tax { get; set; }
            public double total_taxes_and_charges { get; set; }
            public double total { get; set; }

            public int custom_print_count { get; set; }
        }

        public class FrappeDocList
        {
            public List<data> data { get; set; }
        }

    }
}
