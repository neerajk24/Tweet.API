using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tweet.API.Interface;
using Tweet.API.Repositories;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container
builder.Services.AddControllers();

// Add your DI container configuration code here
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Replace UserRepository with the actual implementation of IUserRepository


// Add services to the container.
builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapControllers(); // Map controllers
app.MapRazorPages();

app.Run();
