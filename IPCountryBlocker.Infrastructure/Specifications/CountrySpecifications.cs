using IPCountryBlocker.Domain.Models;

namespace IPCountryBlocker.Infrastructure.Specifications
{
    public class CountrySpecifications : BaseSpecification<Country>
    {
        public CountrySpecifications() : base(x => true)
        {
        }

        public CountrySpecifications(int pageNumber, int pageSize) : base(x => true)
        {
            ApplyPagination(pageSize, pageNumber);
        }

    }
}
