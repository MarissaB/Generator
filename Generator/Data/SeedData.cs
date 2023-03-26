using Generator.Authorization;
using Generator.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Generator.Data
{
    public static class SeedData
    {
        /// <summary>
        /// Check for a pair of users (admin, normal) then seed the database with starter data.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="testUserPass"></param>
        /// <returns></returns>
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPass)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var adminID = await EnsureUser(serviceProvider, testUserPass, "admin@example.com");
                await EnsureRole(serviceProvider, adminID, OperationNames.AdministratorsRole);

                var normalID = await EnsureUser(serviceProvider, testUserPass, "normal@example.com");

                SeedDB(context);
            }
        }
        /// <summary>
        /// Ensure that the user exists and create if they don't
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="testUserPass"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string testUserPass, string userName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new IdentityUser { UserName = userName, EmailConfirmed = true };
                await userManager.CreateAsync(user, testUserPass);
            }

            if (user == null)
            {
                throw new Exception("EnsureUser failed - test PW probably not strong enough.");
            }
            return user.Id;
        }
        /// <summary>
        /// Ensure that the user has the specified role and assign if they don't
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider, string userId, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            if (roleManager == null)
            {
                throw new Exception("EnsureRole failed - roleManager is null.");
            }

            IdentityResult identityResult;
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("EnsureRole failed - testUserPw not strong enough.");
            }
            identityResult = await userManager.AddToRoleAsync(user, role);
            return identityResult;
        }

        public static void SeedDB(ApplicationDbContext context)
        {
            SeedVessels(context);
            // SeedCreatures();
            // SeedTreasures();
        }

        /// <summary>
        /// Adds Vessels to the database if it's empty.
        /// </summary>
        /// <param name="context"></param>
        private static void SeedVessels(ApplicationDbContext context)
        {
            if (context.Vessel.Any())
            {
                return;
            }

            context.Vessel.Add(new Vessel
            {
                Name = "Rowboat", 
                CreatureCapacity = 2, 
                TreasureCapacity = 5, 
                Image = "Rowboat.png" 
            });
            context.Vessel.Add(new Vessel
            {
                Name = "Keelboat",
                CreatureCapacity = 4,
                TreasureCapacity = 10,
                Image = "Keelboat.png"
            });
            context.Vessel.Add(new Vessel
            {
                Name = "Galleon",
                CreatureCapacity = 20,
                TreasureCapacity = 15,
                Image = "Galleon.png"
            });
            context.Vessel.Add(new Vessel
            {
                Name = "Warship",
                CreatureCapacity = 20,
                TreasureCapacity = 15,
                Image = "Warship.png"
            });
            context.Vessel.Add(new Vessel
            {
                Name = "Crate",
                CreatureCapacity = 0,
                TreasureCapacity = 4,
                Image = "Crate.png"
            });
            context.Vessel.Add(new Vessel
            {
                Name = "Barrel",
                CreatureCapacity = 1,
                TreasureCapacity = 2,
                Image = "Barrel.png"
            });
            context.SaveChanges();
        }
    }
}
