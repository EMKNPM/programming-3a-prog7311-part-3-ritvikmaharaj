using Microsoft.EntityFrameworkCore;
using GLMS.API.Data;
using GLMS.API.Models;


namespace GLMS.API.Services
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
            if (file != null)
            {
                var fileName = await _fileService.SavePdfAsync(file);
                contract.FilePath = fileName;
            }

            contract.Status = ContractStatus.Draft;

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Contract>> GetAllContracts()
        {
            return await _context.Contracts
                .Include(c => c.Client)
                .ToListAsync();
        }

        public async Task<Contract?> GetContractById(int id)
        {
            return await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateContract(Contract contract)
        {
            _context.Contracts.Update(contract);
            await _context.SaveChangesAsync();
        }
    }
    }

