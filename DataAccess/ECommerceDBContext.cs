using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

public class ECommerceDBContext : DbContext
{
    public ECommerceDBContext() {  }
    
    public ECommerceDBContext(DbContextOptions<ECommerceDBContext> options) : base(options) { }
    
    public virtual DbSet<Cart> Carts { get; set; }
    public virtual DbSet<CartItems> CartItems { get; set; }
    public virtual DbSet<Chats> Chats { get; set; }
    public virtual DbSet<Delivery> Deliveries { get; set; }
    public virtual DbSet<Inventory> Inventories { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<OrderDetails> OrderDetails { get; set; }
    public virtual DbSet<Orders> Orders { get; set; }
    public virtual DbSet<Payments> Payments { get; set; }
    public virtual DbSet<ProductCategories> ProductCategories { get; set; }
    public virtual DbSet<ProductImages> ProductImages { get; set; }
    public virtual DbSet<Products> Products { get; set; }
    public virtual DbSet<Returns> Returns { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserChat> UserChats { get; set; }
    public virtual DbSet<VerificationToken> VerificationTokens { get; set; }
    public virtual DbSet<Warehouse> Warehouses { get; set; }
    
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var strConn = config.GetConnectionString("DB");

            return strConn;
        }
/*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Chỉ gọi GetConnectionString nếu cần thiết
            var connectionString = GetConnectionString();
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    private string GetConnectionString()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var basePath = AppContext.BaseDirectory;

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        return configuration.GetConnectionString("DB");
    }*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");
            entity.HasKey(e => e.cartId);
            entity.Property(e => e.createAt);
            entity.Property(e => e.updateAt);
            entity.Property(e => e.isDelete);

            entity.HasMany(e => e.cartItems)
                .WithOne(ci => ci.cart)
                .HasForeignKey(ci => ci.cartId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Cart_CartItems");
            entity.HasOne(e => e.user)
                .WithOne(u => u.cart)
                .HasForeignKey<User>(u => u.cartId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Cart_Users");
        });

        modelBuilder.Entity<CartItems>(entity =>
        {
            entity.ToTable("CartItems");
            entity.HasKey(e => e.cartItemId);
            entity.Property(e => e.quantity);
            entity.Property(e => e.addedAt);
            entity.Property(e => e.isDelete);

            entity.HasOne(e => e.cart)
                .WithMany(ci => ci.cartItems)
                .HasForeignKey(ci => ci.cartId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CartItems.Cart");
            entity.HasOne(e => e.products)
                .WithMany(ps => ps.cartItems)
                .HasForeignKey(ps => ps.productId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_CartItems.Products");
        });

        modelBuilder.Entity<Chats>(entity =>
        {
            entity.ToTable("Chats");
            entity.HasKey(e => e.chatId);
            entity.Property(e => e.chatName);
            entity.Property(e => e.createDate);
            entity.Property(e => e.isDelete);

            entity.HasMany(e => e.messages)
                .WithOne(me => me.chats)
                .HasForeignKey(me => me.chatId);
                //.HasConstraintName("FK_Chats_Messages");
            entity.HasMany(e => e.userChats)
                .WithOne(uc => uc.chats)
                .HasForeignKey(uc => uc.chatId);
                //.HasConstraintName("FK_UserChats_UserChats");
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.ToTable("Delivery");
            entity.HasKey(e => e.deliveryId);
            entity.Property(e => e.deliveryDate);
            entity.Property(e => e.deliveryAddress);
            entity.Property(e => e.deliveryStatus);
            entity.Property(e => e.currentLocation);
            entity.Property(e => e.estimatedDeliveryTime);
            entity.Property(e => e.isDelete);
            
            entity.HasOne(e => e.orders)
                .WithOne(or => or.delivery)
                .HasForeignKey<Orders>(d => d.deliveryId);
                //.HasConstraintName("FK_Delivery_Orders");
            entity.HasOne(e => e.shipper)
                .WithMany(u => u.deliveries)
                .HasForeignKey(s => s.userId);
                //.HasConstraintName("FK_Deliveries_Shipper");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.ToTable("Inventory");
            entity.HasKey(e => e.inventoryId);
            entity.Property(e => e.manufactureDate);
            entity.Property(e => e.expiryDate);
            entity.Property(e => e.isDelete);

            entity.HasOne(e => e.products)
                .WithMany(i => i.inventories)
                .HasForeignKey(ps => ps.productId);
                //.HasConstraintName("FK_Inventories_Products");
            entity.HasOne(e => e.warehouse)
                .WithMany(i => i.inventories)
                .HasForeignKey(ps => ps.warehouseId);
                //.HasConstraintName("FK_Inventories_Warehouses");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("Messages");
            entity.HasKey(e => e.messageId);
            entity.Property(e => e.content);
            entity.Property(e => e.sendAt);
            entity.Property(e => e.isDelete);
            entity.Property(e => e.isSent);
            entity.Property(e => e.isDelivered);
            entity.Property(e => e.isRead);

            entity.HasOne(e => e.sender)
                .WithMany(s => s.messages)
                .HasForeignKey(u => u.userId);
                //.HasConstraintName("FK_Messages_Senders");
            entity.HasOne(e => e.chats)
                .WithMany(c => c.messages)
                .HasForeignKey(u => u.chatId);
                //HasConstraintName("FK_Messages_Chats");
        });

        modelBuilder.Entity<OrderDetails>(entity =>
        {
            entity.ToTable("OrderDetails");
            entity.HasKey(e => e.orderDetailsId);
            entity.Property(e => e.quantity);
            entity.Property(e => e.price);
            entity.Property(e => e.isDelete);
            
            entity.HasOne(e => e.orders)
                .WithMany(o => o.orderDetails)
                .HasForeignKey(ps => ps.orderId);
                //.HasConstraintName("FK_OrderDetails_Orders");
            entity.HasOne(e => e.products)
                .WithMany(p => p.orderDetails)
                .HasForeignKey(ps => ps.productId);
                //.HasConstraintName("FK_OrderDetails_Products");
        });

        modelBuilder.Entity<Orders>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(e => e.orderId);
            entity.Property(e => e.orderDate);
            entity.Property(e => e.orderStatus);
            entity.Property(e => e.totalAmount);
            entity.Property(e => e.isDelete);
            
            entity.HasMany(e => e.returns)
                .WithOne(r => r.orders)
                .HasForeignKey(r => r.orderId);
                //.HasConstraintName("FK_Orders_Returns");
            entity.HasMany(e => e.orderDetails)
                .WithOne(o => o.orders)
                .HasForeignKey(o => o.orderId);
                //.HasConstraintName("FK_Orders_OrderDetails");
            entity.HasOne(e => e.user)
                .WithMany(u => u.orders)
                .HasForeignKey(u => u.userId);
                //.HasConstraintName("FK_Orders_Users");
            entity.HasOne(e => e.delivery)
                .WithOne(d => d.orders)
                .HasForeignKey<Delivery>(d => d.orderId);
                //.HasConstraintName("FK_Orders_Delivery");
            entity.HasOne(e => e.payments)
                .WithOne(p => p.orders)
                .HasForeignKey<Payments>(p => p.orderId);
                //.HasConstraintName("FK_Orders_Payments");
        });

        modelBuilder.Entity<Payments>(entity =>
        {
            entity.ToTable("Payments");
            entity.HasKey(e => e.paymentId);
            entity.Property(e => e.paymentDate);
            entity.Property(e => e.totalAmount);
            entity.Property(e => e.paymentMethod);
            entity.Property(e => e.paymentStatus);
            entity.Property(e => e.isDelete);
            
            entity.HasOne(e => e.orders)
                .WithOne(e => e.payments)
                .HasForeignKey<Orders>(e => e.paymentId);
                //.HasConstraintName("FK_Payments_Orders");
        });

        modelBuilder.Entity<ProductCategories>(entity =>
        {
            entity.ToTable("ProductCategories");
            entity.HasKey(e => e.productCategoryId);
            entity.Property(e => e.categoryName);
            entity.Property(e => e.description);
            entity.Property(e => e.isDelete);
            
            entity.HasMany(e => e.products)
                .WithOne(p => p.productCategories)
                .HasForeignKey(pc => pc.productCategoryId);
                //.HasConstraintName("FK_ProductCategories_Products");
        });

        modelBuilder.Entity<ProductImages>(entity =>
        {
            entity.ToTable("ProductImages");
            entity.HasKey(e => e.productImageId);
            entity.Property(e => e.imageUrl);
            entity.Property(e => e.isDelete);
            
            entity.HasOne(e => e.products)
                .WithMany(p => p.productImages)
                .HasForeignKey(ps => ps.productId);
                //.HasConstraintName("FK_ProductImages_Products");
        });

        modelBuilder.Entity<Products>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(e => e.productId);
            entity.Property(e => e.productName);
            entity.Property(e => e.description);
            entity.Property(e => e.price);
            entity.Property(e => e.image);
            entity.Property(e => e.isDelete);
            
            entity.HasMany(e => e.cartItems)
                .WithOne(c => c.products)
                .HasForeignKey(pc => pc.productId);
                //.HasConstraintName("FK_Products_CartItems");
            entity.HasMany(e => e.inventories)
                .WithOne(i => i.products)
                .HasForeignKey(i => i.productId);
                //.HasConstraintName("FK_Products_Inventories");
            entity.HasMany(e => e.orderDetails)
                .WithOne(o => o.products)
                .HasForeignKey(o => o.productId);
                //.HasConstraintName("FK_Products_OrderDetails");
            entity.HasMany(e => e.returns)
                .WithOne(r => r.products)
                .HasForeignKey(r => r.productId);
                //.HasConstraintName("FK_Products_Returns");
            entity.HasMany(e => e.productImages)
                .WithOne(p => p.products)
                .HasForeignKey(pc => pc.productId);
                //.HasConstraintName("FK_Products_ProductImages");
            entity.HasOne(e => e.productCategories)
                .WithMany(p => p.products)
                .HasForeignKey(pc => pc.productCategoryId);
                //.HasConstraintName("FK_Products_ProductCategories");
        });

        modelBuilder.Entity<Returns>(entity =>
        {
            entity.ToTable("Returns");
            entity.HasKey(e => e.returnId);
            entity.Property(e => e.returnDate);
            entity.Property(e => e.quantity);
            entity.Property(e => e.manufactureDate);
            entity.Property(e => e.expiryDate);
            entity.Property(e => e.reason);
            entity.Property(e => e.returnStatus);
            entity.Property(e => e.isDelete);
            
            entity.HasOne(e => e.orders)
                .WithMany(o => o.returns)
                .HasForeignKey(ps => ps.orderId);
                //.HasConstraintName("FK_Returns_Orders");
            entity.HasOne(e => e.products)
                .WithMany(p => p.returns)
                .HasForeignKey(ps => ps.productId);
                //.HasConstraintName("FK_Returns_Products");
            entity.HasOne(e => e.user)
                .WithMany(u => u.returns)
                .HasForeignKey(ps => ps.userId);
                //.HasConstraintName("FK_Returns_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.userId);
            entity.Property(e => e.fullname);
            //entity.Property(e => e.login);
            entity.Property(e => e.address);
            entity.Property(e => e.email);
            entity.Property(e => e.password);
            entity.Property(e => e.phone);
            entity.Property(e => e.image);
            entity.Property(e => e.status);
            entity.Property(e => e.gender);
            entity.Property(e => e.role);
            entity.Property(e => e.fcm);
            entity.Property(e => e.isDeleted);
            entity.Property(e => e.isEnabled);
            
            entity.HasMany(e => e.deliveries)
                .WithOne(d => d.shipper)
                .HasForeignKey(d => d.userId);
                //.HasConstraintName("FK_Shippers_Deliveries");
            entity.HasMany(e => e.orders)
                .WithOne(o => o.user)
                .HasForeignKey(o => o.userId);
                //.HasConstraintName("FK_Users_Orders");
            entity.HasMany(e => e.returns)
                .WithOne(r => r.user)
                .HasForeignKey(r => r.userId);
                //.HasConstraintName("FK_Users_Returns");
            entity.HasMany(e => e.messages)
                .WithOne(m => m.sender)
                .HasForeignKey(m => m.userId);
                //.HasConstraintName("FK_Sender_Messages");
            entity.HasMany(e => e.userChats)
                .WithOne(u => u.user)
                .HasForeignKey(u => u.userId);
                //.HasConstraintName("FK_Users_UserChats");
            entity.HasOne(e => e.cart)
                .WithOne(c => c.user)
                .HasForeignKey<Cart>(c => c.userId);
                //.HasConstraintName("FK_User_Cart");
        });

        modelBuilder.Entity<UserChat>(entity =>
        {
            entity.ToTable("UserChats");
            entity.HasKey(e => new {e.chatId, e.userId});
            
            entity.HasOne(e => e.chats)
                .WithMany(c => c.userChats)
                .HasForeignKey(e => e.chatId);
                //.HasConstraintName("FK_UserChats_Chats");
            entity.HasOne(e => e.user)
                .WithMany(c => c.userChats)
                .HasForeignKey(e => e.userId);
               // .HasConstraintName("FK_UserChats_Users");
        });

        modelBuilder.Entity<VerificationToken>(entity =>
        {
            entity.ToTable("VerificationTokens");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Token);
            entity.Property(e => e.ExpirationTime);
            
            entity.HasOne(vt => vt.user)
                .WithMany()
                .HasForeignKey(vt => vt.userId);
                //.HasConstraintName("FK_VerificationTokens_Users");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.ToTable("Warehouses");
            entity.HasKey(e => e.warehouseId);
            entity.Property(e => e.name);
            entity.Property(e => e.location);
            entity.Property(e => e.isDelete);
            
            entity.HasMany(e => e.inventories)
                .WithOne(i => i.warehouse)
                .HasForeignKey(i => i.warehouseId);
                //.HasConstraintName("FK_Warehouses_Inventories");
            
                
        });
        
    }
    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");
            entity.HasKey(e => e.cartId);
            entity.Property(e => e.createAt);
            entity.Property(e => e.updateAt);
            entity.Property(e => e.isDelete);
        
            entity.HasMany(e => e.cartItems)
                .WithOne(ci => ci.cart)
                .HasForeignKey(ci => ci.cartId)
                .OnDelete(DeleteBehavior.NoAction) // Không xóa CartItems khi Cart bị xóa
                .HasConstraintName("FK_Cart_CartItems");
        
            entity.HasOne(e => e.user)
                .WithOne(u => u.cart)
                .HasForeignKey<User>(u => u.cartId)
                .OnDelete(DeleteBehavior.NoAction) // Không xóa User khi Cart bị xóa
                .HasConstraintName("FK_Cart_Users");
        });
        
        modelBuilder.Entity<CartItems>(entity =>
        {
            entity.ToTable("CartItems");
            entity.HasKey(e => e.cartItemId);
            entity.Property(e => e.quantity);
            entity.Property(e => e.addedAt);
            entity.Property(e => e.isDelete);
        
            entity.HasOne(e => e.cart)
                .WithMany(ci => ci.cartItems)
                .HasForeignKey(ci => ci.cartId)
                .OnDelete(DeleteBehavior.Cascade) // Xóa CartItems khi Cart bị xóa
                .HasConstraintName("FK_CartItems.Cart");
        
            entity.HasOne(e => e.products)
                .WithMany(ps => ps.cartItems)
                .HasForeignKey(ps => ps.productId)
                .OnDelete(DeleteBehavior.NoAction) // Không xóa sản phẩm khi CartItems bị xóa
                .HasConstraintName("FK_CartItems.Products");
        });
        
        modelBuilder.Entity<Chats>(entity =>
        {
            entity.ToTable("Chats");
            entity.HasKey(e => e.chatId);
            entity.Property(e => e.chatName);
            entity.Property(e => e.createDate);
            entity.Property(e => e.isDelete);
        
            entity.HasMany(e => e.messages)
                .WithOne(me => me.chats)
                .HasForeignKey(me => me.chatId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa tin nhắn khi chat bị xóa
                //.HasConstraintName("FK_Chats_Messages");
        
            entity.HasMany(e => e.userChats)
                .WithOne(uc => uc.chats)
                .HasForeignKey(uc => uc.chatId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa UserChats khi Chats bị xóa
                //.HasConstraintName("FK_UserChats_UserChats");
        });
        
        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.ToTable("Delivery");
            entity.HasKey(e => e.deliveryId);
            entity.Property(e => e.deliveryDate).IsRequired();
            entity.Property(e => e.deliveryAddress).IsRequired();
            entity.Property(e => e.deliveryStatus).IsRequired();
            entity.Property(e => e.currentLocation).IsRequired();
            entity.Property(e => e.estimatedDeliveryTime).IsRequired();
            entity.Property(e => e.isDelete).IsRequired();
            
            // Adjust the foreign key constraint to avoid multiple cascade paths
            entity.HasOne(e => e.orders)
                .WithOne(or => or.delivery)
                .HasForeignKey<Delivery>(d => d.orderId)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict or NoAction
            
            entity.HasOne(e => e.shipper)
                .WithMany(u => u.deliveries)
                .HasForeignKey(s => s.userId)
                .OnDelete(DeleteBehavior.Restrict); // Ensure no cascade for shipper
        });


        
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.ToTable("Inventory");
            entity.HasKey(e => e.inventoryId);
            entity.Property(e => e.manufactureDate);
            entity.Property(e => e.expiryDate);
            entity.Property(e => e.isDelete);
        
            entity.HasOne(e => e.products)
                .WithMany(i => i.inventories)
                .HasForeignKey(ps => ps.productId)
                .OnDelete(DeleteBehavior.NoAction); // Không xóa sản phẩm khi Inventory bị xóa
                //.HasConstraintName("FK_Inventories_Products");
        
            entity.HasOne(e => e.warehouse)
                .WithMany(i => i.inventories)
                .HasForeignKey(ps => ps.warehouseId)
                .OnDelete(DeleteBehavior.NoAction); // Không xóa kho hàng khi Inventory bị xóa
                //.HasConstraintName("FK_Inventories_Warehouses");
        });
        
        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("Messages");
            entity.HasKey(e => e.messageId);
            entity.Property(e => e.content);
            entity.Property(e => e.sendAt);
            entity.Property(e => e.isDelete);
            entity.Property(e => e.isSent);
            entity.Property(e => e.isDelivered);
            entity.Property(e => e.isRead);
        
            entity.HasOne(e => e.sender)
                .WithMany(s => s.messages)
                .HasForeignKey(u => u.userId)
                .OnDelete(DeleteBehavior.NoAction); // Không xóa người gửi khi xóa tin nhắn
                //.HasConstraintName("FK_Messages_Senders");
        
            entity.HasOne(e => e.chats)
                .WithMany(c => c.messages)
                .HasForeignKey(u => u.chatId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa tin nhắn khi cuộc trò chuyện bị xóa
                //.HasConstraintName("FK_Messages_Chats");
        });
        
        modelBuilder.Entity<OrderDetails>(entity =>
        {
            entity.ToTable("OrderDetails");
            entity.HasKey(e => e.orderDetailsId);
            entity.Property(e => e.quantity);
            entity.Property(e => e.price);
            entity.Property(e => e.isDelete);
        
            entity.HasOne(e => e.orders)
                .WithMany(o => o.orderDetails)
                .HasForeignKey(ps => ps.orderId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa OrderDetails khi Orders bị xóa
                //.HasConstraintName("FK_OrderDetails_Orders");
        
            entity.HasOne(e => e.products)
                .WithMany(p => p.orderDetails)
                .HasForeignKey(ps => ps.productId)
                .OnDelete(DeleteBehavior.NoAction); // Không xóa sản phẩm khi OrderDetails bị xóa
                //.HasConstraintName("FK_OrderDetails_Products");
        });
        
        modelBuilder.Entity<Orders>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(e => e.orderId);
            entity.Property(e => e.orderDate);
            entity.Property(e => e.orderStatus);
            entity.Property(e => e.totalAmount);
            entity.Property(e => e.isDelete);
        
            entity.HasMany(e => e.returns)
                .WithOne(r => r.orders)
                .HasForeignKey(r => r.orderId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Returns khi Orders bị xóa
                //.HasConstraintName("FK_Orders_Returns");
        
            entity.HasMany(e => e.orderDetails)
                .WithOne(o => o.orders)
                .HasForeignKey(o => o.orderId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa OrderDetails khi Orders bị xóa
                //.HasConstraintName("FK_Orders_OrderDetails");
        
            entity.HasOne(e => e.user)
                .WithMany(u => u.orders)
                .HasForeignKey(u => u.userId)
                .OnDelete(DeleteBehavior.NoAction); // Không xóa User khi Orders bị xóa
                //.HasConstraintName("FK_Orders_Users");
        
            entity.HasOne(e => e.delivery)
                .WithOne(d => d.orders)
                .HasForeignKey<Delivery>(d => d.orderId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Delivery khi Orders bị xóa
                //.HasConstraintName("FK_Orders_Delivery");
        
            entity.HasOne(e => e.payments)
                .WithOne(p => p.orders)
                .HasForeignKey<Payments>(p => p.orderId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Payments khi Orders bị xóa
                //.HasConstraintName("FK_Orders_Payments");
        });
        
        modelBuilder.Entity<Payments>(entity =>
        {
            entity.ToTable("Payments");
            entity.HasKey(e => e.paymentId);
            entity.Property(e => e.paymentDate);
            entity.Property(e => e.totalAmount);
            entity.Property(e => e.paymentMethod);
            entity.Property(e => e.paymentStatus);
            entity.Property(e => e.isDelete);
        
            entity.HasOne(e => e.orders)
                .WithOne(e => e.payments)
                .HasForeignKey<Orders>(e => e.paymentId)
                .OnDelete(DeleteBehavior.NoAction); // Không xóa Orders khi Payments bị xóa
                //.HasConstraintName("FK_Payments_Orders");
        });
        
        modelBuilder.Entity<ProductCategories>(entity =>
        {
            entity.ToTable("ProductCategories");
            entity.HasKey(e => e.productCategoryId);
            entity.Property(e => e.categoryName);
            entity.Property(e => e.description);
            entity.Property(e => e.isDelete);
        
            entity.HasMany(e => e.products)
                .WithOne(p => p.productCategories)
                .HasForeignKey(pc => pc.productCategoryId)
                .OnDelete(DeleteBehavior.NoAction); // Không xóa sản phẩm khi ProductCategory bị xóa
                //.HasConstraintName("FK_ProductCategories_Products");
        });

                modelBuilder.Entity<ProductImages>(entity =>
                {
                    entity.ToTable("ProductImages");
                    entity.HasKey(e => e.productImageId);
                    entity.Property(e => e.imageUrl);
                    entity.Property(e => e.isDelete);
                    
                    entity.HasOne(e => e.products)
                        .WithMany(p => p.productImages)
                        .HasForeignKey(ps => ps.productId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_ProductImages_Products");
                });
        
                modelBuilder.Entity<Products>(entity =>
                {
                    entity.ToTable("Products");
                    entity.HasKey(e => e.productId);
                    entity.Property(e => e.productName);
                    entity.Property(e => e.description);
                    entity.Property(e => e.price);
                    entity.Property(e => e.image);
                    entity.Property(e => e.isDelete);
                    
                    entity.HasMany(e => e.cartItems)
                        .WithOne(c => c.products)
                        .HasForeignKey(pc => pc.productId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_Products_CartItems");
                    entity.HasMany(e => e.inventories)
                        .WithOne(i => i.products)
                        .HasForeignKey(i => i.productId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_Products_Inventories");
                    entity.HasMany(e => e.orderDetails)
                        .WithOne(o => o.products)
                        .HasForeignKey(o => o.productId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_Products_OrderDetails");
                    entity.HasMany(e => e.returns)
                        .WithOne(r => r.products)
                        .HasForeignKey(r => r.productId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_Products_Returns");
                    entity.HasMany(e => e.productImages)
                        .WithOne(p => p.products)
                        .HasForeignKey(pc => pc.productId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_Products_ProductImages");
                    entity.HasOne(e => e.productCategories)
                        .WithMany(p => p.products)
                        .HasForeignKey(pc => pc.productCategoryId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_Products_ProductCategories");
                });
        
                modelBuilder.Entity<Returns>(entity =>
                {
                    entity.ToTable("Returns");
                    entity.HasKey(e => e.returnId);
                    entity.Property(e => e.returnDate);
                    entity.Property(e => e.quantity);
                    entity.Property(e => e.manufactureDate);
                    entity.Property(e => e.expiryDate);
                    entity.Property(e => e.reason);
                    entity.Property(e => e.returnStatus);
                    entity.Property(e => e.isDelete);
                    
                    entity.HasOne(e => e.orders)
                        .WithMany(o => o.returns)
                        .HasForeignKey(ps => ps.orderId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_Returns_Orders");
                    entity.HasOne(e => e.products)
                        .WithMany(p => p.returns)
                        .HasForeignKey(ps => ps.productId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_Returns_Products");
                    entity.HasOne(e => e.user)
                        .WithMany(u => u.returns)
                        .HasForeignKey(ps => ps.userId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_Returns_Users");
                });
        
                modelBuilder.Entity<User>(entity =>
                {
                    entity.ToTable("Users");
                    entity.HasKey(e => e.userId);
                    entity.Property(e => e.fullname);
                    //entity.Property(e => e.login);
                    entity.Property(e => e.address);
                    entity.Property(e => e.email);
                    entity.Property(e => e.password);
                    entity.Property(e => e.phone);
                    entity.Property(e => e.image);
                    entity.Property(e => e.status);
                    entity.Property(e => e.gender);
                    entity.Property(e => e.role);
                    entity.Property(e => e.fcm);
                    entity.Property(e => e.isDeleted);
                    entity.Property(e => e.isEnabled);
                    
                    entity.HasMany(e => e.deliveries)
                        .WithOne(d => d.shipper)
                        .HasForeignKey(d => d.userId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_Shippers_Deliveries");
                    entity.HasMany(e => e.orders)
                        .WithOne(o => o.user)
                        .HasForeignKey(o => o.userId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_Users_Orders");
                    entity.HasMany(e => e.returns)
                        .WithOne(r => r.user)
                        .HasForeignKey(r => r.userId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_Users_Returns");
                    entity.HasMany(e => e.messages)
                        .WithOne(m => m.sender)
                        .HasForeignKey(m => m.userId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_Sender_Messages");
                    entity.HasMany(e => e.userChats)
                        .WithOne(u => u.user)
                        .HasForeignKey(u => u.userId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_Users_UserChats");
                    entity.HasOne(e => e.cart)
                        .WithOne(c => c.user)
                        .HasForeignKey<Cart>(c => c.userId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_User_Cart");
                });
        
                modelBuilder.Entity<UserChat>(entity =>
                {
                    entity.ToTable("UserChats");
                    entity.HasKey(e => new {e.chatId, e.userId});
                    
                    entity.HasOne(e => e.chats)
                        .WithMany(c => c.userChats)
                        .HasForeignKey(e => e.chatId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_UserChats_Chats");
                    entity.HasOne(e => e.user)
                        .WithMany(c => c.userChats)
                        .HasForeignKey(e => e.userId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_UserChats_Users");
                });
        
                modelBuilder.Entity<VerificationToken>(entity =>
                {
                    entity.ToTable("VerificationTokens");
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Token);
                    entity.Property(e => e.ExpirationTime);
                    
                    entity.HasOne(vt => vt.user)
                        .WithMany()
                        .HasForeignKey(vt => vt.userId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_VerificationTokens_Users");
                });
        
                modelBuilder.Entity<Warehouse>(entity =>
                {
                    entity.ToTable("Warehouses");
                    entity.HasKey(e => e.warehouseId);
                    entity.Property(e => e.name);
                    entity.Property(e => e.location);
                    entity.Property(e => e.isDelete);
                    
                    entity.HasMany(e => e.inventories)
                        .WithOne(i => i.warehouse)
                        .HasForeignKey(i => i.warehouseId)
                        .OnDelete(DeleteBehavior.Cascade); // Change to NoAction if needed
                        //.HasConstraintName("FK_Warehouses_Inventories");
                });

        


    }*/

    
}