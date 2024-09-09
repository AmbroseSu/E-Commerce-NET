using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class ProductImages
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int productImageId { get; set; }
    public string imageUrl { get; set; }
    public bool isDelete { get; set; }
    
    public int productId { get; set; }
    public virtual Products products { get; set; }
}