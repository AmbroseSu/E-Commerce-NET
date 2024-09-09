using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enums;

namespace BusinessObject;

public class Payments
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int paymentId { get; set; }
    public DateTime paymentDate { get; set; }
    public float totalAmount { get; set; }
    public PaymentMethod paymentMethod { get; set; }
    public PaymentStatus paymentStatus { get; set; }
    public bool isDelete { get; set; }
    
    public int orderId { get; set; }
    public virtual Orders orders { get; set; }
}