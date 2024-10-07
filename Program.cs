using CodeLab.Data;
using CodeLab.Models;

var builder = WebApplication.CreateBuilder(args);

ContextMongoDb.ConnectionString = builder.Configuration.GetSection("MongoConnection:ConnectionString").Value;
ContextMongoDb.DatabaseName = builder.Configuration.GetSection("MongoConnection:Database").Value;
ContextMongoDb.IsSsl = Convert.ToBoolean(builder.Configuration.GetSection("MongoConnection:IsSSL").Value);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(
        ContextMongoDb.ConnectionString, ContextMongoDb.DatabaseName
    );

builder.Services.AddControllersWithViews();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
