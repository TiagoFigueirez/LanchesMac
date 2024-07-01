using LanchesMac.Models;
using LanchesMac.Repository;
using LanchesMac.Repository.Interfaces;
using LanchesMac.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class LanchesController : Controller
    {
        private readonly ILanchesRepository _lanchesRepository;

        public LanchesController(ILanchesRepository lanchesRepository)
        {
            _lanchesRepository = lanchesRepository;
        }

        public IActionResult List(string categoria)
        {
            //var allLanches = _lanchesRepository.Lanches;
            //return View(allLanches); forma de fazer sem uma view model

            IEnumerable<Lanche> lanches;
            string categoriaAtual = string.Empty;

            if (String.IsNullOrEmpty(categoria))
            {
                lanches = _lanchesRepository.Lanches.OrderBy(l => l.LancheId).ToList();
                categoriaAtual = "Todos os Lanches";
            }
            else
            {
                //if (string.Equals("Normal", categoria, StringComparison.OrdinalIgnoreCase))
                //{
                //    lanches = _lanchesRepository.Lanches.Where(l => l.Categoria.CategoriaNome.Equals("Normal")).OrderBy(l => l.Nome);
                //}
                //else
                //{
                //    lanches = _lanchesRepository.Lanches.Where(l => l.Categoria.CategoriaNome.Equals("Natural")).OrderBy(l => l.Nome);
                //}
                //categoriaAtual = categoria; -- antes de fazer toda logica filtramos os lanches pelo dado recebido do request
                lanches = _lanchesRepository.Lanches
                          .Where(l => l.Categoria.CategoriaNome.Equals(categoria)).OrderBy(c => c.Nome);
            }

            var LanchesViewModel = new LanchesListViewModel()
            {
                Lanches = lanches,
                CategoriaAtual = categoriaAtual
            };

            return View(LanchesViewModel);
        }
        public IActionResult Details(int lancheId)
        {
            var lanche = _lanchesRepository.Lanches.FirstOrDefault(l=> l.LancheId == lancheId);
            return View(lanche);
        }

        public IActionResult Search(string searchString)
        {
            IEnumerable<Lanche> lanches;
            string categoriaAtual = string.Empty;
            if (string.IsNullOrEmpty(searchString))
            {
                lanches = _lanchesRepository.Lanches.OrderBy(p =>p.LancheId);
                categoriaAtual = "Todos os Lanches";
            }
            else
            {
                lanches = _lanchesRepository.Lanches
                          .Where(P => P.Nome.ToLower().Contains(searchString.ToLower()));

                if (lanches.Any())//verifica se algo foi encontrado e armazenado
                    categoriaAtual = "Lanches";
                else
                    categoriaAtual = "Nenhum lanche foi encontrado";
                
            }
            return View("~/Views/Lanches/List.cshtml", new LanchesListViewModel
            {
                Lanches= lanches,
                CategoriaAtual =categoriaAtual
            });
        }
    }

}
/*
   ViewData["Titulo"] = "Todos os Lanches";
   ViewData["DataAtual"]= DateTime.Now.ToString("dd/MM/yyyy");
   ViewBag.Total = "Total de Lanches";
   int AllLanchesCount = allLanches.Count();
   ViewBag.TotalLanches = AllLanchesCount;
   TempData["Nome"] = "Tiago";
   para recuperalos nas views basta adicionar o @ sem o =
 */