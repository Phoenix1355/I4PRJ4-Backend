using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DataAccessLayer.Interfaces;
using Backend.DataAccessLayer.Models;

namespace Backend.DataAccessLayer.Repositories
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
            var rides = await Task.Run(() => _context.Rides.ToList());
            return rides;
        }

        public async Task<Ride> GetRideByIdAsync(int id)
        {
            var ride = await Task.Run(() => _context.Rides.SingleOrDefault(r => r.Id == id));
            return ride;
        }

        public async Task<Ride> AddRideAsync(Ride ride)
        {
            return await Task.Run(() =>
            {
                _context.Rides.Add(ride);
                _context.SaveChanges();
                return ride;
            });
        }

        public async Task<Ride> UpdateRideAsync(Ride ride)
        {
            return await Task.Run(() =>
            {
                _context.Rides.Update(ride);
                _context.SaveChanges();
                return ride;
            });
        }

        public async Task<Ride> DeleteRideAsync(int id)
        {
            return await Task.Run(() =>
            {
                var ride = _context.Rides.SingleOrDefault(r => r.Id == id);
                _context.Rides.Remove(ride);
                _context.SaveChanges();
                return ride;
            });
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