namespace SofCoAr.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SofcoContext : DbContext
    {
        public SofcoContext()
            : base("name=SofcoContext")
        {
            this.Configuration.ProxyCreationEnabled = true;
            this.Configuration.LazyLoadingEnabled = false; 
        }

        public virtual DbSet<BillingMilestoneDetail> BillingMilestoneDetails { get; set; }
        public virtual DbSet<BillingMilestone> BillingMilestones { get; set; }
        public virtual DbSet<CurrencySign> CurrencySigns { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerService> CustomerServices { get; set; }
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<SolFacActionHist> SolFacActionHists { get; set; }
        public virtual DbSet<SolFacActionState> SolFacActionStates { get; set; }
        public virtual DbSet<SolFacHist> SolFacHists { get; set; }
        public virtual DbSet<SolFacState> SolFacStates { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BillingMilestone>()
                .Property(e => e.Monto)
                .HasPrecision(18, 4);

            modelBuilder.Entity<BillingMilestone>()
                .Property(e => e.MontoInic)
                .HasPrecision(18, 4);

            modelBuilder.Entity<BillingMilestone>()
                .HasMany(e => e.BillingMilestoneDetails)
                .WithRequired(e => e.BillingMilestone)
                .HasForeignKey(e => e.IdBillingMilestone);

            modelBuilder.Entity<BillingMilestone>()
                .HasMany(e => e.SolFacActionHists)
                .WithRequired(e => e.BillingMilestone)
                .HasForeignKey(e => e.IdMilestone)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BillingMilestone>()
                .HasMany(e => e.SolFacActionHists1)
                .WithOptional(e => e.BillingMilestoneSource)
                .HasForeignKey(e => e.IdMilestoneSource);

            modelBuilder.Entity<BillingMilestone>()
                .HasMany(e => e.SolFacHists)
                .WithRequired(e => e.BillingMilestone)
                .HasForeignKey(e => e.IdBillingMilestone);

            modelBuilder.Entity<CurrencySign>()
                .HasMany(e => e.BillingMilestoneDetails)
                .WithRequired(e => e.CurrencySign)
                .HasForeignKey(e => e.IdCurrencySign);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.BillingMilestones)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.IdCustomer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.CustomerServices)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.IdCustomer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Projects)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.IdCustomer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerService>()
                .HasMany(e => e.BillingMilestones)
                .WithOptional(e => e.CustomerService)
                .HasForeignKey(e => e.IdService);

            modelBuilder.Entity<CustomerService>()
                .HasMany(e => e.Projects)
                .WithOptional(e => e.CustomerService)
                .HasForeignKey(e => e.IdService);

            modelBuilder.Entity<DocumentType>()
                .HasMany(e => e.BillingMilestones)
                .WithRequired(e => e.DocumentType)
                .HasForeignKey(e => e.IdDocumentType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PaymentMethod>()
                .HasMany(e => e.BillingMilestones)
                .WithRequired(e => e.PaymentMethod)
                .HasForeignKey(e => e.IdPaymentMethod)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Profile>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.Profile)
                .HasForeignKey(e => e.IdProfile);

            modelBuilder.Entity<Profile>()
                .HasMany(e => e.UserProfiles)
                .WithRequired(e => e.Profile)
                .HasForeignKey(e => e.Profile_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                .HasMany(e => e.BillingMilestones)
                .WithRequired(e => e.Project)
                .HasForeignKey(e => e.IdProject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Province>()
                .HasMany(e => e.BillingMilestones)
                .WithOptional(e => e.Province)
                .HasForeignKey(e => e.IdImpProv_1);

            modelBuilder.Entity<Province>()
                .HasMany(e => e.BillingMilestones1)
                .WithOptional(e => e.Province1)
                .HasForeignKey(e => e.IdImpProv_2);

            modelBuilder.Entity<Province>()
                .HasMany(e => e.BillingMilestones2)
                .WithOptional(e => e.Province2)
                .HasForeignKey(e => e.IdImpProv_3);

            modelBuilder.Entity<SolFacActionHist>()
                .Property(e => e.MontoAnte)
                .HasPrecision(18, 4);

            modelBuilder.Entity<SolFacActionHist>()
                .Property(e => e.MontoNuevo)
                .HasPrecision(18, 4);

            modelBuilder.Entity<SolFacActionState>()
                .HasMany(e => e.SolFacActionHists)
                .WithRequired(e => e.SolFacActionState)
                .HasForeignKey(e => e.IdSolFacActionState)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SolFacState>()
                .HasMany(e => e.BillingMilestones)
                .WithOptional(e => e.SolFacState)
                .HasForeignKey(e => e.IdSolFacState);

            modelBuilder.Entity<SolFacState>()
                .HasMany(e => e.SolFacHists)
                .WithRequired(e => e.SolFacState)
                .HasForeignKey(e => e.IdSolFacState)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Status>()
                .HasMany(e => e.CustomerServices)
                .WithRequired(e => e.Status)
                .HasForeignKey(e => e.IdStatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.SolFacActionHists)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IdUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.SolFacHists)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IdUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserProfiles)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);
        }
    }
}
