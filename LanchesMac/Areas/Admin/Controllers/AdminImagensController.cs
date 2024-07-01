using LanchesMac.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LanchesMac.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class AdminImagensController : Controller
    {
        private readonly ConfigurationImage _cofigurationImage;
        private readonly IWebHostEnvironment _hostingEnviroment;

        public AdminImagensController(IOptions<ConfigurationImage> cofigurationImage, IWebHostEnvironment hostingEnviroment)
        {
            _cofigurationImage = cofigurationImage.Value;
            _hostingEnviroment = hostingEnviroment;
            //usamos o IOptions para ter um acesso fortemente tipado  WebHostEnviroment vamos ter acesso a dados de onde nosso site está
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            if( files == null || files.Count == 0 )
            {
                ViewData["Erro"] = "Error: Arquivos(s) não selecionado(s)";
                return View(ViewData);
            }

            if(files.Count > 10)
            {
                ViewData["Erro"] = "Error: arquivos selecionados superior a dez, seleciona até dez arquivos";
            }

            long size = files.Sum(f => f.Length);

            var filePathName = new List<string>();//lista que armazena os nomes dos arquivos

            //pegamos o caminho de onde vai ser salvo as imagens através das informações da hospedagem, webRootPath acha o caminho da pasata wwwrot
            var filePath = Path.Combine(_hostingEnviroment.WebRootPath, _cofigurationImage.NameFolderImageProduct);

            foreach(var formFile in files)
            {
                if(formFile.FileName.Contains(".jpg")|| formFile.FileName.Contains(".gif")
                    || formFile.FileName.Contains(".png"))
                {
                    var fileNameWithPath = string.Concat(filePath, "\\", formFile.FileName);

                    filePathName.Add(fileNameWithPath);

                    using(var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                        //FileMode.Create permite criar o arquvos, aqui criamos o arquivo no direitorio armazenado em stream
                    }
                }
            }
            ViewData["Resultado"] = $"{files.Count} arquivos foram enviados ao servidor, com o tamnho de {size} bytes";

            ViewBag.Arquivos = filePathName;

            return View(ViewData);
        }  
        public async Task<IActionResult> GetImagens()
        {
            FileManagerModel model = new FileManagerModel();// modelo das imagens

            //caminho de onde estão as imagens
            var userImagesPath = Path.Combine(_hostingEnviroment.WebRootPath, _cofigurationImage.NameFolderImageProduct);

            DirectoryInfo dir = new DirectoryInfo(userImagesPath);

            FileInfo[] files = dir.GetFiles();// pegamos os arquivos do servidor

            //passa o caminho da imagem  para o modelo
            model.PathImageProduct = _cofigurationImage.NameFolderImageProduct;

            if(files.Length == 0)
            {
                ViewData["Erro"] = $"Nenhum arquivo encontrado na pasta {userImagesPath}";
            }

            //passa as imagens para o modelo
            model.Files = files;

            return View(model);
        }

        public async Task<IActionResult> DeleteFiles(string fname)
        {
            string _imgemDeletada = Path.Combine(_hostingEnviroment.WebRootPath,
                  _cofigurationImage.NameFolderImageProduct + "\\", fname);
            //unimos a imagem a ser deletada com o caminho 

            if(System.IO.File.Exists(_imgemDeletada))
            {
                System.IO.File.Delete(_imgemDeletada);
                ViewData["Deletado"] = $"Arquvio {_imgemDeletada} deletado com sucesso";
            }
            return View("Index");
        }
    }
}
