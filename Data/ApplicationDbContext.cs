using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using tsuHelp.Models;

namespace tsuHelp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<UserSubjects> UserSubjects { get; set; }

        public DbSet<TagsInPost> TagsInPosts { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<UserChatHubConnection> UserChatHubConnections { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
            

        //    modelBuilder.Entity<Message>()
        //        .HasOne(c => c.Sender)
        //        .WithMany(c => c.SentMessages)
        //        .HasForeignKey(c => c.SenderId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity<Message>()
        //        .HasOne(c => c.Reciever)
        //        .WithMany(c => c.RecievedMessages)
        //        .HasForeignKey(c => c.RecieverId)
        //        .OnDelete(DeleteBehavior.Restrict);
        //}
    }
}
