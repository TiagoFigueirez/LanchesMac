using LanchesMac.Context;
using LanchesMac.Models;
using Microsoft.EntityFrameworkCore;

namespace LanchesMac.Areas.Admin.Services
{
    public class RelatorioVendasService
    {
        private readonly AppDbContext _context;

        public RelatorioVendasService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pedido>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var resultado = from obj in _context.Pedidos select obj;//consulta IQuerable para se usar a o metodo where

            if (minDate.HasValue)//aqui estamos montando ela na memoria 
            {
                //apos selecionarmos todos os pedidos mantemos apenas aqueles que e maior ou igual a data mina
                resultado = resultado.Where(x => x.PedidoEnviado >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                resultado = resultado.Where(x =>x.PedidoEnviado <=maxDate.Value);
            }
            return await resultado
                         .Include(l => l.PedidoItens)
                         .ThenInclude(l => l.Lanche)
                         .OrderByDescending(x => x.PedidoEnviado).ToListAsync();//aqui obtemos os dados de fato a condulta e feita no Db

        }
    }
}
