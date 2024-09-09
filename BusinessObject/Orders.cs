using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enums;

namespace BusinessObject;

public class Orders
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int orderId { get; set; }
    public DateTime orderDate { get; set; }
    public OrdersStatus orderStatus { get; set; }
    public float totalAmount { get; set; }
    public bool isDelete { get; set; }
    
    public int userId { get; set; }
    public int deliveryId { get; set; }
    public int paymentId { get; set; }
    public virtual List<Returns> returns { get; set; }
    public virtual List<OrderDetails> orderDetails { get; set; }
    public virtual User user { get; set; }
    public virtual Delivery delivery { get; set; }
    public virtual Payments payments { get; set; }
}