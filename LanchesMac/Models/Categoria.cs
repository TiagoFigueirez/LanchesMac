using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanchesMac.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }
        [StringLength(100, ErrorMessage = "O tamanho Máximo e cem caracteres")]
        [Required(ErrorMessage ="Informe o nome da categoria")]
        [Display(Name ="Descrição")]
        public string CategoriaNome { get; set; }
        [StringLength(200, ErrorMessage = "O tamanho Máximo e cem caracteres")]
        [Required(ErrorMessage = "Informe o nome a descrição da categoria")]
        [Display(Name = "Nome")]
        public string Descricao { get; set; }

        //como no relacionamento são muitos lanches para uma categoria definimos 
        //uma list para os a categoria 
        public List<Lanche> Lanches { get; set;}
    }
}
