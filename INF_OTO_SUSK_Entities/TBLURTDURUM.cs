using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_OTO_SUSK_Entities
{
    public class TBLURTDURUM
    {
        
        public string SERI_NO { get; set; }
        [Key]
        public string STOK_KODU { get; set; }
        public string ISEMRINO { get; set; }
        public string SUSK_DURUMU { get; set; }
        public string URETIM_ASAMA { get; set; }
        public int INCKEYNO { get; set; }
    }
}
