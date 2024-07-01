namespace LanchesMac.Models
{
    public class FileManagerModel
    {
        public FileInfo[] Files { get; set; }//acesso a metodos para tratar os arquivos
        public IFormFile IFormFile { get; set; }// interface que permite armazena e gerenciar os arquivos para envio atraves de requests http
        public List<IFormFile> IFormFileS {get; set;}// lista de arquivos
        public string PathImageProduct { get; set; }// caminho da imagem 
    }
}
