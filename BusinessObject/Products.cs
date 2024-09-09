using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class Products
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int productId { get; set; }
    public string productName { get; set; }
    public string description { get; set; }
    public float price { get; set; }
    public string image { get; set; }
    public bool isDelete { get; set; }
    
    public int productCategoryId { get; set; }
    public virtual List<CartItems> cartItems { get; set; }
    public virtual List<Inventory> inventories { get; set; }
    public virtual List<OrderDetails> orderDetails { get; set; }
    public virtual List<Returns> returns { get; set; }
    public virtual List<ProductImages> productImages { get; set; }
    public virtual ProductCategories productCategories { get; set; }
    
    
}