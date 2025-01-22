using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mocab.Web.Services.Data.Entities;

namespace Mocab.Web.Services.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<User>(entity =>
            //{
            //    entity.Property(e => e.Initials).HasMaxLength(5);
            //});

            builder.Entity<User>().Property(e => e.Initials).HasMaxLength(5);
            builder.HasDefaultSchema("identity");
        }
    }
}
