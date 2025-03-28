using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPNext_PowerPlay.Models
{
    public class Cred
    {
        public int ID { get; set; }
        public string URL { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }

    }
    public class CredList
    {
        public int ID { get; set; }
        public List<Cred> CredentialList { get; } = new();
    }
}
