using Microsoft.AspNetCore.Identity;
using System.Net;
using tsuHelp.Models;

namespace tsuHelp.Data
{
    public class Seed
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                //var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                //if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                //    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                //if (!await roleManager.RoleExistsAsync(UserRoles.User))
                //    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                //var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                //var userFirst = await userManager.FindByEmailAsync("tema2000art@gmail.com");
                //var userSec = await userManager.FindByEmailAsync("test@gmail.com");
                //await userManager.AddToRoleAsync(userFirst, UserRoles.User);
                //await userManager.AddToRoleAsync(userSec, UserRoles.User);
                //User userFirst = await _userManager.FindByIdAsync("a49547b8-906e-4301-be3e-f003e2966e64");
                //User userSecond = await _userManager.FindByIdAsync("dc07d36d-96af-4546-a500-66e6296b8022");

                //IdentityResult resultFirst = await _userManager.DeleteAsync(userFirst);
                //IdentityResult resultSecond = await _userManager.DeleteAsync(userSecond);

                //Users

                //var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                //string adminUserEmail = "tema2003art@gmail.com";


                //var newAdminUser = new User()
                //{
                //    UserName = adminUserEmail,
                //    Email = adminUserEmail,
                //    EmailConfirmed = true,
                //    Name = "Артем",
                //    Surname = "Руссков",
                //};
                //await userManager.CreateAsync(newAdminUser, "AdMN_ssW0rd");///////////////////////////////////////////////////////////////
                //await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);


                
            }
        }
    }
}