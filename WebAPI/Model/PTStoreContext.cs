using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebAPI.Model
{
    public partial class PTStoreContext : DbContext
    {
        public PTStoreContext(DbContextOptions<PTStoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<StatusUpdateOrder> StatusUpdateOrders { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);
                entity.Property(e => e.ImageUrl).IsUnicode(false);
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FeedbackTime).HasColumnType("datetime");

                entity.Property(e => e.ReplyTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderCode).IsUnicode(false);

                entity.Property(e => e.OrderTime).HasColumnType("datetime");

                entity.Property(e => e.PhoneNumber).IsUnicode(false);

                entity.Property(e => e.TotalCost).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.OrderShippers)
                    .HasForeignKey(d => d.ShipperId)
                    .HasConstraintName("FK__Orders__ShipperI__3A81B327");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.OrderUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__Orders__UpdatedB__3B75D760");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OrderUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Orders__UserId__398D8EEE");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.CurrentPrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__OrderDeta__Order__3C69FB99");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__OrderDeta__Produ__3D5E1FD2");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.CurrentPrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ImageUrl).IsUnicode(false);

                entity.Property(e => e.Os)
                    .IsUnicode(false)
                    .HasColumnName("OS");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Ram)
                    .IsUnicode(false)
                    .HasColumnName("RAM");

                entity.Property(e => e.Cpu)
                    .HasColumnName("CPU");

                entity.Property(e => e.Gpu)
                    .HasColumnType("GPU");

                entity.Property(e => e.Gps)
                    .HasColumnName("GPS");

                entity.Property(e => e.Rom)
                    .IsUnicode(false)
                    .HasColumnName("ROM");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__Products__BrandI__36B12243");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.ReviewTime).HasColumnType("datetime");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Reviews__Product__38996AB5");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Reviews__UserId__37A5467C");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StatusUpdateOrder>(entity =>
            {
                entity.Property(e => e.Detail).HasMaxLength(1);

                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.StatusUpdateOrders)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__StatusUpd__Order__3E52440B");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.StatusUpdateOrders)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__StatusUpd__Updat__3F466844");
            });

            modelBuilder.Entity<Subscriber>(entity =>
            {
                entity.Property(e => e.Email).IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.ImageUrl).IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber).IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Users__RoleId__35BCFE0A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
