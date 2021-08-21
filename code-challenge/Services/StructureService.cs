using challenge.Models;
using challenge.Repositories;
using Microsoft.Extensions.Logging;
using System;

namespace challenge.Services
{
    public class StructureService : IStructureService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<StructureService> _logger;

        public StructureService(ILogger<StructureService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public ReportingStructure GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                _logger.LogDebug($"Generating reporting structure for employee '{id}'", id);
                Employee employee = _employeeRepository.GetById(id);
                if(employee != null)
                {
                    ReportingStructure structure = new ReportingStructure { Employee = employee };
                    return structure.RefreshStructure();
                }
            }
            _logger.LogDebug($"WARNING: Can't generate structure for employee with id '{id}'", id);
            return null;
        }
    }
}
