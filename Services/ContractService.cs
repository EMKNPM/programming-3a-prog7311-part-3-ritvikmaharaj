using Microsoft.EntityFrameworkCore;
using PROGPOEP2.Data;
using PROGPOEP2.Models;


namespace PROGPOEP2.Services
{
    public class ContractService
    {
        
            private readonly LogisticsDbContext _context;
            private readonly FileService _fileService;
           

        public ContractService(LogisticsDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
         
        }

        // search contract method
        public async Task<List<Contract>> SearchContracts(DateTime? start, DateTime? end, ContractStatus? status) //parameters
        {
            var query = _context.Contracts.Include(contract => contract.Client).AsQueryable();

            // if statement to check conditions
            if (start.HasValue)
            {
                query = query.Where(contract => contract.ContractStartDate >= start.Value);
            }

            if (end.HasValue)
            {
                query = query.Where(contract => contract.ContractEndDate <= end.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(contract => contract.Status == status.Value);
            }

            return await query.ToListAsync(); //return results
        }

        public async Task CreateContract(Contract contract, IFormFile file)
        {
            var fileName = await _fileService.SavePdfAsync(file);

            contract.FilePath = fileName;

            contract.Status = ContractStatus.Draft;

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();
        }
    }
    }

