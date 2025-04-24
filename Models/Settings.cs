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
    public class Settings : BaseEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int ID { get; set; }

        public bool Enabled { get; set; }
        public string Name { get; set; }
   
    }
}
