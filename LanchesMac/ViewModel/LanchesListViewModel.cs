using LanchesMac.Models;

namespace LanchesMac.ViewModel
{
    public class LanchesListViewModel
    {/*view model e utilizado quando queremos exibir duas entidades em uma unica view no caso a categoria e lanches e define como os dados vao
        ser exibidas*/
        public IEnumerable<Lanche> Lanches { get; set; }
        public string CategoriaAtual { get; set; }
    }
}
