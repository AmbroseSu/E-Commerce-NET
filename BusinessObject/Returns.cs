using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enums;

namespace BusinessObject;

public class Returns
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int returnId { get; set; }
    public DateTime returnDate { get; set; }
    public int quantity { get; set; }
    public DateTime manufactureDate { get; set; }
    public DateTime expiryDate { get; set; }
    public string reason { get; set; }
    public ReturnStatus returnStatus { get; set; }
    public bool isDelete { get; set; }
    
    
    public int orderId { get; set; }
    public int productId { get; set; }
    public int userId { get; set; }
    public virtual Orders orders { get; set; }
    public virtual Products products { get; set; }
    public virtual User user { get; set; }
}