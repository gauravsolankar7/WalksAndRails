using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WalksAndRails.Api.Data
{
    public class AppDbAuthContext : IdentityDbContext
    {
        public AppDbAuthContext(DbContextOptions<AppDbAuthContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "a74446bd-87d7-42ef-b287-2c55d85d72f1";
            var writerRoleId = "b811593a-d3e5-49d1-b55f-39aef1d67622";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                     Id = readerRoleId,
                     ConcurrencyStamp = readerRoleId,
                     Name = "Reader",
                     NormalizedName = "READER".ToUpper()
                },
                new IdentityRole
                {
                     Id = writerRoleId,
                     ConcurrencyStamp = writerRoleId,
                     Name = "Writer",
                     NormalizedName = "Writer".ToUpper()
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
