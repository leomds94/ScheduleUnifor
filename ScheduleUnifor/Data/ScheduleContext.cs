using Microsoft.EntityFrameworkCore;
using ScheduleUnifor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleUnifor.Data
{
    public partial class ScheduleContext : DbContext
    {
        public ScheduleContext() :
            base()
        {
            OnCreated();
        }

        public ScheduleContext(DbContextOptions<ScheduleContext> options) :
            base(options)
        {
            OnCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=VendingMachineServer;Integrated Security=True;Persist Security Info=True;User ID=;Password=");
            optionsBuilder.UseSqlServer(@"Server=tcp:scheduleinautecserver.database.windows.net,1433;Initial Catalog=ScheduleInautecDB;Persist Security Info=False;User ID=inautec;Password=Inau17tec;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            CustomizeConfiguration(ref optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }

        partial void CustomizeConfiguration(ref DbContextOptionsBuilder optionsBuilder);


        DbSet<Horario> Horarios { get; set; }

        DbSet<Reserva> Reservas { get; set; }

        DbSet<Sala> Salas { get; set; }

        partial void OnCreated();
    }
}
