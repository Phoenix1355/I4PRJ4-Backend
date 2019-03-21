namespace Api.DataAccessLayer.Models
{
    public class CustomerRides
    {
        public int Id { get; set; }
        
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public int RideId { get; set; }

        public Ride Ride { get; set; }

        public int TaxiCompanyId { get; set; }

        public TaxiCompany TaxiCompany { get; set; }

        public string status { get; set; }
    }
}