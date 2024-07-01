using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanchesMac.Models
{
    [Table("CarrinhosCompraItens")]
    public class CarrinhoCompraItem
    {
        public int CarrinhoCompraItemId { get; set; }          
        //como ja criamos a entidade lanche ele vai entender esse item como chave estrangeira
        public int  Quantidade { get; set; }
        [StringLength(200)]
        public string CarrinhoCompraId { get; set; }
        public Lanche Lanche { get; set; }

    }
}
