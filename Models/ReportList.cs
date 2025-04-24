using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPNext_PowerPlay.Models
{
    public class ReportList : BaseEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int ID { get; set; }

        public bool Enabled { get; set; }
        public string ReportName { get; set; }
        public DocType DocType { get; set; }
        public string EndPoint { get; set; }
        public string FieldList { get; set; }
        public string? FilterList { get; set; }
        
    }
}
