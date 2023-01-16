using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Hall_Boking_System.Models
{

    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aboutu> Aboutus { get; set; }
        public virtual DbSet<Acceptance> Acceptances { get; set; }
        public virtual DbSet<Acceptedlist> Acceptedlists { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Contactu> Contactus { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Hall> Halls { get; set; }
        public virtual DbSet<Home> Homes { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Testimonial> Testimonials { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<Visa> Visas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("USER ID=JOR15_User75;PASSWORD=Test321;DATA SOURCE=94.56.229.181:3488/traindb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("JOR15_USER75")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<Aboutu>(entity =>
            {
                entity.ToTable("ABOUTUS");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.HomeId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HOME_ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.PhoneNumber)
                    .HasPrecision(17)
                    .HasColumnName("PHONE_NUMBER");

                entity.HasOne(d => d.Home)
                    .WithMany(p => p.Aboutus)
                    .HasForeignKey(d => d.HomeId)
                    .HasConstraintName("SYS_C00272910");
            });

            modelBuilder.Entity<Acceptance>(entity =>
            {
                entity.ToTable("ACCEPTANCE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.AcceptDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ACCEPT_DATE");

                entity.Property(e => e.AcceptlistId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ACCEPTLIST_ID");

                entity.Property(e => e.CustomersId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CUSTOMERS_ID");

                entity.Property(e => e.HallId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HALL_ID");

                entity.Property(e => e.ReservationId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("RESERVATION_ID");

                entity.HasOne(d => d.Acceptlist)
                    .WithMany(p => p.Acceptances)
                    .HasForeignKey(d => d.AcceptlistId)
                    .HasConstraintName("SYS_C00272881");

                entity.HasOne(d => d.Customers)
                    .WithMany(p => p.Acceptances)
                    .HasForeignKey(d => d.CustomersId)
                    .HasConstraintName("SYS_C00272882");

                entity.HasOne(d => d.Hall)
                    .WithMany(p => p.Acceptances)
                    .HasForeignKey(d => d.HallId)
                    .HasConstraintName("SYS_C00272883");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.Acceptances)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("SYS_C00272884");
            });

            modelBuilder.Entity<Acceptedlist>(entity =>
            {
                entity.ToTable("ACCEPTEDLIST");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.AcceptStatus)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ACCEPT_STATUS");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("ADDRESS");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CITY");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("CATEGORY");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.CategoryDescription)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("CATEGORY_DESCRIPTION");

                entity.Property(e => e.CategoryImage)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("CATEGORY_IMAGE");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CATEGORY_NAME");
            });

            modelBuilder.Entity<Contactu>(entity =>
            {
                entity.ToTable("CONTACTUS");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.HomeId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HOME_ID");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("MESSAGE");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.PhoneNumber)
                    .HasPrecision(17)
                    .HasColumnName("PHONE_NUMBER");

                entity.HasOne(d => d.Home)
                    .WithMany(p => p.Contactus)
                    .HasForeignKey(d => d.HomeId)
                    .HasConstraintName("SYS_C00272903");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("CUSTOMERS");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FIRST_NAME");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LAST_NAME");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.PhoneNumber)
                    .HasPrecision(15)
                    .HasColumnName("PHONE_NUMBER");

                entity.Property(e => e.UserImage)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("USER_IMAGE");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("USER_NAME");
            });

            modelBuilder.Entity<Hall>(entity =>
            {
                entity.ToTable("HALL");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.AddressId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ADDRESS_ID");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CATEGORY_ID");

                entity.Property(e => e.HallDescription)
                    .HasMaxLength(600)
                    .IsUnicode(false)
                    .HasColumnName("HALL_DESCRIPTION");

                entity.Property(e => e.HallImage)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("HALL_IMAGE");

                entity.Property(e => e.HallName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("HALL_NAME");

                entity.Property(e => e.HallPrice)
                    .HasColumnType("NUMBER(20)")
                    .HasColumnName("HALL_PRICE");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Halls)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("SYS_C00272854");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Halls)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("SYS_C00272853");
            });

            modelBuilder.Entity<Home>(entity =>
            {
                entity.ToTable("HOME");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.HomeImage1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("HOME_IMAGE1");

                entity.Property(e => e.HomeImage2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("HOME_IMAGE2");

                entity.Property(e => e.HomeImage3)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("HOME_IMAGE3");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("PAYMENT");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.CustomersId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CUSTOMERS_ID");

                entity.Property(e => e.PaymentAmount)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PAYMENT_AMOUNT");

                entity.Property(e => e.VisaId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("VISA_ID");

                entity.HasOne(d => d.Customers)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.CustomersId)
                    .HasConstraintName("SYS_C00272864");

                entity.HasOne(d => d.Visa)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.VisaId)
                    .HasConstraintName("SYS_C00272865");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("RESERVATIONS");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.CustomersId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CUSTOMERS_ID");

                entity.Property(e => e.DateIn)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_IN");

                entity.Property(e => e.DateOut)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_OUT");

                entity.Property(e => e.HallId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HALL_ID");

                entity.Property(e => e.PaymentId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PAYMENT_ID");

                entity.HasOne(d => d.Customers)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.CustomersId)
                    .HasConstraintName("SYS_C00272871");

                entity.HasOne(d => d.Hall)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.HallId)
                    .HasConstraintName("SYS_C00272870");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("SYS_C00272872");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("REVIEWS");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.CustomersId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CUSTOMERS_ID");

                entity.Property(e => e.HallId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HALL_ID");

                entity.Property(e => e.Opinion)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("OPINION");

                entity.HasOne(d => d.Customers)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.CustomersId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("SYS_C00275982");

                entity.HasOne(d => d.Hall)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.HallId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("SYS_C00275983");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("ROLES");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ROLE_NAME");
            });

            modelBuilder.Entity<Testimonial>(entity =>
            {
                entity.ToTable("TESTIMONIAL");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.AcceptlistId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ACCEPTLIST_ID");

                entity.Property(e => e.CustomersId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CUSTOMERS_ID");

                entity.Property(e => e.HomeId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HOME_ID");

                entity.Property(e => e.TestimonialOpinion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TESTIMONIAL_OPINION");

                entity.HasOne(d => d.Acceptlist)
                    .WithMany(p => p.Testimonials)
                    .HasForeignKey(d => d.AcceptlistId)
                    .HasConstraintName("SYS_C00272895");

                entity.HasOne(d => d.Customers)
                    .WithMany(p => p.Testimonials)
                    .HasForeignKey(d => d.CustomersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C00272894");

                entity.HasOne(d => d.Home)
                    .WithMany(p => p.Testimonials)
                    .HasForeignKey(d => d.HomeId)
                    .HasConstraintName("SYS_C00272896");
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.ToTable("USER_LOGIN");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.CustomersId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CUSTOMERS_ID");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.RoleId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ROLE_ID");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("USER_NAME");

                entity.HasOne(d => d.Customers)
                    .WithMany(p => p.UserLogins)
                    .HasForeignKey(d => d.CustomersId)
                    .HasConstraintName("SYS_C00272840");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserLogins)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("SYS_C00272839");
            });

            modelBuilder.Entity<Visa>(entity =>
            {
                entity.ToTable("VISA");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.CustomersId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CUSTOMERS_ID");

                entity.Property(e => e.VisaAmount)
                    .HasColumnType("NUMBER")
                    .HasColumnName("VISA_AMOUNT");

                entity.Property(e => e.VisaName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("VISA_NAME");

                entity.Property(e => e.VisaNumber)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("VISA_NUMBER");

                entity.HasOne(d => d.Customers)
                    .WithMany(p => p.Visas)
                    .HasForeignKey(d => d.CustomersId)
                    .HasConstraintName("SYS_C00272860");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
