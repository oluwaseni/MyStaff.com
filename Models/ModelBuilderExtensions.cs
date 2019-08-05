using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder )
        {
            modelBuilder.Entity<Employee>().HasData(

              new Employee
              {
                  Id = 1,
                  Name = "Adeolu",
                  Email = "ade.olu@gmail.com",
                  Department = Dpt.HR
              },
               new Employee
               {
                   Id = 2,
                   Name = "Adedeji",
                   Email = "ade.deji@gmail.com",
                   Department = Dpt.IT
               }
              );
        }
    }
}
