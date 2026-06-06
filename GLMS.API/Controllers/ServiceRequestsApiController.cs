using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GLMS.API.Data;
using GLMS.API.Models;
using GLMS.API.Services;

namespace GLMS.API.Controllers
{
    [ApiController]

    [Route("api/servicerequests")]

    public class ServiceRequestsApiController : ControllerBase
    {
        private readonly ServiceRequestService _service;   // dependancies
        private readonly LogisticsDbContext _context;

        public ServiceRequestsApiController(   // constructor
            ServiceRequestService service,
            LogisticsDbContext context)
        {
            _service = service;
            _context = context;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var requests = _context.ServiceRequests
                .Include(r => r.Contract)
                .ThenInclude(c => c.Client)
                .ToList();

            return Ok(requests);
        }


        [HttpPost]
            public async Task<IActionResult> Create(
            [FromBody] ServiceRequest request,
            [FromQuery] decimal usdAmount)
              {
                try
                 {
                    if (request == null)
                    return BadRequest("Request body is null");

                    if (request.ContractId <= 0)
                    return BadRequest("Invalid ContractId");

                    await _service.CreateServiceRequest(request, usdAmount);

                    return Created("", request);
                 }

                catch (Exception ex)

                 {
               
                return BadRequest(new
                {

                    message = ex.Message,
                    detail = ex.InnerException?.Message

                });
            }
        }
    }
}