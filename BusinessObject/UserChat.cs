using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class UserChat
{
    [Key, Column(Order = 0)]
    public int chatId { get; set; }
    [Key, Column(Order = 1)]
    public int userId { get; set; }
    
    [ForeignKey(("chatId"))]
    public Chats chats { get; set; }
    [ForeignKey(("userId"))]
    public User user { get; set; }
}