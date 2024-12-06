using CodeLab.Data;
using CodeLab.Models;
using CodeLab.Repositories;
using CodeLab.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;


var builder = WebApplication.CreateBuilder(args);

// Configurações de MongoDB
ContextMongoDb.ConnectionString = builder.Configuration.GetSection("MongoConnection:ConnectionString").Value;
ContextMongoDb.DatabaseName = builder.Configuration.GetSection("MongoConnection:Database").Value;
ContextMongoDb.IsSsl = Convert.ToBoolean(builder.Configuration.GetSection("MongoConnection:IsSSL").Value);

// Configuração do Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(
        ContextMongoDb.ConnectionString, ContextMongoDb.DatabaseName
    );

// Configuração de autenticação externa (Google e Facebook)
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]
            ?? throw new InvalidOperationException("Google ClientId não configurado.");
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]
            ?? throw new InvalidOperationException("Google ClientSecret não configurado.");
        options.CallbackPath = "/signin-google";
    })
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"]
            ?? throw new InvalidOperationException("Facebook AppId não configurado.");
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]
            ?? throw new InvalidOperationException("Facebook AppSecret não configurado.");
    });

// Autorização com política Admin
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policyBuilder => policyBuilder.RequireRole("Admin"));

// Configuração de cookies de autenticação
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login"; // Caminho de login
    options.AccessDeniedPath = "/"; // Caminho de acesso negado
});

// Serviços adicionais
builder.Services.AddTransient<UserSeeder>();
builder.Services.AddSingleton<ContextMongoDb>();
builder.Services.AddSingleton<IPerguntaRepository, PerguntasRepository>();
builder.Services.AddSingleton<IAdminService, AdminService>();
builder.Services.AddSingleton<IQuizRepository, QuizRepository>();

builder.Services.AddSession();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configurações de produção
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// Middleware de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Adicionando seeding do admin na inicialização
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();
    await seeder.Run(); // Chama o método que faz o seed do usuário admin
}

// Configuração de rotas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
