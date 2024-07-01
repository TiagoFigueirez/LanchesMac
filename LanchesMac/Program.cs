using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using LanchesMac.Context;
using LanchesMac.Repository.Interfaces;
using LanchesMac.Repository;
using LanchesMac.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Identity;
using LanchesMac.Services;
using ReflectionIT.Mvc.Paging;
using LanchesMac.Areas.Admin.Services;
using FastReport.Data;


var builder = WebApplication.CreateBuilder(args);
//cria um host para a nossa aplica��o

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
/*assim que adicionamos um servi�o do BD usamos a instancia de builder para adicionar o metodo AddDbContext depois passamos a classe e em seguida
 criamos um lambda para dizer que vamos usar o SqlServer e o GetConnectionString para passar a conex�o com o bd*/

FastReport.Utils.RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));

//servi�o que habilita a cria��o de usuario com o identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>().
                    AddEntityFrameworkStores<AppDbContext>().
                    AddDefaultTokenProviders();

//coneceta o FastReport ao banco de dados 

//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequireDigit = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequiredLength = 3;
//    options.Password.RequiredUniqueChars = 1;
//    //codigo onde voc� sobreescreve a poliica de dificuldade de senha para o identity
//});

//servi�os do nosso repository
builder.Services.AddTransient<ILanchesRepository, LancheRepository>();
builder.Services.AddTransient<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddTransient<IPedidosRepository, PedidosRepository>();
builder.Services.AddScoped<ISeedUserRoleIntial,  SeedUserRoleIntial>();//adiciona o servi�o que vai difinir o perfil do usu�rio
builder.Services.AddScoped<RelatorioVendasService>();//servi�o de relat�rio de vendas
builder.Services.AddScoped<GraficosVendasService>();// servi�o de relat�rio de graficos 
builder.Services.AddScoped<RelatorioVendasLanches>();
builder.Services.AddAuthorization(options =>
{
    //adiciona o servi�o de autentica��o para erro
    options.AddPolicy("Admin",
        politica =>
        {
            politica.RequireRole("Admin");
        });
});


//regista o servi�o que passa a caminho da imagem
builder.Services.Configure<ConfigurationImage>(builder.Configuration.GetSection("ConfigurationFolderImage"));

//usamos o servi�o do Http Cotext para pegar os dados do request com ele vai ser invocado uma vez pois e Singleton
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
// servi�o que habilita a sess�o do carrinho
builder.Services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));

builder.Services.AddPaging(options =>
{
    options.ViewName = "Bootstrap4";
    options.PageParameterName = "pageindex";

});

//colocamos o servi�o para usar a memoria do cache e a sess�o
builder.Services.AddMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())//verifica se � um ambiente de desenvolvimento
{
    //se for ele mostrar� dados detalhados de exess�o 
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();//habilita pipelina para o redirecionamento do http
app.UseStaticFiles();//habilia para usar os arquivos estaticos do wwwroot para que este n�o tenha acesso p�blico deve colocar depois de athorization

//registra o middleware para usar o fast report
app.UseFastReport();


app.UseRouting();//pipeline de roteamento para definir a rota/mapeamento

CriarPerfisUsuarios(app);

//habilitando para usar a Sess�o
app.UseSession();

app.UseAuthentication();//habilita a autentica��o para cria��o e usuario com o identity framework core
app.UseAuthorization();//pipeline de autoriza��o 
//lembrando que eles DEVEM ser colocados nessa sequencia


app.MapControllerRoute(
     //mapa para a cria��o da area admin
     name: "areas",
     pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
     );

app.MapControllerRoute(
    name: "categoriaFiltro",
    pattern: "Lanches/{action}/{categoria?}",
    defaults: new { Controller = "Lanches", action = "List" }
    );

app.MapControllerRoute(//mapeaento da rota
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//defini a url da pagina inicial 

app.Run();
void CriarPerfisUsuarios(WebApplication app)//cria os perfis iniciais do admin e usu�rio normal
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();//cria instancia dos servi�os para o escopo

    using(var scope = scopedFactory?.CreateScope())//defino meu escopo
    {
        var service = scope?.ServiceProvider.GetService<ISeedUserRoleIntial>();
        service?.SeedRoles();
        service.SeedUsers();
    }
}
