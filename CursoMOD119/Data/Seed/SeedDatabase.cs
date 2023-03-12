using CursoMOD119.Lib;
using Microsoft.AspNetCore.Identity;

namespace CursoMOD119.Data.Seed
{
    public class SeedDatabase
    {
        public static void Seed(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager,
                               RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager).Wait();
            SeedUsers(userManager).Wait();
        }

        //Create a 'admin' and 'operative' roles
        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            try
            {
                var roleCheck = await roleManager.RoleExistsAsync(AppConstants.ADMIN_ROLE);

                if (!roleCheck)
                {
                    var adminRole = new IdentityRole
                    {
                        Name = AppConstants.ADMIN_ROLE
                    };
                    var roleResult = await roleManager.CreateAsync(adminRole);
                }

                roleCheck = await roleManager.RoleExistsAsync(AppConstants.OPERATIVE_ROLE);

                if (!roleCheck)
                {
                    var operativeRole = new IdentityRole
                    {
                        Name = AppConstants.OPERATIVE_ROLE
                    };
                    var roleResult = await roleManager.CreateAsync(operativeRole);
                }
            }
            catch (Exception ex)
            {

            }
        }

        //Creat 'admin' and 'operative' user with respective roles
        private static async Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            try
            {
                IdentityUser adminUser = new IdentityUser
                {
                    UserName = AppConstants.ADMIN_USER,
                    Email = AppConstants.ADMIN_EMAIL
                };
                var dbAdmin = await userManager.FindByNameAsync(adminUser.UserName);

                if (dbAdmin == null)
                {
                    var result = await userManager.CreateAsync(adminUser, AppConstants.ADMIN_PWD);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, AppConstants.ADMIN_ROLE);
                        dbAdmin = await userManager.FindByNameAsync(adminUser.UserName);
                        dbAdmin!.EmailConfirmed = true;
                        await userManager.UpdateAsync(dbAdmin);
                    }
                }

                IdentityUser operativeUser = new IdentityUser
                {
                    UserName = AppConstants.OPERATIVE_USER,
                    Email = AppConstants.OPERATIVE_EMAIL
                };
                var dbOperative = await userManager.FindByNameAsync(operativeUser.UserName);

                if (dbOperative == null)
                {
                    var result = await userManager.CreateAsync(operativeUser, AppConstants.OPERATIVE_PWD);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(operativeUser, AppConstants.OPERATIVE_ROLE);
                        dbOperative = await userManager.FindByNameAsync(operativeUser.UserName);
                        dbOperative!.EmailConfirmed = true;
                        await userManager.UpdateAsync(dbOperative);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
