using LanchesMac.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminGraficoController : Controller
    {
        private readonly GraficosVendasService _graficoVendasService;

        public AdminGraficoController(GraficosVendasService graficoVendasService)
        {
            _graficoVendasService = graficoVendasService ?? throw 
                new ArgumentNullException(nameof(graficoVendasService));
        }

        public JsonResult VendasLanches(int dias)
        {
            var lancheVendasTotais = _graficoVendasService.GetVendasLanches(dias);
            return Json(lancheVendasTotais);
            //paga os dados e converte no formato Json
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VendasMensal()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VendasSemanal()
        {
            return View();
        }
    }
}
