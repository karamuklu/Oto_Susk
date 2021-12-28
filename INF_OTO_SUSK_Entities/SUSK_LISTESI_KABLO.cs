using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_OTO_SUSK_Entities
{
    public class SUSK_LISTESI_KABLO
    {
        public DateTime TARIH { get; set; }
        [Key]
        public string ISEMRINO { get; set; }
        public string STOK_KODU { get; set; }
        public decimal MIKTAR { get; set; }
        public int SUSKDEPOGIRIS { get; set; }
        public int SUSKDEPOCIKIS { get; set; }
    }
}
