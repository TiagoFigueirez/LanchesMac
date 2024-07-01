using LanchesMac.Models;

namespace LanchesMac.ViewModel
{
    public class PedidoLancheViewModel
    {
        public Pedido Pedido { get; set; }
        public IEnumerable<PedidoDetalhe> Detalhe { get; set;}
    }
}
