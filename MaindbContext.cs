using Infrastructure_Layer.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;

namespace Data_Layer;

public partial class MaindbContext : DbContext
{
    public MaindbContext()
    {
    }

    public MaindbContext(DbContextOptions<MaindbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categorys { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<MeasuresOfScale> MeasuresOfScales { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderdProduct> OrderdProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("name=MainDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PRIMARY");

            entity.ToTable("brands");

            entity.Property(e => e.BrandId)
                .ValueGeneratedNever()
                .HasColumnName("Brand_ID");
            entity.Property(e => e.BrandName)
                .HasMaxLength(45)
                .HasColumnName("Brand_Name");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => new { e.CustomerId, e.ProductId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("cart");

            entity.HasIndex(e => e.CustomerId, "fk_Cart_Customers1_idx");

            entity.HasIndex(e => e.ProductId, "fk_Cart_Products1_idx");

            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.Qty).HasColumnName("QTY");

            entity.HasOne(d => d.Customer).WithMany(p => p.Carts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Cart_Customers1");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Cart_Products1");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("categorys");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("Category_ID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(45)
                .HasColumnName("Category_Name");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PRIMARY");

            entity.ToTable("customers");

            entity.HasIndex(e => e.Email, "Email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Phone, "Phone_UNIQUE").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.Area).HasMaxLength(45);
            entity.Property(e => e.Cookie).HasColumnType("text");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("Create_Date");
            entity.Property(e => e.CreditDebitCard)
                .HasMaxLength(45)
                .HasColumnName("Credit/Debit_Card");
            entity.Property(e => e.Email).HasMaxLength(45);
            entity.Property(e => e.FirstName)
                .HasMaxLength(45)
                .HasColumnName("First_Name");
            entity.Property(e => e.House).HasMaxLength(45);
            entity.Property(e => e.LastName)
                .HasMaxLength(45)
                .HasColumnName("Last_Name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(45)
                .HasColumnName("Middle_Name");
            entity.Property(e => e.Password).HasMaxLength(45);
            entity.Property(e => e.Phone).HasMaxLength(45);
            entity.Property(e => e.Streat).HasMaxLength(45);
        });

        modelBuilder.Entity<MeasuresOfScale>(entity =>
        {
            entity.HasKey(e => e.MeasureOfScaleId).HasName("PRIMARY");

            entity.ToTable("measures_of_scale");

            entity.HasIndex(e => e.MeasureOfScale, "Measure_Of_Scale_UNIQUE").IsUnique();

            entity.Property(e => e.MeasureOfScaleId)
                .ValueGeneratedNever()
                .HasColumnName("Measure_Of_Scale_ID");
            entity.Property(e => e.MeasureOfScale)
                .HasMaxLength(45)
                .HasColumnName("Measure_Of_Scale");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("orders");

            entity.HasIndex(e => e.CustomerId, "fk_Orders_Customers1_idx");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("Order_ID");
            entity.Property(e => e.Area).HasMaxLength(45);
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.House).HasMaxLength(45);
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("Order_Date");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(45)
                .HasColumnName("Order_Status");
            entity.Property(e => e.Streat).HasMaxLength(45);

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Orders_Customers1");
        });

        modelBuilder.Entity<OrderdProduct>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("orderd_products");

            entity.HasIndex(e => e.OrderId, "fk_Orderd_Products_Orders1_idx");

            entity.HasIndex(e => e.ProductId, "fk_Orderd_Products_Products1_idx");

            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.Qty).HasColumnName("QTY");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderdProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Orderd_Products_Orders1");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderdProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Orderd_Products_Products1");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRIMARY");

            entity.ToTable("products");

            entity.HasIndex(e => e.BrandId, "fk_Products_Brands1_idx");

            entity.HasIndex(e => e.CategoryId, "fk_Products_Category1_idx");

            entity.HasIndex(e => e.MeasureOfScaleId, "fk_Products_table11_idx");

            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.BrandId).HasColumnName("Brand_ID");
            entity.Property(e => e.CategoryId).HasColumnName("Category_ID");
            entity.Property(e => e.Description).HasColumnType("tinytext");
            entity.Property(e => e.Image1).HasMaxLength(45);
            entity.Property(e => e.Image2).HasMaxLength(45);
            entity.Property(e => e.Image3).HasMaxLength(45);
            entity.Property(e => e.Image4).HasMaxLength(45);
            entity.Property(e => e.InstockQty).HasColumnName("Instock_QTY");
            entity.Property(e => e.MeasureOfScaleId).HasColumnName("Measure_Of_Scale_ID");
            entity.Property(e => e.NewOrUsed).HasColumnName("New_OR_Used");
            entity.Property(e => e.ProductName)
                .HasMaxLength(45)
                .HasColumnName("Product_Name");
            entity.Property(e => e.SignupDate)
                .HasColumnType("datetime")
                .HasColumnName("Signup_Date");
            entity.Property(e => e.Size).HasMaxLength(45);

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Products_Brands1");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Products_Category1");

            entity.HasOne(d => d.MeasureOfScale).WithMany(p => p.Products)
                .HasForeignKey(d => d.MeasureOfScaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Products_table11");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
