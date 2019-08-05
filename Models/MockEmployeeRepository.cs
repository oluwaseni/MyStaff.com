using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {

        private List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee(){ Id = 1, Name = "Adekunle Timothy", Department = Dpt.HR, Email = "adekunletimothy@gmail.com" },
                new Employee(){ Id = 2, Name = "dekunle Timothy", Department = Dpt.IT, Email = "adekunletimothy@gmail.com" },
                new Employee(){ Id = 3, Name = "kunle Timothy", Department = Dpt.IT, Email = "adekunletimothy@gmail.com" }
            };
        }

        public Employee Delete(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == id);
            if(employee != null)
            {
                _employeeList.Remove(employee);
            }

            return employee;
        }



        //public Employee Add(Employee employee)
        //{

        //    employee.Id = _employeeList.Max(e => e.Id) + 1;
        //    _employeeList.Add(employee);
        //    return employee;
        //}

        public IEnumerable<Employee> GetAllEmployee()
        {

            return _employeeList;
        }

        public Employee GetEmployee(int Id)
        {
            //THis returns the employee whose Id represents the current Id requested
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }

        public Employee Post(Employee employee)
        {

            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }

            return employee;
        }
    }
}
