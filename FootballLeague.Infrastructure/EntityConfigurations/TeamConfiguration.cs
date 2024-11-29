using FootballLeague.Domain.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballLeague.Infrastructure.EntityConfigurations
{
    internal class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(TeamConstraint.NameMaxLength);

            builder
                .Property(x => x.DisplayName)
                .IsRequired()
                .HasMaxLength(TeamConstraint.DisplayNameMaxLength);

            builder
                .HasIndex(x => x.Name)
                .IsUnique();

            builder
                .HasMany(x => x.Matches)
                .WithOne(x => x.Team1)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.Team1Id)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}