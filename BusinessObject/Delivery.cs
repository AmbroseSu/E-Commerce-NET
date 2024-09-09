using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enums;

namespace BusinessObject;

public class Delivery
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int deliveryId { get; set; }
    public DateTime deliveryDate { get; set; }
    public string deliveryAddress { get; set; }
    public DeliveryStatus deliveryStatus { get; set; }
    public string currentLocation { get; set; }
    public string estimatedDeliveryTime { get; set; }
    public bool isDelete { get; set; }
    
    public int orderId { get; set; }
    public int userId { get; set; }
    public virtual Orders orders { get; set; }
    public virtual User shipper { get; set; }
}