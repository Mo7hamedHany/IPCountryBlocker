namespace IPCountryBlocker.Domain.Models
{
    public class IpGeolocation
    {
        public string Ip { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Isp { get; set; }
        public string Org { get; set; }
    }
}
