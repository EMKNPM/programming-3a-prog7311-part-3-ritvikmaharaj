using Microsoft.AspNetCore.Mvc;
using PROGPOEP2.Models;
using PROGPOEP2.Services;

namespace PROGPOEP2.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly ApiClientService _api;

        public ServiceRequestController(ApiClientService api)
        {
            _api = api;
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var contracts = await _api.GetContracts();

            ViewBag.Contracts = contracts;

            return View(new ServiceRequest());
        }


        [HttpPost]
        public async Task<IActionResult> Create(ServiceRequest request, decimal usdAmount)
        {
            if (request == null || request.ContractId == 0)
            {
                return BadRequest("Invalid service request");
            }

            await _api.CreateServiceRequest(request, usdAmount);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Index()
        {
            var requests = await _api.GetServiceRequests();
            return View(requests);
        }
    }
}