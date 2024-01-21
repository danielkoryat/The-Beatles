using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Basic_Project.Models;

namespace Basic_Project.Data
{
    public class Basic_ProjectContext : DbContext
    {
        public Basic_ProjectContext(DbContextOptions<Basic_ProjectContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Basic_Project.Models.Tour> Tours { get; set; } = default!;

        public DbSet<Basic_Project.Models.Album> Albums { get; set; } = default!;

        public DbSet<Basic_Project.Models.Merchandise> Merchandises { get; set; } = default!;
    }
}