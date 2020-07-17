using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_OTO_SUSK_Entities
{
    public class MEKANIK_TRANSFER
    {
        [Key]
        public string HAM_KODU { get; set; }
        public double ILKTRANSFERMIKTAR { get; set; }
        public int ILKTRANSFER_DEPO { get; set; }
        public double IKINCITRANSFER { get; set; }
        public int IKINCITRANSFER_DEPO { get; set; }
       
    }
}
