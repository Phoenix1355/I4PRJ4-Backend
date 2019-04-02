using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Repositories
{
    /// <summary>
    /// This class exposes all the possible request to the database that is related to "Rides"
    /// </summary>
    public class RideRepository : IRideRepository, IDisposable
    {
        private readonly ApplicationContext _context;

        public RideRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Ride>> GetAllRidesAsync()
        {
            var rides = await _context.Rides.ToListAsync();
            return rides;
        }

        /// <summary>
        /// Returns all SoloRides with status WaitingForAccept
        /// </summary>
        /// <returns></returns>
        public Task<List<SoloRide>> GetOpenSoloRidesAsync()
        {
            var rides = _context.SoloRides
                .Where(x=>x.Status == Status.Expired) //TODO: Change this method
                .ToListAsync();
            return rides;
        }

        public async Task<Ride> GetRideByIdAsync(int id)
        {
            var ride = await _context.Rides.SingleOrDefaultAsync(r => r.Id == id);
            return ride;
        }

        public async Task<Ride> AddRideAsync(Ride ride)
        {
            await _context.Rides.AddAsync(ride);
            await _context.SaveChangesAsync();
            return ride;
        }

        public async Task<Ride> UpdateRideAsync(Ride ride)
        {
            _context.Rides.Update(ride);
            await _context.SaveChangesAsync();
            return ride;
        }

        public async Task<Ride> DeleteRideAsync(int id)
        {
            var ride = await _context.Rides.SingleOrDefaultAsync(r => r.Id == id);
            _context.Rides.Remove(ride);
            await _context.SaveChangesAsync();
            return ride;
        }

        #region IDisposable implementation

        //Dispose pattern:
        //https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose#basic_pattern
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _context?.Dispose();
        }

        #endregion
    }
}