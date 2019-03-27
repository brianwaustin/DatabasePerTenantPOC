using DatabasePerTenantPOC.Data.CustomerDB;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DatabasePerTenantPOC.Data.TenantDB
{
    public class CustomerDbContext : DbContext
    {
        public virtual DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// Only purpose is to help EF Core create database tables code first
        /// </summary>
        /// <param name="options"></param>
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options) { }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="shardMap">Database shardmap</param>
        /// <param name="shardingKey">Shard key</param>
        /// <param name="connectionStr">Base connection string</param>
        public CustomerDbContext(ShardMap shardMap, int shardingKey, string connectionStr) :
            base(CreateDdrConnection(shardMap, shardingKey, connectionStr)) { }        

        /// <summary>
        /// Creates the DDR (Data Dependent Routing) connection.
        /// </summary>
        /// <param name="shardMap">The shard map.</param>
        /// <param name="shardingKey">The sharding key.</param>
        /// <param name="connectionStr">The connection string.</param>
        /// <returns></returns>
        private static DbContextOptions CreateDdrConnection(ShardMap shardMap, int shardingKey, string connectionStr)
        {
            // Ask shard map to broker a validated connection for the given key
            SqlConnection sqlConn = shardMap.OpenConnectionForKey(shardingKey, connectionStr);

            var optionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
            var options = optionsBuilder.UseSqlServer(sqlConn).Options;

            return options;
        }

        /// <summary>
        /// Model builder for context
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Customer__A4AE64D814038057");                

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(25);
                
            });
        }
    }
}
