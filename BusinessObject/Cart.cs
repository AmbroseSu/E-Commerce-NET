using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class Cart
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int cartId { get; set; }
    public DateTime createAt { get; set; }
    public DateTime updateAt { get; set; }
    public bool isDelete { get; set; }
    
    public int userId { get; set; }
    public virtual List<CartItems> cartItems { get; set; }
    public virtual User user { get; set; }
    
}