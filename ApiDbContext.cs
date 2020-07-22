using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace LibraryAPI
{
    public class ApiDbContext : DbContext
    {
        #region Constructors
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
        #endregion

        #region Properties
        public DbSet<Book> Books { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RentEvent> RentEvents { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        #endregion
    }
}
