using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_OTO_SUSK_Entities
{
    public class YARIMAMULISEMRIMKA
    {
        public string REFISEMRINO { get; set; }
        public string ANAMAMUL { get; set; }
        public string STOK_KODU { get; set; }
        public decimal RECETE_MIKTAR { get; set; }
        public decimal IHTIYAC_ISEMRIMIKTAR { get; set; }
        public string YMAMUL_ISEMRINO { get; set; }
        public decimal YMAMUL_ISEMRIMIKTAR { get; set; }
        public decimal BAKIYE115 { get; set; }
        public decimal BAKIYE118 { get; set; }
        public decimal ACILACAK_ISEMRIMIKTAR { get; set; }
        public DateTime TARIH { get; set; }
    }
}
