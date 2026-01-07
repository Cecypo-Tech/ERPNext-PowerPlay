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
        [Index(nameof(DocType), nameof(Name), nameof(Date))]
        public class data
        {
            [Key]
            public int Job { get; set; }
            public DateTime JobDate { get; set; }
            public DocType DocType { get; set; }
            public DateTime Date { get; set; }    //Can be any of posting_date, transaction_date, date
            public string Owner { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }   //Can be any of customer_name, supplier_name, title
            public string Status { get; set; }
            public double Grand_Total { get; set; }

            public int custom_print_count { get; set; }
            public string Set_Warehouse { get; set; }
        }

        public class FrappeDocList
        {
            public List<data> data { get; set; }
        }

    }
}
