using challenge.Models;
using System;

namespace challenge.Services
{
    public interface IStructureService
    {
        ReportingStructure GetById(String id);
    }
}
