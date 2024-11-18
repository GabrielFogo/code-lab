
using CodeLab.Data;
using CodeLab.Models;
using CodeLab.Repositories;
using CodeLab.Services;
using Microsoft.AspNetCore.Identity;

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

// Autorização com política Admin
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policyBuilder => policyBuilder.RequireRole("Admin"));

// Configuração de cookies de autenticação
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login"; // Defina o caminho de login
    options.AccessDeniedPath = "/";
});

// Registre o UserSeeder para poder injetá-lo
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
// Adicionar autenticação antes da autorização
app.UseAuthentication();
app.UseAuthorization();

// Adicionando seeding do admin na inicialização
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();
    await seeder.Run(); // Chama o método que faz o seed do usuário admin
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();