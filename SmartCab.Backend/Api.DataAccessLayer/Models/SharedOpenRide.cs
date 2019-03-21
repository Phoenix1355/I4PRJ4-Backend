namespace Api.DataAccessLayer.Models
{
    public class SharedOpenRide : Ride
    {
     public virtual MatchedRides MatchedRides { get; set; }

     public int MatchedRidesId { get; set; }
    }
}