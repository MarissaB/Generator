using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Generator.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Generator.Models.Vessel> Vessel { get; set; } = default!;
        public DbSet<Generator.Models.Creature> Creature { get; set; } = default!;
        public DbSet<Generator.Models.Treasure> Treasure { get; set; } = default!;
        public DbSet<Generator.Models.Outpost> Outpost { get; set; } = default!;
        public DbSet<Generator.Models.ReligiousSite> ReligiousSite { get; set; } = default!;
        public DbSet<Generator.Models.Artisan> Artisan { get; set; } = default!;
        public DbSet<Generator.Models.SpecialtyShop> SpecialtyShop { get; set; } = default!;
        public DbSet<Generator.Models.Container> Container { get; set; } = default!;
        public DbSet<Generator.Areas.Identity.Data.UserConfig> UserConfig { get; set; } = default!;
        public DbSet<Generator.Models.Encounter> Encounter { get; set; } = default!;
        public DbSet<Generator.Models.Participant> Participant { get; set; } = default!;
    }
}