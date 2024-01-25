using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Main_Project.Models;

namespace Main_Project.Data
{
    public class MainProjectContext : DbContext
    {
        public MainProjectContext(DbContextOptions<MainProjectContext> options)
            : base(options)
        {
        }

        public DbSet<Main_Project.Models.Tour> Tours { get; set; } = default!;

        public DbSet<Main_Project.Models.Album> Albums { get; set; } = default!;

        public DbSet<Main_Project.Models.Merchandise> Merchandises { get; set; } = default!;
    }
}