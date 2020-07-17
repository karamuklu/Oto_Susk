using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_OTO_SUSK_Entities
{
    public class INF_OTOSUSK_Context : DbContext
    {
        public DbSet<SUSK_LISTESI_MKA> SUSK_LISTESI_MKA { get; set; }
        public DbSet<TBLISEMRI> TBLISEMRI { get; set; }
        public DbSet<DEPOLOKASYONDURUM_MKA> DEPOLOKASYONDURUM_MKA { get; set; }
        public DbSet<TBLDEPHAR> TBLDEPHAR { get; set; }
        public DbSet<TBLSTHAR> TBLSTHAR { get; set; }

        //Tabloların sonuna s eklememesi için OnModelCreating metodunu ezdik
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TBLISEMRI>().ToTable("SUSK_LISTESI_MKA");
            modelBuilder.Entity<TBLISEMRI>().ToTable("TBLISEMRI");
            modelBuilder.Entity<DEPOLOKASYONDURUM_MKA>().ToTable("DEPOLOKASYONDURUM_MKA");
            modelBuilder.Entity<TBLDEPHAR>().ToTable("TBLDEPHAR");
            modelBuilder.Entity<TBLSTHAR>().ToTable("TBLSTHAR");
        }

    }
    
}
