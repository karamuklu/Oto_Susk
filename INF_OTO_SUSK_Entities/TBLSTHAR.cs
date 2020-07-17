using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_OTO_SUSK_Entities
{
    public class TBLSTHAR
    {
        public string FISNO { get; set; }
        [Key]
        public string STOK_KODU { get; set; }
        public Int16 SIRA { get; set; }
        public int INCKEYNO { get; set; }
        public Int16 DEPO_KODU { get; set; }
    }
}
