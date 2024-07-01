using LanchesMac.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace LanchesMac.Context
{
    //IdentityDbContext<IdentityUser> gerencia os usuarios por isso usamos no AppDbContext, e necessario resitrar o serviço
    
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //Construtor define as configurações para  meu contexto do banco de dados
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Lanche> Lanches { get; set;}
        public DbSet<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }
        //DbSet mapeia as classes que vão ser maepadas para as tableas
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalhe> PedidoDetalhes { get; set; }    
    }
}
