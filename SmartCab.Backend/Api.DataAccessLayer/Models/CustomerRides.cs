namespace Api.DataAccessLayer.Models
{
    public class CustomerRides
    {
        public int Id { get; set; }
        
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public int RideId { get; set; }

        public virtual Ride Ride { get; set; }

        public int TaxiCompanyId { get; set; }

        public virtual TaxiCompany TaxiCompany { get; set; }

        public string status { get; set; }
    }
}