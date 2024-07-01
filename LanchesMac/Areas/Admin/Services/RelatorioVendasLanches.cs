using LanchesMac.Context;
using LanchesMac.Models;
using Microsoft.EntityFrameworkCore;

namespace LanchesMac.Areas.Admin.Services
{
    public class RelatorioVendasLanches
    {
        private readonly AppDbContext _context;

        public RelatorioVendasLanches(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lanche>> GetLancheReport()
        {
            var lanches = await _context.Lanches.ToListAsync();

            if (lanches is null)
            {
                return default(IEnumerable<Lanche>);

            }
            return lanches;
        }
        public async Task<IEnumerable<Categoria>> GetCategoriaReport()
        {
            var categorias = await _context.Categorias.ToListAsync();

            if (categorias is null)
            {
                return default(IEnumerable<Categoria>);

            }
            return categorias;
        }
    }
}
