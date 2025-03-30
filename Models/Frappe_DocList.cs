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
            public DocType DocType { get; set; }
            public string name { get; set; }
            public string title { get; set; }   //Can be any of customer_name, supplier_name, title
            public string date { get; set; }    //Can be any of posting_date, transaction_date, date
            public string status { get; set; }
            public double grand_total { get; set; }

            public int custom_print_count { get; set; }
        }

        public class FrappeDocList
        {
            public List<data> data { get; set; }
        }

    }
}
