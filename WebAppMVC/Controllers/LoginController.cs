using Business.Abstract;
using Business.Contants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            this._loginService = loginService;
        }
        public IActionResult Login()
        {
            LoginModel objLoginModel = new LoginModel();
            ViewBag.Message1 = "Yanlış Kullanıcı adı veya şifre";
            return View(objLoginModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel objLoginModel)
        {
            if (!ModelState.IsValid)
            {
                var result = _loginService.Login(objLoginModel.UserName, objLoginModel.Password);
                if (result.Success)
                {
                    var claims = new List<Claim>() {
                        new Claim(ClaimTypes.NameIdentifier, Convert.ToString(result.Data))
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {
                        IsPersistent = objLoginModel.RememberLogin
                    });

                    return LocalRedirect("/");

                }
                else
                {
              
                    ModelState.AddModelError("", "Yanlış kullanıcı adı ya da şifre");

                }

            }
            return LocalRedirect("/");

        }

        public async Task<IActionResult> LogOut()
        {
            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page    
            return LocalRedirect("/Login/Login");
        }
    }
}
