using IoTDevicesAzureFunctions.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTDevicesAzureFunctions.DataContext
{
    public class CosmosDbContext : DbContext
    {
        public CosmosDbContext()
        {

        }
        public CosmosDbContext(DbContextOptions<CosmosDbContext> options) : base(options)
        {

        }
        public DbSet<DeviceToCloudMessage> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeviceToCloudMessage>(entity =>
            {
                entity.HasKey(k => k.DeviceId);
                entity.ToContainer("Messages");
                entity.HasPartitionKey(entity => entity.PartitionKey);
            });
        }
    }
}
