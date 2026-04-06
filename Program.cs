using Microsoft.EntityFrameworkCore;
using PlantPlanner.Data;
using PlantPlanner.Services.Contracts;
using PlantPlanner.Services.Core;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddScoped<IPlantService, PlantService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = { "Administrator", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(role));

            if (!roleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleResult.Errors.Select(e => e.Description)));
            }
        }
    }

    string adminEmail = "admin@plantplanner.com";
    string adminPassword = "Admin123<";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var createAdminResult = await userManager.CreateAsync(adminUser, adminPassword);

        if (!createAdminResult.Succeeded)
        {
            throw new Exception(string.Join("; ", createAdminResult.Errors.Select(e => e.Description)));
        }
    }

    if (!await userManager.IsInRoleAsync(adminUser, "Administrator"))
    {
        var addAdminRoleResult = await userManager.AddToRoleAsync(adminUser, "Administrator");

        if (!addAdminRoleResult.Succeeded)
        {
            throw new Exception(string.Join("; ", addAdminRoleResult.Errors.Select(e => e.Description)));
        }
    }

    var allUsers = await userManager.Users.ToListAsync();

    foreach (var user in allUsers)
    {
        if (user.Email == adminEmail)
        {
            continue;
        }

        if (!await userManager.IsInRoleAsync(user, "User"))
        {
            var addUserRoleResult = await userManager.AddToRoleAsync(user, "User");

            if (!addUserRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addUserRoleResult.Errors.Select(e => e.Description)));
            }
        }
    }
}
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
