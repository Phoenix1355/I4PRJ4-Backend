using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.UnitOfWork;
using Api.Requests;
using Api.Responses;
using AutoMapper;
using CustomExceptions;
using Microsoft.AspNetCore.Identity;

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// Exposes methods containing business logic related to customers.
    /// </summary>
    public class MatchService : IMatchService
    {
        /// <summary>
        /// Returns true if the ride starting and endpoint is within max distance km of each other, air flight. 
        /// </summary>
        /// <param name="ride1"></param>
        /// <param name="ride2"></param>
        /// <returns>Whether the rides is close enough</returns>
        public bool Match(Ride ride1, Ride ride2, int maxDistance)
        {
            //This is flight distance so should be taken with a gram of salt. 
            var distanceBetweenStartDestinationsInKm = DistanceBetweenCoordinates(ride1.StartDestination.Lat, ride1.StartDestination.Lng, ride2.StartDestination.Lat,
                ride2.StartDestination.Lng);
            var distanceBetweenEndDestinationsInKm = DistanceBetweenCoordinates(ride1.EndDestination.Lat, ride1.EndDestination.Lng, ride2.EndDestination.Lat,
                ride2.EndDestination.Lng);
            if (distanceBetweenEndDestinationsInKm < maxDistance && distanceBetweenStartDestinationsInKm < maxDistance)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Using Haversine function
        /// </summary>
        /// <param name="lat1">Latitude 1</param>
        /// <param name="lon1">Latitude 2</param>
        /// <param name="lat2">Longitude 2</param>
        /// <param name="lon2">Longitude 2</param>
        /// <returns>The distance in km between the points, airflight. </returns>
        private double DistanceBetweenCoordinates(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6372.8; // In kilometers
            var dLat = toRadians(lat2 - lat1);
            var dLon = toRadians(lon2 - lon1);
            lat1 = toRadians(lat1);
            lat2 = toRadians(lat2);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Asin(Math.Sqrt(a));
            return R * 2 * Math.Asin(Math.Sqrt(a));
        }

        /// <summary>
        /// Convert angle to radians. 
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private double toRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}