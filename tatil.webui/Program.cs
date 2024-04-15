using tatil.business;
using tatil.data;
using tatil.data.Concrete;
using tatil.webui;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCookieSettings();
builder.Services.AddBusinessServices();
builder.Services.AddDataServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

SeedDatabase.Seed(app);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "tatil-evleri",
    pattern: "/tatil-evleri",
    defaults: new { controller = "Home", Action = "Adverts" }
);

app.MapControllerRoute(
    name: "mainPage",
    pattern: "/",
    defaults: new { controller = "Home", Action = "Index" }
);


app.MapControllerRoute(
    name: "hakkimizda",
    pattern: "/hakkimizda",
    defaults: new { controller = "Home", Action = "AboutUs" }
);


app.MapControllerRoute(
    name: "iletisim",
    pattern: "/iletisim",
    defaults: new { controller = "Home", Action = "Contact" }
);

app.MapControllerRoute(
    name: "giris",
    pattern: "/giris",
    defaults: new { controller = "Auth", Action = "Login" }
);

app.MapControllerRoute(
    name: "cikis",
    pattern: "/cikis",
    defaults: new { controller = "Auth", Action = "Logout" }
);

app.MapControllerRoute(
    name: "userPanel",
    pattern: "/user/{action}/{id?}",
    defaults: new { controller = "User", Action = "Index" }
);

app.MapControllerRoute(
    name: "authpanel",
    pattern: "/auth/{action}/{id?}",
    defaults: new { controller = "Auth", Action = "Index" }
);

app.MapControllerRoute(
    name: "homepanel",
    pattern: "/Home/{action}/{id?}",
    defaults: new { controller = "Home", Action = "Index" }
);


app.MapControllerRoute(
    name: "adminPanel",
    pattern: "/Admin/{action}/{id?}",
    defaults: new { controller = "Admin", Action = "Index" }
);

app.MapControllerRoute(
    name: "getByCategory",
    pattern: "/{categoryUrl}",
    defaults: new { controller = "Home", Action = "Adverts" }
);

app.MapControllerRoute(
    name: "ilandetay",
    pattern: "/{categoryUrl}/{advertUrl}",
    defaults: new { controller = "Home", Action = "AdvertDetails" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
