using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalog.Models;

[Table("Products")]
public class ProductModel
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [StringLength(80)]
    public string? Name { get; set; }

    [Required]
    [StringLength(300)]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }

    public float Stock {  get; set; }
    public DateTime DateRegister { get; set; }

    public int CategoryId { get; set; }

    [JsonIgnore] //ignora a propriedade na serialização, não mostra as categorias nos metodos do produto    
    public CategoryModel? Category { get; set; }  

}
