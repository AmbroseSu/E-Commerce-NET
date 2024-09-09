using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class CartItems
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int cartItemId { get; set; }
    public int quantity { get; set; }
    public DateTime addedAt { get; set; }
    public bool isDelete { get; set; }
    
    public int cartId { get; set; }
    public int productId { get; set; }
    public virtual Cart cart { get; set; }
    public virtual Products products { get; set; }
}