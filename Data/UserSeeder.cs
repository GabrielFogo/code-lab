using CodeLab.Models;
using CodeLab.Services;
using Microsoft.AspNetCore.Identity;

namespace CodeLab.Data;

public class UserSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IAdminService _adminService;

    public UserSeeder(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,IAdminService adminService, IConfiguration configuration)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _adminService = adminService ?? throw new ArgumentNullException(nameof(adminService));
    }

    public async Task Run()
    {
        var adminEmail = _adminService.GetAdminEmail();
        var adminPassword = _adminService.GetAdminPassword();

        await EnsureRoleExists("Admin");
        await EnsureAdminUserExists(adminEmail, adminPassword);
    }


    private async Task EnsureRoleExists(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            var roleResult = await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });
            if (!roleResult.Succeeded)
                throw new Exception($"Failed to create role '{roleName}': {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            
        }
    }

    private async Task EnsureAdminUserExists(string adminEmail, string adminPassword)
    {
        var adminUser = await _userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var userResult = await _userManager.CreateAsync(adminUser, adminPassword);
            
            if (!userResult.Succeeded)
                throw new Exception($"Failed to create admin user: " + $"{string.Join(", ", userResult.Errors.Select(e => e.Description))}");
            

            var roleResult = await _userManager.AddToRoleAsync(adminUser, "Admin");
            if (!roleResult.Succeeded)
                throw new Exception($"Failed to assign 'Admin' role to user: " + $"{string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            
        }
    }
}
