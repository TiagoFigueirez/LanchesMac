using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LanchesMac.Repository
{
    public class LancheRepository : ILanchesRepository
    {
        public AppDbContext _DbContext;

        public LancheRepository(AppDbContext dbContext)
        {
            _DbContext = dbContext;
        }
        //retorna o lanche e suas categorias
        public IEnumerable<Lanche> Lanches => _DbContext.Lanches.Include(c => c.Categoria);
        //retorna os lanches preferidos e suas categorias
        public IEnumerable<Lanche> LanchesPreferidos => _DbContext.Lanches.Where(c => c.IsLanchePreferido).Include(c=> c.Categoria);
        //retorna o lanche pelo seu id
        public Lanche GetLancheById(int Lancheid) => _DbContext.Lanches.FirstOrDefault( C => C.LancheId == Lancheid);
      
        //Include() permite obter os dados relacionados incluindo-os na consulta, Where define um filtro
    }
}
