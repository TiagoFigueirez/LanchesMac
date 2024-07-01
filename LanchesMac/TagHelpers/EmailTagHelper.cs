using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Identity.Client;

namespace LanchesMac.TagHelpers
{
    public class EmailTagHelper : TagHelper
    {
       public string Endereco {  get; set; } 
       public string Conteudo {  get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href","emailto" + Endereco);
            output.Content.SetContent(Conteudo);    
        }
    }
}
