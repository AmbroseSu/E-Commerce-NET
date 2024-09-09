using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class Message
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int messageId { get; set; }
    public string content { get; set; }
    public DateTime sendAt { get; set; }
    public bool isDelete { get; set; }
    public bool isSent { get; set; }
    public bool isDelivered { get; set; }
    public bool isRead { get; set; }
    
    public int userId { get; set; }
    public int chatId { get; set; }
    public virtual User sender { get; set; }
    public virtual Chats chats { get; set; }
}