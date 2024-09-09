using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class Warehouse
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int warehouseId { get; set; }
    public string name { get; set; }
    public string location { get; set; }
    public bool isDelete { get; set; }
    
    public virtual List<Inventory> inventories { get; set; }
}