using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enums;

namespace BusinessObject;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int userId { get; set; }
    public string fullname { get; set; }
    //public string login { get; set; }
    public string address { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string phone { get; set; }
    public string image { get; set; }
    public string status { get; set; }
    public Gender gender { get; set; }
    public Role role { get; set; }
    public string fcm { get; set; }
    public bool isDeleted { get; set; }
    public bool isEnabled { get; set; } = false;
    
    public int cartId { get; set; }
    public virtual List<Delivery> deliveries { get; set; }
    public virtual List<Orders> orders { get; set; }
    public virtual List<Returns> returns { get; set; }
    public virtual List<Message> messages { get; set; }
    public virtual List<UserChat> userChats { get; set; }
    public virtual Cart cart { get; set; }
    
    
}