using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Api.BusinessLogicLayer.Requests
{
    public class DepositRequest
    {
        [Required]
        [Range(0, Int32.MaxValue,
            ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Deposit { get; set; }
    }
}
