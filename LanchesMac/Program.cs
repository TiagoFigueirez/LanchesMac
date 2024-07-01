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
//cria um host para a nossa aplicação

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
/*assim que adicionamos um serviço do BD usamos a instancia de builder para adicionar o metodo AddDbContext depois passamos a classe e em seguida
 criamos um lambda para dizer que vamos usar o SqlServer e o GetConnectionString para passar a conexão com o bd*/

FastReport.Utils.RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));

//serviço que habilita a criação de usuario com o identity
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
//    //codigo onde você sobreescreve a poliica de dificuldade de senha para o identity
//});

//serviços do nosso repository
builder.Services.AddTransient<ILanchesRepository, LancheRepository>();
builder.Services.AddTransient<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddTransient<IPedidosRepository, PedidosRepository>();
builder.Services.AddScoped<ISeedUserRoleIntial,  SeedUserRoleIntial>();//adiciona o serviço que vai difinir o perfil do usuário
builder.Services.AddScoped<RelatorioVendasService>();//serviço de relatório de vendas
builder.Services.AddScoped<GraficosVendasService>();// serviço de relatório de graficos 
builder.Services.AddScoped<RelatorioVendasLanches>();
builder.Services.AddAuthorization(options =>
{
    //adiciona o serviço de autenticação para erro
    options.AddPolicy("Admin",
        politica =>
        {
            politica.RequireRole("Admin");
        });
});


//regista o serviço que passa a caminho da imagem
builder.Services.Configure<ConfigurationImage>(builder.Configuration.GetSection("ConfigurationFolderImage"));

//usamos o serviço do Http Cotext para pegar os dados do request com ele vai ser invocado uma vez pois e Singleton
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
// serviço que habilita a sessão do carrinho
builder.Services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));

builder.Services.AddPaging(options =>
{
    options.ViewName = "Bootstrap4";
    options.PageParameterName = "pageindex";

});

//colocamos o serviço para usar a memoria do cache e a sessão
builder.Services.AddMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())//verifica se é um ambiente de desenvolvimento
{
    //se for ele mostrará dados detalhados de exessão 
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();//habilita pipelina para o redirecionamento do http
app.UseStaticFiles();//habilia para usar os arquivos estaticos do wwwroot para que este não tenha acesso público deve colocar depois de athorization

//registra o middleware para usar o fast report
app.UseFastReport();


app.UseRouting();//pipeline de roteamento para definir a rota/mapeamento

CriarPerfisUsuarios(app);

//habilitando para usar a Sessão
app.UseSession();

app.UseAuthentication();//habilita a autenticação para criação e usuario com o identity framework core
app.UseAuthorization();//pipeline de autorização 
//lembrando que eles DEVEM ser colocados nessa sequencia


app.MapControllerRoute(
     //mapa para a criação da area admin
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
void CriarPerfisUsuarios(WebApplication app)//cria os perfis iniciais do admin e usuário normal
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();//cria instancia dos serviços para o escopo

    using(var scope = scopedFactory?.CreateScope())//defino meu escopo
    {
        var service = scope?.ServiceProvider.GetService<ISeedUserRoleIntial>();
        service?.SeedRoles();
        service.SeedUsers();
    }
}
