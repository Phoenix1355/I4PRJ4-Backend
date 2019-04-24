using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.Statuses;
using CustomExceptions;

namespace Api.DataAccessLayer.UnitOfWork
{
    public interface ICreateRideUOW
    {
        void ReservePriceFromCustomer(string customerId, decimal price);
        SoloRide AddRide(SoloRide ride);
        Order CreateOrder();
        Order AddRideToOrder(Ride ride, Order order);
        void SaveChanges();
    }
}
