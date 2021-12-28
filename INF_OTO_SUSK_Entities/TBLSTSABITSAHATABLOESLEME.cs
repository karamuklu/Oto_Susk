using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_OTO_SUSK_Entities
{
    public class TBLSTSABITSAHATABLOESLEME
    {
        [Key]
        public string STOK_KODU { get; set; }

        public string KT_SUSK_ILISKISI { get; set; }
    }
}
