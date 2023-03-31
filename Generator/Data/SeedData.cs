﻿using Generator.Authorization;
using Generator.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Generator.Data
{
    // TODO: Add more data for each category. Icons from game-icons.net
    // TODO: Figure out a way to flatten and read the seed data, because typing out individual Add lines kinda sucks. JSON file?
    public static class SeedData
    {
        private static readonly string SeedPathPrefix = @"Data" + Path.DirectorySeparatorChar + "Seeds" + Path.DirectorySeparatorChar;
        private enum SeedTypes
        {
            Vessel,
            Creature,
            Treasure,
            Outpost,
            ReligiousSite,
            Artisan,
            SpecialtyShop
        }

        
        /// <summary>
        /// Check for a pair of users (admin, normal) then seed the database with starter data.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="testUserPass"></param>
        /// <returns></returns>
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPass, string adminEmail, string normalEmail)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var adminID = await EnsureUser(serviceProvider, testUserPass, adminEmail);
                await EnsureRole(serviceProvider, adminID, OperationNames.AdministratorsRole);

                var normalID = await EnsureUser(serviceProvider, testUserPass, normalEmail);

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

        /// <summary>
        /// Populates each table based on JSON file in Seeds folder.
        /// </summary>
        /// <param name="context"></param>
        public static void SeedDB(ApplicationDbContext context)
        {
            // To generate JSON seed file from a POPULATED table in SQL, run /Data/Seeds/GenerateSeedJSON.sql query.
            // The query doesn't work if it's empty lol.
            SeedJSON(context, SeedTypes.Vessel);
            SeedJSON(context, SeedTypes.Creature);
            SeedJSON(context, SeedTypes.Treasure);
            SeedJSON(context, SeedTypes.Outpost);
            SeedJSON(context, SeedTypes.ReligiousSite);
            SeedJSON(context, SeedTypes.Artisan);
            SeedJSON(context, SeedTypes.SpecialtyShop);
        }

        private static void SeedJSON(ApplicationDbContext context, SeedTypes type)
        {
            if (type == SeedTypes.Vessel && !context.Vessel.Any())
            {
                string vesselJSON = File.ReadAllText(SeedPathPrefix + type + ".json");
                if (vesselJSON != null)
                {
                    List<Vessel> vessels = JsonConvert.DeserializeObject<List<Vessel>>(vesselJSON);
                    context.Vessel.AddRange(vessels);
                    context.SaveChanges();
                }
            }

            if (type == SeedTypes.Creature && !context.Creature.Any())
            {
                string creatureJSON = File.ReadAllText(SeedPathPrefix + type + ".json");
                if (creatureJSON != null)
                {
                    List<Creature> creatures = JsonConvert.DeserializeObject<List<Creature>>(creatureJSON);
                    context.Creature.AddRange(creatures);
                    context.SaveChanges();
                }
            }
            if (type == SeedTypes.Treasure && !context.Treasure.Any())
            {
                string treasureJSON = File.ReadAllText(SeedPathPrefix + type + ".json");
                if (treasureJSON != null)
                {
                    List<Treasure> treasures = JsonConvert.DeserializeObject<List<Treasure>>(treasureJSON);
                    context.Treasure.AddRange(treasures);
                    context.SaveChanges();
                }
            }
            if (type == SeedTypes.Outpost && !context.Outpost.Any())
            {
                string outpostJSON = File.ReadAllText(SeedPathPrefix + type + ".json");
                if (outpostJSON != null)
                {
                    List<Outpost> outposts = JsonConvert.DeserializeObject<List<Outpost>>(outpostJSON);
                     context.Outpost.AddRange(outposts);
                     context.SaveChanges();
                }
            }
            if (type == SeedTypes.ReligiousSite && !context.ReligiousSite.Any())
            {
                string religiousSitesJSON = File.ReadAllText(SeedPathPrefix + type + ".json");
                if (religiousSitesJSON != null)
                {
                    List<ReligiousSite> religiousSites = JsonConvert.DeserializeObject<List<ReligiousSite>>(religiousSitesJSON);
                    context.ReligiousSite.AddRange(religiousSites);
                    context.SaveChanges();
                }
            }
            if (type == SeedTypes.Artisan && !context.Artisan.Any())
            {
                string artisanJSON = File.ReadAllText(SeedPathPrefix + type + ".json");
                if (artisanJSON != null)
                {
                    List<Artisan> artisans = JsonConvert.DeserializeObject<List<Artisan>>(artisanJSON);
                    context.Artisan.AddRange(artisans);
                    context.SaveChanges();
                }
            }
            if (type == SeedTypes.SpecialtyShop && !context.SpecialtyShop.Any())
            {
                string specialtyShopsJSON = File.ReadAllText(SeedPathPrefix + type + ".json");
                if (specialtyShopsJSON != null)
                {
                    List<SpecialtyShop> specialtyShops = JsonConvert.DeserializeObject<List<SpecialtyShop>>(specialtyShopsJSON);
                    context.SpecialtyShop.AddRange(specialtyShops);
                    context.SaveChanges();
                }
            }
        }
    }
}
