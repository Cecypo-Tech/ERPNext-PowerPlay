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
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Warehouse
    {
        public int ID { get; set; }
        public string name { get; set; }
        public bool selected { get; set; }
    }

    public class WarehouseRoot
    {
        public List<Warehouse> data { get; set; }
    }



}
