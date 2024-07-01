using LanchesMac.Context;
using LanchesMac.Models;

namespace LanchesMac.Areas.Admin.Services
{
    public class GraficosVendasService
    {
        private readonly AppDbContext _context;

        public GraficosVendasService(AppDbContext context)
        {
            _context = context;
        }

        public List<LancheGrafico> GetVendasLanches(int dias = 360)
        {
            var data = DateTime.Now.AddDays(-dias); //define a data a qual vamos iniciar a seleção dos dados

            var lanches = (from pd in _context.PedidoDetalhes
                           join l in _context.Lanches on pd.LancheId equals l.LancheId
                           where pd.Pedido.PedidoEnviado >= data
                           group pd by new { pd.LancheId, l.Nome }
                           into g
                           select new
                           {
                               LancheNome = g.Key.Nome,
                               LanchesQuantidade = g.Sum(q => q.Quantidade),
                               LanchesValorTotal = g.Sum(a => a.Preco * a.Quantidade)
                           });
            /*nessa consulta criamos duas variáveis l e pd dizemos que queremos selecionar elas pelo id em equals no where dizemos os dados que queremso puxar em seguida agrupamos tudo em 
             g e ao slecionarmos os dados nas chaves informamos como queremos os dados */

            var listaLanches = new List<LancheGrafico>();

            foreach(var item in lanches)
            {
                var lanche = new LancheGrafico();
                lanche.LancheNome = item.LancheNome;
                lanche.LancheQuantidade = item.LanchesQuantidade;
                lanche.LanchesValorTotal = item.LanchesValorTotal;
                listaLanches.Add(lanche);
            }

            return listaLanches;
                               
        }
    }
}
