using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class Inventory
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int inventoryId { get; set; }
    public DateTime manufactureDate { get; set; }
    public DateTime expiryDate { get; set; }
    public bool isDelete { get; set; }
    
    public int productId { get; set; }
    public int warehouseId { get; set; }
    public virtual Products products { get; set; }
    public virtual Warehouse warehouse { get; set; }
}