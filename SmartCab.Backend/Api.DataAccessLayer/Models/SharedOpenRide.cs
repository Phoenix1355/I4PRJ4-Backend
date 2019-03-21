namespace Api.DataAccessLayer.Models
{
    public class SharedOpenRide : Ride
    {
     public MatchedRides MatchedRides { get; set; }

     public int MatchedRidesId { get; set; }
    }
}