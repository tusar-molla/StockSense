using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockSense.Interface;
using StockSense.Models.ViewModels.Auth;
using System.Security.Claims;

namespace StockSense.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }
            try
            {
                await _authService.RegisterAsync(register);
                TempData["SuccessMessage"] = "Registration successful! Please log in.";
                return RedirectToAction("Login", "Auth");
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "Registration Unsuccessful! " + ex.Message;
                return View(register);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login) {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            try
            {
                var user = await _authService.LoginAsync(login.Email!, login.Password!);

                var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.FullName!),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Role, user.RoleId!.ToString())
            };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                        AllowRefresh = true
                    });
                TempData["SuccessMessage"] = $"Welcome back, {user.FullName}!";
                return RedirectToAction("Dashboard", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Login Unsuccessful! " + ex.Message;
                return View(login);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }


        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                if (model.ConfirmPassword != model.NewPassword)
                {
                    throw new Exception("New password and confirm password do not match");
                }
                var result = await _authService.ResetPasswordAsync(model.CurrentPassword!, model.NewPassword!);
                if (result)
                {
                    TempData["SuccessMessage"] = "Password successfully reset!";
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return View(model);
        }
    }
}
