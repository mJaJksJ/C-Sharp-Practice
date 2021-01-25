using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Infrastructure.Interception;
using AutoLotDAL_EF.Interception;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

namespace AutoLotDAL_EF.Models
{
    public partial class AutoLotEntities : DbContext
    {
        public AutoLotEntities()
            : base("name=AutoLotConnection")
        {
            //DbInterception.Add(new ConsoleWriterInterceptor());
            //DatabaseLogger.StartLogging();
            //DbInterception.Add(DatabaseLogger);
            var context = (this as IObjectContextAdapter).ObjectContext;
            context.ObjectMaterialized += OnObjectMaterialixed;
            context.SavingChanges += OnSavingChanges;
        }

        private void OnSavingChanges(object sender, EventArgs e)
        {
            
        }

        private void OnObjectMaterialixed(object sender, System.Data.Entity.Core.Objects.ObjectMaterializedEventArgs e)
        {
            
        }

        private static readonly DatabaseLogger DatabaseLogger = new DatabaseLogger("sqllog.txt", true);

        public virtual DbSet<CreditRisk> CreditRisks { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
