using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartCabPoC.DataLayer.Abstractions;
using SmartCabPoC.DataLayer.Models;

namespace SmartCabPoC.DataLayer.Repositories
{
    public class RideRepository : IRideRepository, IDisposable
    {
        private readonly SmartCabContext _context;

        public RideRepository(ISmartCabContext context)
        {
            _context = (SmartCabContext)context;
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