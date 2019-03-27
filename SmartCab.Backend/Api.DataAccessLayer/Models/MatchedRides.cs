using System.Collections.Generic;

namespace Api.DataAccessLayer.Models
{
    /// <summary>
    /// An accepted shared ride. 
    /// </summary>
    public class MatchedRides
    {
        public int Id { get; set; }

        public virtual List<SharedOpenRide> SharedOpenRides { get; set; }
    }
}