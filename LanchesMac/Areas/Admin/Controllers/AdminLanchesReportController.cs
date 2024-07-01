using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using LanchesMac.Areas.Admin.FastReportUtils;
using LanchesMac.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminLanchesReportController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnv;
        private readonly RelatorioVendasLanches _relatorioVendasLanches;

        public AdminLanchesReportController(IWebHostEnvironment webHostEnv, 
            RelatorioVendasLanches relatorioVendasLanches)
        {
            _webHostEnv = webHostEnv;
            _relatorioVendasLanches = relatorioVendasLanches;
        }

        public async Task<ActionResult> LancheCategoriaReport()
        {
            var webReport = new WebReport();// usamos para criar um pdf na web
            var msqlDataConnection = new MsSqlDataConnection();//cria conexão com o sql

            //acessamos o relatório e informamaos o Bd com a instancia de Bd
            webReport.Report.Dictionary.AddChild(msqlDataConnection);

            //caminho do layout
            webReport.Report.Load(Path.Combine(_webHostEnv.ContentRootPath, "wwwroot/reports",
                                                "LanchesPorCategoria.frx"));

            //cria os data tables para cada dado e em cada metodo pegamos os dados
            var lanches = HelperFastReport.GetTable(await _relatorioVendasLanches.GetLancheReport(), "LanchesReport");
            var categoria = HelperFastReport.GetTable(await _relatorioVendasLanches.GetCategoriaReport(), "CategoriaReport");

            //passa os dados para webReport que vai mandar o relatório para a view
            webReport.Report.RegisterData(lanches, "LanchesReport");
            webReport.Report.RegisterData(categoria, "CategoriaReport");

            return View(webReport);
        }
        public async Task<ActionResult> LanchesCategoriaPDF()
        {
            var webReport = new WebReport();// usamos para criar um pdf na web
            var msqlDataConnection = new MsSqlDataConnection();//cria conexão com o sql

            //acessamos o relatório e informamaos o Bd com a instancia de Bd
            webReport.Report.Dictionary.AddChild(msqlDataConnection);

            //caminho do layout
            webReport.Report.Load(Path.Combine(_webHostEnv.ContentRootPath, "wwwroot/reports",
                                                "LanchesPorCategoria.frx"));

            //cria os data tables para cada dado e em cada metodo pegamos os dados
            var lanches = HelperFastReport.GetTable(await _relatorioVendasLanches.GetLancheReport(), "LanchesReport");
            var categoria = HelperFastReport.GetTable(await _relatorioVendasLanches.GetCategoriaReport(), "CategoriaReport");

            //passa os dados para webReport que vai mandar o relatório para a view
            webReport.Report.RegisterData(lanches, "LanchesReport");
            webReport.Report.RegisterData(categoria, "CategoriaReport");

            webReport.Report.Prepare();

            Stream stream = new MemoryStream();//cria o arquivo na memória

            //exporta o relatório para pdf
            webReport.Report.Export(new PDFSimpleExport(), stream);
            stream.Position = 0;

            //return File(stream, "application/zip", "LancheCategoria.pdf");
            return new FileStreamResult(stream, "application/pdf");
        }
    }
}
