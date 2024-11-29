using FootballLeague.Domain.Matches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballLeague.Infrastructure.EntityConfigurations
{
    internal class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasKey(x => new { x.Team1Id, x.Team2Id, x.StartedAt });

            builder.HasIndex(x => x.Key);
            builder.HasIndex(x => x.StartedAt);

            builder
                .HasOne(x => x.Team2)
                .WithMany()
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.Team2Id)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}