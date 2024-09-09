using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class ProductCategories
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int productCategoryId { get; set; }
    public string categoryName { get; set; }
    public string description { get; set; }
    public bool isDelete { get; set; }
    
    public virtual List<Products> products { get; set; }
}