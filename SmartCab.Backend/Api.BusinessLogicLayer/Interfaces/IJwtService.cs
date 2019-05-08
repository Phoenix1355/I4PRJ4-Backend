namespace Api.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Defines a number of methods used to generate Json Web Tokens
    /// </summary>
    public interface IJwtService
    {
        string GenerateJwtToken(string userId, string email, string role);
    }
}