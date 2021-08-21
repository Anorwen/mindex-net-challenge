using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            Employee target = _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
            LoadDirectReports(target);
            return target;
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        private void LoadDirectReports(Employee employee)
        {
            if(employee == null)
                return;

            _employeeContext.Entry(employee).Collection(e => e.DirectReports).Load();
            if(employee.DirectReports != null)
            {
                foreach(Employee underling in employee.DirectReports)
                {
                    LoadDirectReports(underling);
                }
            }
        }
    }
}
