using GLMS.API.Data;
using GLMS.API.Models;
using GLMS.API.Services;


namespace GLMS.API.Services
{
    public class ServiceRequestService
  
    {
        private readonly LogisticsDbContext _context;
        private readonly CurrencyService _currencyService;

        public ServiceRequestService(LogisticsDbContext context, CurrencyService currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        // checking request 
        public async Task CreateServiceRequest(ServiceRequest request, decimal usdAmount)
        {
            var contract = await _context.Contracts.FindAsync(request.ContractId);

            if (contract == null)
            {
                throw new Exception("Contract not found");
            }

            if (contract.Status == ContractStatus.Expired || contract.Status == ContractStatus.OnHold)
            {
                throw new Exception("Contract is not active");
            }

            var rate = await _currencyService.GetRate();

            request.Cost = usdAmount * rate;
           
            request.Status = "Pending";

            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();
        }


    }
}