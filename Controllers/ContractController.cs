using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROGPOEP2.Data;
using PROGPOEP2.Models;
using PROGPOEP2.Services;

namespace PROGPOEP2.Controllers
{
    public class ContractController : Controller
    {
        private readonly ContractService _contractService;
        private readonly LogisticsDbContext _context;
       

        public ContractController(ContractService contractService, LogisticsDbContext context)
        {
            _contractService = contractService;
            _context = context;
        }

       
        [HttpGet]
        public IActionResult Create()
        {

            ViewBag.Clients = _context.Clients.ToList();
            return View();

        }

   
        [HttpPost]
        public async Task<IActionResult> Create(Contract contract, IFormFile file)
        {

            await _contractService.CreateContract(contract, file);
            return RedirectToAction("Search");
        }

     
        [HttpGet]
        public IActionResult Search()
        {

            return View();

        }

       
        [HttpPost]
        public async Task<IActionResult> Search(DateTime start, DateTime end, ContractStatus status)

        {

          var results = await _context.Contracts .Include(contract => contract.Client)
         .Where(contract => contract.ContractStartDate >= start && contract.ContractEndDate <= end &&contract.Status == status).ToListAsync();

            return View("SearchResults", results);
        }
    }
}