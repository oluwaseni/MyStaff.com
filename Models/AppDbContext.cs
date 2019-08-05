using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{

    //For this class to behave like the DbContext class, we make it derive from the DbContext class provided
    //by EntityFrameWorkCore
    public class AppDbContext : DbContext
    {
        //For the DbContext class to be able to do any ASP work, it needs an instance of the DbContext options class.
        //This DbContext options class carries the configurations info such as the connection string to use, the database provider to use etc.
        //The easiest way to do that is by creating a constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //The next thing we do is to set AppDbContext properties for each TYPE that we have in our project


        }
        public DbSet<Employee> Employees { get; set; } //As we use this, it will be translated into SQL queries in the database

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }
    }
}
