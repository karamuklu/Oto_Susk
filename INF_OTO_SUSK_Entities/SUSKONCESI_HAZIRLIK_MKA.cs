using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_OTO_SUSK_Entities
{
    public class SUSKONCESI_HAZIRLIK_MKA
    {
        public string STOK_KODU { get; set; }
        public string MAMULMU { get; set; }
        public decimal RECETE_MIKTAR { get; set; }
        public decimal TRANSFERMIKTAR { get; set; }
        public decimal BAKIYE117 { get; set; }
        public decimal BAKIYE114 { get; set; }
        public decimal BAKIYE115 { get; set; }
        public int GIRISDEPOKODU { get; set; }
        public int CIKISDEPOKODU { get; set; }
        public string ANAMAMUL { get; set; }
        public string REFISEMRINO { get; set; }
        public decimal ANAMAMULISEMRIMIKTAR { get; set; }
        public int SEVIYE { get; set; }
    }
}
