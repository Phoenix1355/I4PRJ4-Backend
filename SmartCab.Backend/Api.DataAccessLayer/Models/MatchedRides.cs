using System.Collections.Generic;

namespace Api.DataAccessLayer.Models
{
    public class MatchedRides
    {
        public int Id { get; set; }

        public List<SharedOpenRide> SharedOpenRides { get; set; }
    }
}