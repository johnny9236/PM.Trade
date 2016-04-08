using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace TradeTest
{
    public class CodeFirstDbContext : DbContext
    {
        //public DbSet<BOCQueryAccountDtlModel> BOC { get; set; }

        //public DbSet<AptitudeCode> AptitudeCode { get; set; }

        //public DbSet<EntrpriseInfo> EntrpriseInfo { get; set; }


        //public DbSet<EnterpriseSpeciality> EnterpriseSpeciality { get; set; }

        //public DbSet<Personnel> Personnel { get; set; }

        //public DbSet<PersonnelSpeciality> PersonnelSpeciality { get; set; }


      //  public DbSet<TradeTest.model.ICBCQueryInfo> ICBCQueryInfo { get; set; }
        public DbSet<TradeTest.model.ICBCRtnQueryInfo> ICBCRtnQueryInfo { get; set; }

        




        public CodeFirstDbContext() : base("DefautConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BOCQueryAccountDtlModel>();
        }
    }
}
