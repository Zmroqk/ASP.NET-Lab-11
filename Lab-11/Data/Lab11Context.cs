using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lab_11.Models;

namespace Lab_11.Data
{
    public class Lab11Context : DbContext
    {
        public Lab11Context (DbContextOptions<Lab11Context> options)
            : base(options)
        {
        }

        public DbSet<Lab_11.Models.Category> Category { get; set; }

        public DbSet<Lab_11.Models.Article> Article { get; set; }
    }
}
