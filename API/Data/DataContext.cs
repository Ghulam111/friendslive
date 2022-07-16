using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);

                builder.Entity<UserLike>()
                .HasKey( k => new {k.LikedUserId, k.SourceUserId});

                builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany( l=> l.LikedUsers)
                .HasForeignKey(l => l.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<UserLike>()
                .HasOne(s => s.LikedUser)
                .WithMany( l=> l.LikedByUsers)
                .HasForeignKey(l => l.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<Message>()
                .HasOne( u => u.Recipient)
                .WithMany( l => l.MessageRecieved)
                .OnDelete(DeleteBehavior.Restrict);

                builder.Entity<Message>()
                .HasOne( u => u.Sender)
                .WithMany( l => l.MessageSent)
                .OnDelete(DeleteBehavior.Restrict);
                

            }

    }

  
}