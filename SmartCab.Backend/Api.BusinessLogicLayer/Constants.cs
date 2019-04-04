namespace Api.BusinessLogicLayer
{
    public static class Constants
    {
        //============================ Related to data annotations ====================================
        public const string EmailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        public const string EmailRegexErrorMessage = "The provided email was not valid.";

        public const string PhoneNumberRegex = "^[1-9][0-9]{7}$";
        public const string PhoneNumberRegexErrorMessage = "The phone number must consist of exactly 8 numbers and cannot start with 0.";


        //============================ Related to JWT claims ==========================================
        public const string UserIdClaim = "UserId";
    }
}