namespace Api.DataAccessLayer.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public int PostalCode { get; set; }
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }

        public Address(string cityName, int postalCode, string streetName, int streetNumber)
        {
            CityName = cityName;
            PostalCode = postalCode;
            StreetName = streetName;
            StreetNumber = streetNumber;
        }
    }
}