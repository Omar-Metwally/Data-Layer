using Infrastructure_Layer.Models;
using Microsoft.EntityFrameworkCore;

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

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Governorate> Governorates { get; set; }

    public virtual DbSet<MeasuresOfScale> MeasuresOfScales { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderdProduct> OrderdProducts { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Street> Streets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("name=MainDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PRIMARY");

            entity
                .ToTable("brands")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.BrandId)
                .ValueGeneratedNever()
                .HasColumnName("Brand_ID");
            entity.Property(e => e.BrandName)
                .HasMaxLength(45)
                .HasColumnName("Brand_Name");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.CustomerId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("cart")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.CustomerId, "fk_Cart_Customers1_idx");

            entity.HasIndex(e => e.ProductId, "fk_Cart_Products1_idx");

            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
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

            entity
                .ToTable("categorys")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

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

            entity
                .ToTable("customers")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.Email, "Email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Phone, "Phone_UNIQUE").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.Cookie).HasColumnType("text");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("Create_Date");
            entity.Property(e => e.Email).HasMaxLength(45);
            entity.Property(e => e.FirstName)
                .HasMaxLength(45)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(45)
                .HasColumnName("Last_Name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(45)
                .HasColumnName("Middle_Name");
            entity.Property(e => e.Password).HasMaxLength(45);
            entity.Property(e => e.Phone).HasMaxLength(45);
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.DistrictId).HasName("PRIMARY");

            entity.ToTable("districts");

            entity.HasIndex(e => e.GovernorateId, "fk_Districts_Governorates1_idx");

            entity.Property(e => e.DistrictId)
                .ValueGeneratedNever()
                .HasColumnName("DistrictID");
            entity.Property(e => e.DistrictName).HasMaxLength(45);
            entity.Property(e => e.GovernorateId).HasColumnName("GovernorateID");

            entity.HasOne(d => d.Governorate).WithMany(p => p.Districts)
                .HasForeignKey(d => d.GovernorateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Districts_Governorates1");
        });

        modelBuilder.Entity<Governorate>(entity =>
        {
            entity.HasKey(e => e.GovernorateId).HasName("PRIMARY");

            entity.ToTable("governorates");

            entity.Property(e => e.GovernorateId)
                .ValueGeneratedNever()
                .HasColumnName("GovernorateID");
            entity.Property(e => e.GovernorateName).HasMaxLength(45);
        });

        modelBuilder.Entity<MeasuresOfScale>(entity =>
        {
            entity.HasKey(e => e.MeasureOfScaleId).HasName("PRIMARY");

            entity
                .ToTable("measures_of_scale")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

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

            entity
                .ToTable("orders")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.CustomerId, "fk_Orders_Customers1_idx");

            entity.HasIndex(e => e.StreetId, "fk_orders_streats1_idx");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("Order_ID");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("Order_Date");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(45)
                .HasColumnName("Order_Status");
            entity.Property(e => e.PayedByVisa).HasColumnName("Payed_By_Visa");
            entity.Property(e => e.StreetId).HasColumnName("StreetID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Orders_Customers1");

            entity.HasOne(d => d.Street).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StreetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orders_streats1");
        });

        modelBuilder.Entity<OrderdProduct>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("orderd_products")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

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

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity
                .ToTable("payments")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("Order_ID");
            entity.Property(e => e.CardName)
                .HasMaxLength(45)
                .HasColumnName("Card_Name");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(45)
                .HasColumnName("Card_Number");
            entity.Property(e => e.Cvc)
                .HasMaxLength(3)
                .HasColumnName("CVC");
            entity.Property(e => e.ExpireDate)
                .HasMaxLength(45)
                .HasColumnName("Expire_Date");
            entity.Property(e => e.PaymentAmount).HasColumnName("Payment_Amount");

            entity.HasOne(d => d.Order).WithOne(p => p.Payment)
                .HasForeignKey<Payment>(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Payments_Orders1");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRIMARY");

            entity
                .ToTable("products")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.BrandId, "fk_Products_Brands1_idx");

            entity.HasIndex(e => e.CategoryId, "fk_Products_Category1_idx");

            entity.HasIndex(e => e.MeasureOfScaleId, "fk_Products_table11_idx");

            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.BrandId).HasColumnName("Brand_ID");
            entity.Property(e => e.CategoryId).HasColumnName("Category_ID");
            entity.Property(e => e.Description).HasColumnType("mediumtext");
            entity.Property(e => e.Image1).HasMaxLength(200);
            entity.Property(e => e.Image2).HasMaxLength(200);
            entity.Property(e => e.Image3).HasMaxLength(200);
            entity.Property(e => e.Image4).HasMaxLength(200);
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

        modelBuilder.Entity<Street>(entity =>
        {
            entity.HasKey(e => e.StreetId).HasName("PRIMARY");

            entity.ToTable("streets");

            entity.HasIndex(e => e.DistrictId, "fk_streets_Districts1_idx");

            entity.Property(e => e.StreetId)
                .ValueGeneratedNever()
                .HasColumnName("StreetID");
            entity.Property(e => e.DistrictId).HasColumnName("DistrictID");
            entity.Property(e => e.StreetName).HasMaxLength(45);

            entity.HasOne(d => d.District).WithMany(p => p.Streets)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_streets_Districts1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
