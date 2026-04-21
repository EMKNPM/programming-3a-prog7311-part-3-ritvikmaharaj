using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROGPOEP2.Data;
using PROGPOEP2.Models;
using PROGPOEP2.Services;

namespace PROGPOEP2.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly ServiceRequestService _service;
        private readonly LogisticsDbContext _context;

        public ServiceRequestController(ServiceRequestService service, LogisticsDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {

            ViewBag.Contracts = _context.Contracts.Include(c => c.Client).ToList();
            return View();

        }

        public IActionResult Index()
        {

            var requests = _context.ServiceRequests .Include(r => r.Contract) .ThenInclude(contract => contract.Client) .ToList();
            return View(requests);

        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceRequest request, decimal usdAmount)
        {

            await _service.CreateServiceRequest(request, usdAmount);
            return RedirectToAction("Index");

        }
    }
}