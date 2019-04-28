using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;
using Api.DataAccessLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IUoW _unitOfWork;

        public OrderRepository(IUoW unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}