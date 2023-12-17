using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Models;

public partial class EcommerceContext : DbContext
{
    public EcommerceContext()
    {
    }

    public EcommerceContext(DbContextOptions<EcommerceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<PurchaseHistory> PurchaseHistories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-83N888M\\SQLEXPRESS;Database=ecommerce;Integrated Security=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__account__F267253E18E75309");

            entity.ToTable("account", tb => tb.HasTrigger("tgr_accountcreatestamp"));

            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.AccountEmail)
                .HasMaxLength(200)
                .HasColumnName("accountEmail");
            entity.Property(e => e.AccountLastLogin)
                .HasColumnType("datetime")
                .HasColumnName("accountLastLogin");
            entity.Property(e => e.AccountName)
                .HasMaxLength(200)
                .HasColumnName("accountName");
            entity.Property(e => e.AccountPassword)
                .HasMaxLength(100)
                .HasColumnName("accountPassword");
            entity.Property(e => e.AccountRegisterDate)
                .HasColumnType("datetime")
                .HasColumnName("accountRegisterDate");
            entity.Property(e => e.AccountRoleId).HasColumnName("accountRoleID");
            entity.Property(e => e.IsActive).HasColumnName("isActive");

            entity.HasOne(d => d.AccountRole).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.AccountRoleId)
                .HasConstraintName("fk_account2");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__blog__FA0AA70D82276C04");

            entity.ToTable("blog", tb =>
                {
                    tb.HasTrigger("tgr_createstamp");
                    tb.HasTrigger("tgr_modstamp");
                });

            entity.Property(e => e.BlogId).HasColumnName("blogID");
            entity.Property(e => e.BlogAuthor)
                .HasMaxLength(200)
                .HasColumnName("blogAuthor");
            entity.Property(e => e.BlogContent).HasColumnName("blogContent");
            entity.Property(e => e.BlogCreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("blogCreatedDate");
            entity.Property(e => e.BlogImage)
                .HasMaxLength(1000)
                .HasColumnName("blogImage");
            entity.Property(e => e.BlogModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("blogModifiedDate");
            entity.Property(e => e.BlogSlug).HasColumnName("blogSlug");
            entity.Property(e => e.BlogSummary)
                .HasMaxLength(1000)
                .HasColumnName("blogSummary");
            entity.Property(e => e.BlogTitle)
                .HasMaxLength(200)
                .HasColumnName("blogTitle");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__brand__06B772B9A01C9B7F");

            entity.ToTable("brand");

            entity.Property(e => e.BrandId).HasColumnName("brandID");
            entity.Property(e => e.BrandImage)
                .HasMaxLength(1000)
                .HasColumnName("brandImage");
            entity.Property(e => e.BrandName)
                .HasMaxLength(200)
                .HasColumnName("brandName");
            entity.Property(e => e.BrandOrigin)
                .HasMaxLength(1000)
                .HasColumnName("brandOrigin");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__category__23CAF1F800946818");

            entity.ToTable("category", tb =>
                {
                    tb.HasTrigger("tgr_categorycreatestamp");
                    tb.HasTrigger("tgr_categorymodstamp");
                });

            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.CategoryCreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("categoryCreatedDate");
            entity.Property(e => e.CategoryDescription)
                .HasMaxLength(1000)
                .HasColumnName("categoryDescription");
            entity.Property(e => e.CategoryModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("categoryModifiedDate");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(200)
                .HasColumnName("categoryName");
            entity.Property(e => e.CategorySlug).HasColumnName("categorySlug");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.ProductCount).HasColumnName("productCount");
            entity.Property(e => e.SubCategoryCount).HasColumnName("subCategoryCount");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__customer__B611CB9D229AD61D");

            entity.ToTable("customer", tb => tb.HasTrigger("tgr_customercreatestamp"));

            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.CustomerAddress)
                .HasMaxLength(500)
                .HasColumnName("customerAddress");
            entity.Property(e => e.CustomerAvatar)
                .HasMaxLength(1000)
                .HasColumnName("customerAvatar");
            entity.Property(e => e.CustomerBank)
                .HasMaxLength(200)
                .HasColumnName("customerBank");
            entity.Property(e => e.CustomerBankAccount)
                .HasMaxLength(200)
                .HasColumnName("customerBankAccount");
            entity.Property(e => e.CustomerEmail)
                .HasMaxLength(200)
                .HasColumnName("customerEmail");
            entity.Property(e => e.CustomerJoinDate)
                .HasColumnType("datetime")
                .HasColumnName("customerJoinDate");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(200)
                .HasColumnName("customerName");
            entity.Property(e => e.CustomerOrderQuantity)
                .HasDefaultValueSql("((0))")
                .HasColumnName("customerOrderQuantity");
            entity.Property(e => e.CustomerPhone)
                .HasMaxLength(20)
                .HasColumnName("customerPhone");
            entity.Property(e => e.IsActive).HasColumnName("isActive");

            entity.HasOne(d => d.Account).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("fk_customer_1");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("PK__discount__D2130A06A5FCF691");

            entity.ToTable("discount", tb =>
                {
                    tb.HasTrigger("tgr_discountcreatestamp");
                    tb.HasTrigger("tgr_discountmodstamp");
                });

            entity.Property(e => e.DiscountId).HasColumnName("discountID");
            entity.Property(e => e.DiscountCode)
                .HasMaxLength(200)
                .HasColumnName("discountCode");
            entity.Property(e => e.DiscountCreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("discountCreatedDate");
            entity.Property(e => e.DiscountDescription).HasColumnName("discountDescription");
            entity.Property(e => e.DiscountEndDate)
                .HasColumnType("datetime")
                .HasColumnName("discountEndDate");
            entity.Property(e => e.DiscountModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("discountModifiedDate");
            entity.Property(e => e.DiscountName)
                .HasMaxLength(200)
                .HasColumnName("discountName");
            entity.Property(e => e.DiscountQuantity).HasColumnName("discountQuantity");
            entity.Property(e => e.DiscountStartDate)
                .HasColumnType("datetime")
                .HasColumnName("discountStartDate");
            entity.Property(e => e.DiscountType)
                .HasMaxLength(200)
                .HasColumnName("discountType");
            entity.Property(e => e.DiscountUsed).HasColumnName("discountUsed");
            entity.Property(e => e.DiscountValue).HasColumnName("discountValue");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__orders__0809337D2BDCAD75");

            entity.ToTable("orders", tb => tb.HasTrigger("tgr_orderscreatestamp"));

            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.DiscountId).HasColumnName("discountID");
            entity.Property(e => e.DiscountPrice).HasColumnName("discountPrice");
            entity.Property(e => e.GrandPrice).HasColumnName("grandPrice");
            entity.Property(e => e.OrderAddress)
                .HasMaxLength(1000)
                .HasColumnName("orderAddress");
            entity.Property(e => e.OrderCompleteDate)
                .HasColumnType("datetime")
                .HasColumnName("orderCompleteDate");
            entity.Property(e => e.OrderCreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("orderCreatedDate");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(100)
                .HasDefaultValueSql("('pending')")
                .HasColumnName("orderStatus");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(200)
                .HasColumnName("paymentMethod");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("('unpaid')")
                .HasColumnName("paymentStatus");
            entity.Property(e => e.ShippingFee).HasColumnName("shippingFee");
            entity.Property(e => e.TotalPrice).HasColumnName("totalPrice");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("fk_orders_1");

            entity.HasOne(d => d.Discount).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("fk_orders_2");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__order_de__E4FEDE2ADDF2A0FA");

            entity.ToTable("order_detail");

            entity.Property(e => e.OrderDetailId).HasColumnName("orderDetailID");
            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.ProductName)
                .HasMaxLength(200)
                .HasColumnName("productName");
            entity.Property(e => e.ProductPrice).HasColumnName("productPrice");
            entity.Property(e => e.ProductQuantity).HasColumnName("productQuantity");
            entity.Property(e => e.ProductTotalPrice).HasColumnName("productTotalPrice");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_order_detail_1");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_order_detail_2");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__product__2D10D14AE84A46B5");

            entity.ToTable("product", tb =>
                {
                    tb.HasTrigger("tgr_productcreatestamp");
                    tb.HasTrigger("tgr_productmodstamp");
                });

            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.IsFlashSale).HasColumnName("isFlashSale");
            entity.Property(e => e.ProductBarcode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("productBarcode");
            entity.Property(e => e.ProductBrandId).HasColumnName("productBrandID");
            entity.Property(e => e.ProductCategoryId).HasColumnName("productCategoryID");
            entity.Property(e => e.ProductCreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("productCreatedDate");
            entity.Property(e => e.ProductDiscountPrice).HasColumnName("productDiscountPrice");
            entity.Property(e => e.ProductImage)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("productImage");
            entity.Property(e => e.ProductInStock).HasColumnName("productInStock");
            entity.Property(e => e.ProductInfo)
                .HasMaxLength(5000)
                .IsUnicode(true)
                .HasColumnName("productInfo");
            entity.Property(e => e.ProductModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("productModifiedDate");
            entity.Property(e => e.ProductName)
                .HasMaxLength(200)
                .HasColumnName("productName");
            entity.Property(e => e.ProductOriginalPrice).HasColumnName("productOriginalPrice");
            entity.Property(e => e.ProductSlug).HasColumnName("productSlug");
            entity.Property(e => e.ProductSoldQuantity).HasColumnName("productSoldQuantity");
            entity.Property(e => e.ProductSubCategoryId).HasColumnName("productSubCategoryID");

            entity.HasOne(d => d.ProductBrand).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductBrandId)
                .HasConstraintName("fk_product_2");

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCategoryId)
                .HasConstraintName("fk_product_3");

            entity.HasOne(d => d.ProductSubCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductSubCategoryId)
                .HasConstraintName("fk_product_1");
        });

        modelBuilder.Entity<PurchaseHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__purchase__19BDBDB35D9EDD66");

            entity.ToTable("purchase_history");

            entity.Property(e => e.HistoryId).HasColumnName("historyID");
            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(200)
                .HasColumnName("paymentMethod");
            entity.Property(e => e.PurchaseCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("purchaseCreatedDate");
            entity.Property(e => e.TotalPrice).HasColumnName("totalPrice");

            entity.HasOne(d => d.Customer).WithMany(p => p.PurchaseHistories)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("fk_purchase_1");

            entity.HasOne(d => d.Order).WithMany(p => p.PurchaseHistories)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_purchase_2");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__role__CD98460A8162768F");

            entity.ToTable("role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("roleID");
            entity.Property(e => e.RoleDescription)
                .HasMaxLength(500)
                .HasColumnName("roleDescription");
            entity.Property(e => e.RoleName)
                .HasMaxLength(200)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.SubCategoryId).HasName("PK__subCateg__F8206449E0C4D57F");

            entity.ToTable("subCategory");

            entity.Property(e => e.SubCategoryId).HasColumnName("subCategoryID");
            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.ProductCount).HasColumnName("productCount");
            entity.Property(e => e.SubCategoryCreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("subCategoryCreatedDate");
            entity.Property(e => e.SubCategoryDescription)
                .HasMaxLength(1000)
                .HasColumnName("subCategoryDescription");
            entity.Property(e => e.SubCategoryModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("subCategoryModifiedDate");
            entity.Property(e => e.SubCategoryName)
                .HasMaxLength(200)
                .HasColumnName("subCategoryName");
            entity.Property(e => e.SubCategorySlug).HasColumnName("subCategorySlug");

            entity.HasOne(d => d.Category).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("fk_sub_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
