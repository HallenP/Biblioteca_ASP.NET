using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProyectoF2.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

/*=================AUTENTICACION=====================*/
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        //se debe crear el login
        option.LoginPath = "/Acceso/Index";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        option.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.Redirect("/Home/SinPermiso");
            return Task.CompletedTask;
        };
    });

builder.Services.AddDbContext<BibliotecaMarcos3Context>(options =>
         options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


//builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(pdfTools));

//pdfTools.ObjectSettings.WkHtmlToPdfPath = "RUTA_ABSOLUTA_WKHTMLTOPDF";

//var pdfTools = new PdfTools();



//builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(pdfTools));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Index}/{id?}");

IWebHostEnvironment env = app.Environment;
Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "../Rotativa/Windows");

app.Run();
