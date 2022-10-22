using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URLShortener.Data
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string adminName = "admin";
            string password = "_Aa123456";

            string user1Email = "user1@gmail.com";
            string user1Name = "user1";
            string user2Email = "user2@gmail.com";
            string user2Name = "user2";


            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }


            if (await userManager.FindByNameAsync(adminName) == null)
            {
                IdentityUser admin = new IdentityUser { Email = adminEmail, UserName = adminName, EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

            if (await userManager.FindByNameAsync(user1Name) == null)
            {
                IdentityUser user = new IdentityUser { Email = user1Email, UserName = user1Name, EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                }
            }
            if (await userManager.FindByNameAsync(user2Name) == null)
            {
                IdentityUser user = new IdentityUser { Email = user2Email, UserName = user2Name, EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                }
            }
        }
    }
}
