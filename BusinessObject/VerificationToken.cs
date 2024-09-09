using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class VerificationToken
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string Token { get; set; }
    
    public DateTime ExpirationTime { get; set; }

    private const int EXPIRATION_TIME_MINUTES = 1;

    // Navigation property for related User entity
    public int userId { get; set; }
    public virtual User user { get; set; }

    public VerificationToken()
    {
        // Default constructor
    }

    public VerificationToken(string token, User user)
    {
        this.Token = token;
        this.user = user;
        this.ExpirationTime = GetTokenExpirationTime();
    }

    public VerificationToken(string token)
    {
        this.Token = token;
        this.ExpirationTime = GetTokenExpirationTime();
    }

    private DateTime GetTokenExpirationTime()
    {
        return DateTime.Now.AddMinutes(EXPIRATION_TIME_MINUTES);
    }
}