using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_OTO_SUSK_Entities
{
    public class TBLSERITRA
    {
        
        public string STOK_KODU { get; set; }
        [Key]
        public string SERI_NO { get; set; }
        public int ADET { get; set; }

    }
}
