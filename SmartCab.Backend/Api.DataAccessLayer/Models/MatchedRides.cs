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

        public virtual List<SharedRide> SharedOpenRides { get; set; }

        public Status Status { get; set; }
    }
}