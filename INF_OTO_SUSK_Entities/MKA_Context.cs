using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_OTO_SUSK_Entities
{
    public class MKA_Context:DbContext
    {
        public DbSet<TBLURTDURUM> TBLURTDURUM { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TBLURTDURUM>().ToTable("TBLURTDURUM");
        }
    }
}
