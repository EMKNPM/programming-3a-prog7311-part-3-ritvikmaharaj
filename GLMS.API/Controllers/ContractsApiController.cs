using Microsoft.AspNetCore.Mvc;
using GLMS.API.Services;
using GLMS.API.Models;

namespace GLMS.API.Controllers
{
    [ApiController]

    [Route("api/contracts")]

    public class ContractsApiController : ControllerBase
    {
        private readonly ContractService _service;

        public ContractsApiController(ContractService service)
        {
            _service = service;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetContracts()
        {
            var result = await _service.GetAllContracts();
            return Ok(result);
        }

        
        // create contract 
        [HttpPost]
        public async Task<IActionResult> CreateContract([FromBody] Contract contract)
        {
            if (contract == null)
                return BadRequest("Contract cannot be null.");

            await _service.CreateContract(contract, null);

            return CreatedAtAction(nameof(GetContracts), new { id = contract.Id }, contract);
        }


        [HttpGet("search")]
              public async Task<IActionResult> Search(
              DateTime? start, // parameters
              DateTime? end,
              ContractStatus? status)
        {
            var result = await _service.SearchContracts(start, end, status); // service call
            return Ok(result);
        }


        // patch used to update contract status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] ContractStatus status)
        {
            var contract = await _service.GetContractById(id);

            if (contract == null)
                return NotFound();

            contract.Status = status;

            await _service.UpdateContract(contract);

            return Ok(contract);
        }
    }
}