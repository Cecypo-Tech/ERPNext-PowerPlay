using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPNext_PowerPlay.Models
{
    public class BaseEntity
    {
        //[Browsable(false) ]
        public DateTime? DateCreated { get; set; }
        //[Browsable(false)]
        public DateTime? DateModified { get; set; }
    }
}
