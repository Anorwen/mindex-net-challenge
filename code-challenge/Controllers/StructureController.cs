using challenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace challenge.Controllers
{
    [Route("api/structure")]
    public class StructureController : Controller
    {

        private readonly ILogger _logger;
        private readonly IStructureService _structureService;


        public StructureController(ILogger<StructureController> logger, IStructureService structureService)
        {
            _logger = logger;
            _structureService = structureService;
        }

        [HttpGet("{id}", Name = "getStructureById")]
        public IActionResult GetStructureById(String id)
        {
            _logger.LogDebug($"Received structure get request for '{id}'");

            var structure = _structureService.GetById(id);

            if (structure == null)
                return NotFound();

            return Ok(structure);
        }
    }
}
