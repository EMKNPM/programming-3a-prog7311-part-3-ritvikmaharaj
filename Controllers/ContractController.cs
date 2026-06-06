using Microsoft.AspNetCore.Mvc;
using PROGPOEP2.Models;
using PROGPOEP2.Services;

namespace PROGPOEP2.Controllers
{
    public class ContractController : Controller
    {
        private readonly ApiClientService _api; // dependency injection

        public ContractController(ApiClientService api)
        {
            _api = api;
        }

        
        [HttpGet]
        public async Task<IActionResult> Create()  // display form
        {
            var clients = await _api.GetClients();
            ViewBag.Clients = clients ?? new List<Client>();

            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(Contract contract, IFormFile file)
        {
            await _api.CreateContract(contract);
            return RedirectToAction("Search");
        }

        
        [HttpGet]
        public IActionResult Search()  // contract search page
        {
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> Search(string start, string end, string status)
        {
            DateTime? startDate = DateTime.TryParse(start, out var s) ? s : null;
            DateTime? endDate = DateTime.TryParse(end, out var e) ? e : null;

            ContractStatus? statusEnum = null;      // checks 

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (int.TryParse(status, out int statusInt))
                {
                    statusEnum = (ContractStatus)statusInt;
                }
            }

            var contracts = await _api.SearchContracts(startDate, endDate, statusEnum);

            return View("SearchResults", contracts);
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var contracts = await _api.GetContracts();

            return Content($"Contracts found: {contracts.Count}");
        }


    }
}