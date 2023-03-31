using Generator.Authorization;
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
        
        /// <summary>
        /// Add Creatures to the database if it's empty
        /// </summary>
        /// <param name="context"></param>
        private static void SeedCreatures(ApplicationDbContext context)
        {
            if (context.Creature.Any())
            {
                return;
            }

            context.Creature.Add(new Creature
            {
                Name = "Bandit",
                Description = "Generic guy, low skills.",
                Image = "Humanoid.png"
            });
            context.Creature.Add(new Creature
            {
                Name = "Big Bandit",
                Description = "Tough guy, low skills.",
                Image = "Humanoid.png"
            });
            context.Creature.Add(new Creature
            {
                Name = "Lizard",
                Description = "Small and cute.",
                Image = "Beast.png"
            });
            context.Creature.Add(new Creature
            {
                Name = "Cat",
                Description = "Fluffy.",
                Image = "Beast.png"
            });
            context.SaveChanges();
        }

        /// <summary>
        /// Add Treasures to the database if it's empty
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        private static void SeedTreasures(ApplicationDbContext context)
        {
            if (context.Treasure.Any())
            {
                return;
            }

            context.Treasure.Add(new Treasure
            {
                Name = "Gold coins",
                Description = "Pile of gold coins. Roll 5d12.",
                Image = "Treasure.png"
            });
            context.Treasure.Add(new Treasure
            {
                Name = "Gems",
                Description = "Small bag of assorted gems. Roll 5d12 x 100 for gold value.",
                Image = "Gem.png"
            });
            context.Treasure.Add(new Treasure
            {
                Name = "Silverware",
                Description = "Forks, knives, and spoons in good condition.",
                Image = "Treasure.png"
            });
            context.Treasure.Add(new Treasure
            {
                Name = "Dagger",
                Description = "A dagger with no sheath.",
                Image = "Sword.png"
            });
            context.SaveChanges();
        }

        /// <summary>
        /// Add Outposts to the database if it's empty
        /// </summary>
        /// <param name="context"></param>
        private static void SeedOutposts(ApplicationDbContext context)
        {
            if (context.Outpost.Any())
            {
                return;
            }

            context.Outpost.Add(new Outpost
            {
                Name = "Small Outpost",
                Image = "Outpost.png",
                ReligionCapacity = 1,
                ArtisanCapacity = 3,
                SpecialtyShopCapacity = 1
            });
            context.Outpost.Add(new Outpost
            {
                Name = "Medium Outpost",
                Image = "Outpost.png",
                ReligionCapacity = 2,
                ArtisanCapacity = 6,
                SpecialtyShopCapacity = 3
            });
            context.Outpost.Add(new Outpost
            {
                Name = "Large Outpost",
                Image = "Outpost.png",
                ReligionCapacity = 4,
                ArtisanCapacity = 10,
                SpecialtyShopCapacity = 5
            });

            context.SaveChanges();
        }

        /// <summary>
        /// Add ReligiousSites to the database if it's empty
        /// </summary>
        /// <param name="context"></param>
        private static void SeedReligiousSites(ApplicationDbContext context)
        {
            if (context.ReligiousSite.Any())
            {
                return;
            }

            context.ReligiousSite.Add(new ReligiousSite
            {
                Name = "Small Statue",
                Image = "ReligiousSite.png",
                Description = "A small statue to a deity is neatly presented in the middle of the outpost."
            });
            context.ReligiousSite.Add(new ReligiousSite
            {
                Name = "Temple Shrine",
                Image = "ReligiousSite.png",
                Description = "A temple shrine to a deity is built at one end of the outpost. People regularly place small offerings and light incense in front of its statue.\r\n"
            });
            context.ReligiousSite.Add(new ReligiousSite
            {
                Name = "Church",
                Image = "ReligiousSite.png",
                Description = "A stout church is built facing the outpost center square. People regularly gather for ceremonies and a cleric maintains the building.\r\n"
            });
            context.ReligiousSite.Add(new ReligiousSite
            {
                Name = "Temple",
                Image = "ReligiousSite.png",
                Description = "An ornate temple is built at one edge of the outpost. Statues to respective deities are slotted into the walls, and a monk lives nearby.\r\n"
            });
            context.ReligiousSite.Add(new ReligiousSite
            {
                Name = "Tree Shrine",
                Image = "ReligiousSite.png",
                Description = "The locals worship at a massive tree with a twisted trunk and spreading branches.\r\n"
            });
            context.ReligiousSite.Add(new ReligiousSite
            {
                Name = "Cave Shrine",
                Image = "ReligiousSite.png",
                Description = "A nearby cave is home to a small shrine where water drips down a carved wall. Locals make offerings here for good fortune.\r\n"
            });

            context.SaveChanges();
        }

        /// <summary>
        /// Add Artisans to the database if it's empty
        /// </summary>
        /// <param name="context"></param>
        private static void SeedArtisans(ApplicationDbContext context)
        {
            if (context.Artisan.Any())
            {
                return;
            }

            context.Artisan.Add(new Artisan
            {
                Name = "Alchemists and apothecaries",
                Image = "Artisan.png",
                Description = "They sell specialty potions and will offer trades for rare herbs and materials."
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Armorers, locksmiths, and finesmiths",
                Image = "Artisan.png",
                Description = "They sell lock picking tools and armor."
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Brewers, distillers, and vintners",
                Image = "Artisan.png",
                Description = "Have a fantastic ale!"
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Calligraphers, scribes, and scriveners",
                Image = "Artisan.png",
                Description = "Scribes can often decipher smudged or damaged writing."
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Carpenters, roofers, and plasterers",
                Image = "Artisan.png",
                Description = "Excellent for ship and wagon repair."
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Cartographers, surveyors, and chart-makers",
                Image = "Artisan.png",
                Description = "They sell maps of the area and can match up damaged or old maps and directions."
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Cobblers and shoemakers",
                Image = "Artisan.png",
                Description = "Great for fixing boots and finding specialty boots."
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Cooks and bakers",
                Image = "Artisan.png",
                Description = "Want to hire a better cook?"
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Glassblowers and glaziers",
                Image = "Artisan.png",
                Description = "Repairing spyglasses and glass items."
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Jewelers and gemcutters",
                Image = "Artisan.png",
                Description = "Appraisals included!"
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Leatherworkers, skinners, and tanners",
                Image = "Artisan.png",
                Description = "They can repair armor as well as bags. They can also craft things from many hides."
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Masons and stonecutters",
                Image = "Artisan.png",
                Description = "Great at identifying odd materials in weapons and spearheads."
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Painters, limners, and sign-makers",
                Image = "Artisan.png",
                Description = "Less utility in this world but useful for appraisals on painted artwork."
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Potters and tile-makers",
                Image = "Artisan.png",
                Description = "Appraisals."
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Shipwrights and sailmakers",
                Image = "Artisan.png",
                Description = "Ship building, design, and repairs."
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Smiths and metal-forgers",
                Image = "Artisan.png",
                Description = "Fixing and crafting metal weaponry."
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Tinkers, pewterers, and casters",
                Image = "Artisan.png",
                Description = "Appraisal of loot mostly."
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Wagon-makers and wheelwrights",
                Image = "Artisan.png",
                Description = "Everyone needs carts and wheels."
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Weavers and dyers",
                Image = "Artisan.png",
                Description = "Need a change of sails? Cloth armor"
            });
            context.Artisan.Add(new Artisan
            {
                Name = "Woodcarvers, coopers, and bowyers",
                Image = "Artisan.png",
                Description = "Wooden weapons such as staves and bows, appraising carved items, etc."
            });

            context.SaveChanges();
        }

        /// <summary>
        /// Add SpecialtyShops to the database if it's empty
        /// </summary>
        /// <param name="context"></param>
        private static void SeedSpecialtyShops(ApplicationDbContext context)
        {
            if (context.SpecialtyShop.Any())
            {
                return;
            }

            context.SpecialtyShop.Add(new SpecialtyShop
            {
                Name = "Necromancer",
                Image = "SpecialtyShop.png",
                Description = "Why not put wild animals back together again? Look how well it... er, moves..."
            });
            context.SpecialtyShop.Add(new SpecialtyShop
            {
                Name = "Musician",
                Image = "SpecialtyShop.png",
                Description = "Retired from traveling as a bard but world-renowned for their instruments. Can also repair broken instruments."
            });
            context.SpecialtyShop.Add(new SpecialtyShop
            {
                Name = "Tattoo Artist",
                Image = "SpecialtyShop.png",
                Description = "Ranges from basic flash tattoos and temporary ink to very expensive magical tattoos."
            });
            context.SpecialtyShop.Add(new SpecialtyShop
            {
                Name = "Trinket Collector",
                Image = "SpecialtyShop.png",
                Description = "Trinkets of all kinds!"
            });
            context.SpecialtyShop.Add(new SpecialtyShop
            {
                Name = "Tinkerer",
                Image = "SpecialtyShop.png",
                Description = "Anything from toys and small fire-starters to little mechanical dragons and other contraptions."
            });
            context.SpecialtyShop.Add(new SpecialtyShop
            {
                Name = "Gambling Bar",
                Image = "SpecialtyShop.png",
                Description = "Play various games of chance and maybe take on the local big-shots."
            });
            context.SpecialtyShop.Add(new SpecialtyShop
            {
                Name = "Pet Shop",
                Image = "SpecialtyShop.png",
                Description = "Local fauna for sale! May or may not live very long...(bonus if Necromancer is on the same map)"
            });
            context.SpecialtyShop.Add(new SpecialtyShop
            {
                Name = "Fortune Teller",
                Image = "SpecialtyShop.png",
                Description = "A master of bullshittery...or is it?"
            });
            context.SpecialtyShop.Add(new SpecialtyShop
            {
                Name = "Poison Shop",
                Image = "SpecialtyShop.png",
                Description = "Poisons for blades, beverages, and anything you need. Not the easiest shop to find."
            });
            context.SpecialtyShop.Add(new SpecialtyShop
            {
                Name = "Exotic Hunter",
                Image = "SpecialtyShop.png",
                Description = "Hides and preserved parts from creatures around the world."
            });
            context.SpecialtyShop.Add(new SpecialtyShop
            {
                Name = "Tea Shop",
                Image = "SpecialtyShop.png",
                Description = "Teas of all types, from medicinal to flavorful."
            });
            context.SpecialtyShop.Add(new SpecialtyShop
            {
                Name = "Theater",
                Image = "SpecialtyShop.png",
                Description = "Performances from local bards, storytellers, and local talent."
            });
            context.SaveChanges();
        }
    }
}
