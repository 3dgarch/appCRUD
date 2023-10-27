using CRUD.Models;
using CRUD.Repositorios.Contrato;
using CRUD.Repositorios.Implementacion;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


/* usar referencias*/
builder.Services.AddScoped<IGenericRepository<Categoria>, CategoriaRepository>();
builder.Services.AddScoped<IGenericRepository<Producto>, ProductoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
