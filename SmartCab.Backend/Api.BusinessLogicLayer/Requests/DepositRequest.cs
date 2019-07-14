using System.ComponentModel.DataAnnotations;

namespace Api.BusinessLogicLayer.Requests
{
    public class DepositRequest
    {
        [Required]
        [Range((double) Constants.MinDepositAmount, (double) Constants.MaxDepositAmount,
            ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public decimal Deposit { get; set; }
    }
}
