﻿using Api.DataAccessLayer.Statuses;

namespace Api.DataAccessLayer.Models
{
    /// <summary>
    /// Purely a class inheriting from the Ride-class to distinguish between solo and shared rides. 
    /// </summary>
    /// <seealso cref="Api.DataAccessLayer.Models.Ride" />
    public class SoloRide : Ride
    {

        public SoloRide()
        {
            Status = RideStatus.WaitingForAccept;
        }
    }
}