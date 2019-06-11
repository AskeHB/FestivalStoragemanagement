using Bits.model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarnivalStoragemanagement.Data
{
    public class CarnivalStoregemanagementDbContext : DbContext
    {
        public CarnivalStoregemanagementDbContext() : base("CSDB")
        {

        }

        public DbSet<Item> Items { get; set; }

        public DbSet<Loan> Loans { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

    }
}
