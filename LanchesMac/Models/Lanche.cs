using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanchesMac.Models
{
    public class Lanche
    {
        public int LancheId { get; set; }

        [Required(ErrorMessage ="Informe o nome do lanche")]
        [Display(Name ="Nome do Lanche")]
        [StringLength(80, MinimumLength =10, ErrorMessage ="O {0} deve ter no minimo {1} e no máximo {2}")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição do lanche deve ser informada")]
        [Display(Name = "Descrição do Lanche")]
        [MinLength(10,ErrorMessage = "A descrição curta deve ter no minimo {1} caracteres")]
        [MaxLength(200, ErrorMessage = "A descrição curta deve ter no Máximo {1} caracteres")]
        public string DescricaoCurta { get; set; }

        [Required(ErrorMessage = "A descrição do lanche deve ser informada")]
        [Display(Name = "Descrição do Lanche")]
        [MinLength(10, ErrorMessage = "A descrição detalhada deve ter no minimo {1} caracteres")]
        [MaxLength(200, ErrorMessage = "A descrição detalhada deve ter no Máximo {1} caracteres")]
        public string DescricaoDetalhada { get; set; }
        [Required(ErrorMessage = "Informe o preço do lanche")]
        [Display(Name ="Preço")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(1, 999.99, ErrorMessage ="O preço deve ser entre 1 e 999,99")]
        public decimal Preco { get; set; }
        [Display(Name ="Caminho da imagem normal")]
        [StringLength(200, ErrorMessage =" O {0} deve ter no maximo {1} caracteres")]
        public string ImagemUrl { get; set; }
        [Display(Name = "Caminho da imagem em miniatura")]
        [StringLength(200, ErrorMessage = " O {0} deve ter no maximo {1} caracteres")]
        public string ImagemThumbnailUrl { get; set; }
        [Display(Name = "Preferido ?")]
        public bool IsLanchePreferido { get; set; }
        [Display(Name ="Estoque ")]
        public bool EmEstoque { get; set; }

        //usado para definir a chave estrangeira de categoria
        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }
    }
}
