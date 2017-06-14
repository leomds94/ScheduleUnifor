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

        DbSet<AulaHorario> AulaHorarios { get; set; }

        DbSet<Bloco> Blocos { get; set; }

        DbSet<DiaLetivo> DiaLetivos { get; set; }

        DbSet<Horario> Horarios { get; set; }

        DbSet<Horario> Periodos { get; set; }

        DbSet<Reserva> Reservas { get; set; }

        DbSet<Sala> Salas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.ReservasMapping(modelBuilder);
            this.CustomizeReservasMapping(modelBuilder);

            this.AulaHorarioMapping(modelBuilder);
            this.CustomizeAulaHorarioMapping(modelBuilder);

            this.BlocoMapping(modelBuilder);
            this.CustomizeBlocoMapping(modelBuilder);

            this.DiaLetivoMapping(modelBuilder);
            this.CustomizeDiaLetivoMapping(modelBuilder);

            this.HorarioMapping(modelBuilder);
            this.CustomizeHorarioMapping(modelBuilder);

            this.SalaMapping(modelBuilder);
            this.CustomizeSalaMapping(modelBuilder);


            RelationshipsMapping(modelBuilder);
            CustomizeMapping(ref modelBuilder);
        }

        partial void OnCreated();

        private void ReservasMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reserva>().ToTable(@"Reservas");
            modelBuilder.Entity<Reserva>().Property(x => x.Sala).HasColumnName(@"Sala").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Reserva>().Property(x => x.AulaHorario).HasColumnName(@"AulaHorario").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Reserva>().HasKey(r => new
            {
                r.Sala,
                r.AulaHorario
            });
        }

        partial void CustomizeReservasMapping(ModelBuilder modelBuilder);

        private void AulaHorarioMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AulaHorario>().ToTable(@"AulaHorario");
            modelBuilder.Entity<AulaHorario>().Property(x => x.Horario).HasColumnName(@"Horario").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<AulaHorario>().Property(x => x.DiaLetivo).HasColumnName(@"DiaLetivo").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<AulaHorario>().HasKey(s => new
            {
                s.Horario,
                s.DiaLetivo
            });
        }

        partial void CustomizeAulaHorarioMapping(ModelBuilder modelBuilder);

        private void DiaLetivoMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiaLetivo>().ToTable(@"DiaLetivo");
            modelBuilder.Entity<DiaLetivo>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<DiaLetivo>().Property(x => x.NomeDiaSemana).HasColumnName(@"NomeDiaSemana").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<DiaLetivo>().Property(x => x.Dia).HasColumnName(@"Dia").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<DiaLetivo>().HasKey(@"Id");
        }

        partial void CustomizeDiaLetivoMapping(ModelBuilder modelBuilder);

        private void HorarioMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Horario>().ToTable(@"Horario");
            modelBuilder.Entity<Horario>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Horario>().Property(x => x.TurnoNome).HasColumnName(@"TurnoNome").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Horario>().Property(x => x.Periodo).HasColumnName(@"Periodo").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Horario>().HasKey(@"Id");
        }

        partial void CustomizeHorarioMapping(ModelBuilder modelBuilder);

        private void BlocoMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bloco>().ToTable(@"Bloco");
            modelBuilder.Entity<Bloco>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Bloco>().Property(x => x.Descricao).HasColumnName(@"Descricao").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Bloco>().HasKey(@"Id");
        }

        partial void CustomizeBlocoMapping(ModelBuilder modelBuilder);

        private void SalaMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sala>().ToTable(@"Bloco");
            modelBuilder.Entity<Sala>().Property(x => x.NumeroSala).HasColumnName(@"Numero").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Sala>().Property(x => x.Bloco).HasColumnName(@"Bloco").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Sala>().HasKey(@"Id");
        }

        partial void CustomizeSalaMapping(ModelBuilder modelBuilder);

        private void RelationshipsMapping(ModelBuilder modelBuilder)
        {

            #region Product Navigation properties

            modelBuilder.Entity<Product>().HasMany(x => x.ProductMachines).WithOne(op => op.Product).IsRequired(true).HasForeignKey(@"ProductId");

            #endregion

            #region AulaHorario Navigation properties

            modelBuilder.Entity<AulaHorario>().HasOne(x => x.DiaLetivo).WithOne(op => op).IsRequired(true).HasForeignKey(@"OwnerId");
            modelBuilder.Entity<AulaHorario>().HasOne(x => x.Horario).WithOne(op => op.Machine).IsRequired(true).HasForeignKey(@"MachineId");

            #endregion

            #region ProductMachine Navigation properties

            modelBuilder.Entity<ProductMachine>().HasOne(x => x.Product).WithOne(op => op.ProductMachines).IsRequired(true).HasForeignKey(@"ProductId");
            modelBuilder.Entity<ProductMachine>().HasOne(x => x.Machine).WithMany(op => op.ProductMachines).IsRequired(true).HasForeignKey(@"MachineId");
            modelBuilder.Entity<ProductMachine>().HasMany(x => x.ProductOrders).WithOne(op => op.ProductMachine).IsRequired(true).HasForeignKey(@"ProductMachineId");
            modelBuilder.Entity<ProductMachine>().HasMany(x => x.Supplies).WithOne(op => op.ProductMachine).IsRequired(true).HasForeignKey(@"ProductMachineId");

            #endregion

            #region ProductOrder Navigation properties

            modelBuilder.Entity<ProductOrder>().HasOne(x => x.ProductMachine).WithMany(op => op.ProductOrders).IsRequired(true).HasForeignKey(@"ProductMachineId");
            modelBuilder.Entity<ProductOrder>().HasOne(x => x.Order).WithMany(op => op.ProductOrders).IsRequired(true).HasForeignKey(@"OrderId");

            #endregion

            #region Supply Navigation properties

            modelBuilder.Entity<Supply>().HasOne(x => x.ProductMachine).WithMany(op => op.Supplies).IsRequired(true).HasForeignKey(@"ProductMachineId");

            #endregion

            #region MachineSpot Navigation properties

            modelBuilder.Entity<MachineSpot>().HasOne(x => x.Machine).WithMany(op => op.MachineSpots).IsRequired(true).HasForeignKey(@"MachineId");

            #endregion

            #region Owner Navigation properties

            modelBuilder.Entity<Owner>().HasMany(x => x.Machines).WithOne(op => op.Owner).IsRequired(true).HasForeignKey(@"OwnerId");

            #endregion

            #region MachineMaintenance Navigation properties

            modelBuilder.Entity<MachineMaintenance>().HasOne(x => x.Machine).WithMany(op => op.Maintenances).IsRequired(true).HasForeignKey(@"MachineId");
            modelBuilder.Entity<MachineMaintenance>().HasOne(x => x.Maintenance).WithMany(op => op.MachineMaintenances).IsRequired(true).HasForeignKey(@"MaintenanceId");

            #endregion

            #region Order Navigation properties

            modelBuilder.Entity<Order>().HasMany(x => x.ProductOrders).WithOne(op => op.Order).IsRequired(true).HasForeignKey(@"OrderId");

            #endregion

            #region Maintenance Navigation properties

            modelBuilder.Entity<Maintenance>().HasMany(x => x.MachineMaintenances).WithOne(op => op.Maintenance).IsRequired(true).HasForeignKey(@"MaintenanceId");

            #endregion
        }
    }

}
