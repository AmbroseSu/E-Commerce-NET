using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class Chats
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int chatId { get; set; }
    public string chatName { get; set; }
    public DateTime createDate { get; set; }
    public bool isDelete { get; set; }
    
    public virtual List<Message> messages { get; set; }
    public virtual List<UserChat> userChats { get; set; }
}