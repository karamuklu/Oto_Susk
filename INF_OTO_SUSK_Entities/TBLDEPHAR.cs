using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_OTO_SUSK_Entities
{
    public class TBLDEPHAR
    {
        [Key]
        public string STOKKODU { get; set; }
        public string HUCREKODU { get; set; }
        public decimal NETMIKTAR { get; set; }
        public string GC { get; set; }
        public DateTime TARIH { get; set; }
        public string HTUR { get; set; }
        public string DEPOHARFISNO { get; set; }
        public string KAYITYAPANKUL { get; set; }
        public DateTime KAYITTARIHI { get; set; }
        public int? STHARINC { get; set; }
    }
}
