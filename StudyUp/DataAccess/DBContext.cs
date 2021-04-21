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
        public DbSet<Group> Groups { get; set; }

        public DBContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Deck>().HasKey(d => d.Id);
            modelBuilder.Entity<Flashcard>().HasKey(f => f.Id);
            modelBuilder.Entity<Group>().HasKey(f => f.Id);

            modelBuilder.Entity<User>()
                .HasMany(g => g.Groups)
                .WithOne(c => c.Creator)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(d => d.Decks)
                .WithOne(a => a.Author)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.FollowedUsers)
                .WithOne(u => u.Follower)
                .HasForeignKey(id => id.FollowerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Deck>()
                .HasMany(f => f.Flashcards)
                .WithOne(d => d.Deck)
                .OnDelete(DeleteBehavior.Cascade);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { optionsBuilder.UseLazyLoadingProxies(); }

    }
}
