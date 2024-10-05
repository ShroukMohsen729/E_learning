using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
namespace E_learning
{
    public class E_learningcontext : DbContext
    { 
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Uploaded> Uploaded { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-96AGL16\\MYSQL;Initial Catalog=E_learningdatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False");
        }
        public E_learningcontext() { }
    }
}
