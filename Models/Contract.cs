namespace PROGPOEP2.Models
{
    public class Contract
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public ContractStatus Status { get; set; }

        public DateTime ContractStartDate { get; set; }

        public DateTime ContractEndDate { get; set; }

        public string ServiceLevel { get; set; }

        public string FilePath { get; set; }

        public Client Client { get; set; }

        public ICollection<ServiceRequest> ServiceRequests { get; set; }
    }

    public enum ContractStatus
    {
        Draft,
        Active,
        Expired,
        OnHold
    }
}