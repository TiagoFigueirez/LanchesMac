using LanchesMac.Models;

namespace LanchesMac.Repository.Interfaces
{
    public interface IPedidosRepository
    {
        void CriarPedido(Pedido pedido);
    }
}
