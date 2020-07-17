using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_OTO_SUSK_Entities
{
    public class DEPOLOKASYONDURUM_MKA
    {
        public string STOKKODU { get; set; }
        [Key]
        public string HUCREKODU { get; set; }
        public Int16 DEPO_KODU { get; set; }
        public decimal NETBAKIYE { get; set; }
    }
}
