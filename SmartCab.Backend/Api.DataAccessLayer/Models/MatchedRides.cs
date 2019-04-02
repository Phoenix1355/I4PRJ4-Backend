using System.Collections.Generic;
using Api.DataAccessLayer.Statuses;

namespace Api.DataAccessLayer.Models
{
    /// <summary>
    /// An accepted shared ride. 
    /// </summary>
    public class MatchedRides
    {
        public int Id { get; set; }

        public virtual List<SharedOpenRide> SharedOpenRides { get; set; }

        public RideStatus RideStatus { get; set; }
    }
}