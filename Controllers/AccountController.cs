using CodeLab.Models;
using CodeLab.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace sistema_de_login_mvc.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IAdminService _adminService;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IAdminService adminService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _adminService = adminService;
    }

    [HttpGet]
    [Route("Login")]
    public IActionResult Login()
    {
        return View("Login");
    }

    [HttpGet]
    [Route("Registro")]
    public IActionResult Registro() 
    {
        return View("Registro");
    }

    [HttpPost]
    [Route("Registro")]
    public async Task<IActionResult> Registro(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, XpNescessario = 0, XpGanho = 0, Dinheiro = 0};
            
            var result = await _userManager.CreateAsync(user, model.Password!);
            
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Username!,
                model.Password!,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                if(model.Username != _adminService.GetAdminEmail())
                    return RedirectToAction("Index", "Home");
                return RedirectToAction("Index", "Admin");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return View(model);
    }

    [HttpPost]
    [Route("Logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    [Route("ExternalLogin")]
    public IActionResult ExternalLogin(string provider, string returnUrl = "/")
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    [HttpGet]
    [Route("ExternalLoginCallback")]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = "/", string? remoteError = null)
    {
        if (remoteError != null)
        {
            ModelState.AddModelError(string.Empty, $"Erro no login externo: {remoteError}");
            return RedirectToAction(nameof(Login));
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return RedirectToAction(nameof(Login));
        }

        // Verifica se o login externo já está associado a um usuário
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        if (result.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }

        // Se o usuário não existe, cria um novo
        var email = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Email);
        if (email == null)
        {
            ModelState.AddModelError(string.Empty, "O provedor externo não retornou um email.");
            return RedirectToAction(nameof(Login));
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email
        };

        var createResult = await _userManager.CreateAsync(user);
        if (createResult.Succeeded)
        {
            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }

        foreach (var error in createResult.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return RedirectToAction(nameof(Login));
    }

}