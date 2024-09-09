using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class OrderDetails
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int orderDetailsId { get; set; }
    public int quantity { get; set; }
    public float price { get; set; }
    public bool isDelete { get; set; }
    
    public int orderId { get; set; }
    public int productId { get; set; }
    public virtual Orders orders { get; set; }
    public virtual Products products { get; set; }
}