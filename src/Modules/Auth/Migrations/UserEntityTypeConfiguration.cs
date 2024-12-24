using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Models;
using NetDream.Modules.Auth.Repositories;
using System;

namespace NetDream.Modules.Auth.Migrations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("User");
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
            builder.Property(table => table.Email).HasColumnName("email").HasMaxLength(200).HasDefaultValue(string.Empty);
            builder.Property(table => table.Mobile).HasColumnName("mobile").HasMaxLength(20).HasDefaultValue(string.Empty);
            builder.Property(table => table.Password).HasColumnName("password").HasMaxLength(100);
            builder.Property(table => table.Sex).HasColumnName("sex").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.Avatar).HasColumnName("avatar")
                .HasDefaultValue(string.Empty);
            builder.Property(table => table.Birthday).HasColumnName("birthday").HasDefaultValue(DateTime.Now.ToString("yyyy-MM-DD"));
            builder.Property(table => table.Money).HasColumnName("money").HasDefaultValue(0);
            builder.Property(table => table.Credits).HasColumnName("credits").HasDefaultValue(0).HasComment("»ý·Ö");
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.Token).HasColumnName("token").HasMaxLength(60).HasDefaultValue(0);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(UserRepository.STATUS_ACTIVE);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
