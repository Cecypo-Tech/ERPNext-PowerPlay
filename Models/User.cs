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
    public class User
    {
        public int ID { get; set; }
        public string FullName{ get; set; }
        public string Email{ get; set; }
        public bool selected { get; set; }
    }
}
