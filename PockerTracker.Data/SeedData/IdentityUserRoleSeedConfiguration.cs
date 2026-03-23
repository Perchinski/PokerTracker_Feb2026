using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.Data.SeedData
{
    public class IdentityUserRoleSeedConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
            {
                RoleId = SeedConstants.AdminRoleId,
                UserId = SeedConstants.AdminOneId
            },
                new IdentityUserRole<string>
            {
                RoleId = SeedConstants.UserRoleId,
                UserId = "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d"
            },
                new IdentityUserRole<string>
            {
                RoleId = SeedConstants.UserRoleId,
                UserId = "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f"
            });
        }
    }
}
