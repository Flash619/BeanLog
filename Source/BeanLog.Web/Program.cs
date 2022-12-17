using BeanLog.Modules.Core.Infrastructure.Extensions;
using BeanLog.Modules.Identity.Infrastructure.Extensions;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreModule(builder.Configuration.GetSection("Modules:Core"));
builder.Services.AddIdentityModule(builder.Configuration.GetSection("Modules:Identity"));

builder.Services.AddControllersWithViews();

builder.Services.AddMudServices(); // Required for prerendering.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection()
    .UseAuthentication()
    .UseBlazorFrameworkFiles()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthorization();

app.MapFallbackToController("Index", "Home");

app.Run();
