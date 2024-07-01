using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repository.Interfaces;

namespace LanchesMac.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {

        public AppDbContext _dbContext;

        public CategoriaRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;// temos que gerar o construtor para as consultas
        }

        public IEnumerable<Categoria> Categorias => _dbContext.Categorias;
    }
}
