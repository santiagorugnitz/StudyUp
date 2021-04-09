using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace DataAccess
{
    public class DBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Flashcard> Flashcards { get; set; }

        public DBContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Deck>().HasKey(d => d.Id);
            modelBuilder.Entity<Flashcard>().HasKey(f => f.Id);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { optionsBuilder.UseLazyLoadingProxies(); }

    }
}
