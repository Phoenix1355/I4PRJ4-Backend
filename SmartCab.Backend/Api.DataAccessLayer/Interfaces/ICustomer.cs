using Api.DataAccessLayer.Models;

namespace Api.DataAccessLayer.Interfaces
{
    public interface ICustomer
    {
        int Id { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        string PhoneNumber { get; set; }
        string ApplicationUserId { get; set; }
        ApplicationUser ApplicationUser { get; set; }
    }
}