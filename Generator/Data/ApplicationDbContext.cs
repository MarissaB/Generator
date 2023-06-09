﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Generator.Models;

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
    }
}