namespace Api.DataAccessLayer.Models
{
    /// <summary>
    /// Inheritase base properties from ride, foreign key to MatchedRide. Matched ride is nullable. 
    /// </summary>
    /// <seealso cref="Api.DataAccessLayer.Models.Ride" />
    public class SharedOpenRide : Ride
    {
     public virtual MatchedRides MatchedRides { get; set; }

     public int MatchedRidesId { get; set; }
    }
}