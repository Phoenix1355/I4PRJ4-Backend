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
            _context = (ApplicationContext)context; //If we don't make this cast, we wont have access to eg. SaveChanges.
        }


        public async Task<List<Ride>> GetAllRidesAsync()
        {
            var rides = await Task.Run(() => _context.Rides.ToList());

            return rides;
        }


        public async Task AddRide(Ride ride)
        {
            await Task.Run(() =>
            {
                _context.Rides.Add(ride);
                _context.SaveChanges();
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