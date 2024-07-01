using LanchesMac.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;// usado para registrar o usuario
        private readonly SignInManager<IdentityUser> _signInManager;// classe usada para verificar o login

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            //procura o usuario 
            var user = await _userManager.FindByNameAsync(loginVM.UserName);

            if (user != null)
            {
                /*codigo tenta fazer o login após o usuario ser encontrado colocamos false e false para o cookie não persistir quando o navegdor fechar
                  o outro false e para conta não bloquear caso o login falhe*/
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    //verifica se temos algo em RutrnUrl e se não vai para home
                    if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(loginVM.ReturnUrl);
                }
            }
            ModelState.AddModelError("", "Falha ao realizar login!");
            return View(loginVM);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//usado para evitar falsificaão de requisição entres sites
        public async Task<IActionResult> Register(LoginViewModel registroVM)
        {

            //valida o modelo
            if (ModelState.IsValid)
            {
                var user = new IdentityUser() { UserName = registroVM.UserName };
                var result = await _userManager.CreateAsync(user, registroVM.Password);// tenta cria o usuario com as senha

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("Registro", "Falha ao registrar o usuário");
                }
            }
            return View(registroVM);   
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            //limpa todos os objetos da Session
            HttpContext.Session.Clear();
            //zera o usario logado
            HttpContext.User = null;
            await _signInManager.SignOutAsync();   
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}