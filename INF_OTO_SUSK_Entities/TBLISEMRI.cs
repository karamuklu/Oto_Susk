using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_OTO_SUSK_Entities
{
    public class TBLISEMRI
    {
        [Key]
        public string ISEMRINO { get; set; }
        public string STOK_KODU { get; set; }
        public decimal MIKTAR { get; set; }
        public DateTime TARIH { get; set; }
        public string USK_STATUS { get; set; }
        public string KAPALI { get; set; }
    }
}
