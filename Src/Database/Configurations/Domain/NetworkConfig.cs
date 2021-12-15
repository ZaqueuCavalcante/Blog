using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Domain
{
    public class NetworkConfig : IEntityTypeConfiguration<Network>
    {
        public void Configure(EntityTypeBuilder<Network> network)
        {
            network.ToTable("networks");

            network.HasKey(n => n.Id);

            network.Property(n => n.Name).IsRequired();
            network.Property(n => n.Uri).IsRequired();
        }
    }
}
