using IPCountryBlocker.Application.Features.Countries.Commands.Models;
using IPCountryBlocker.Domain.Models;

namespace IPCountryBlocker.Test.Helpers
{
    public class TestDataBuilder
    {
        public static BlockCountryCommand BuildValidBlockCountryCommand(string code = "US", string name = "United States")
        {
            return new BlockCountryCommand
            {
                Code = code,
                Name = name
            };
        }

        public static TemporalCountryBlockCommand BuildValidTemporalBlockCommand(
            string code = "GB",
            string name = "United Kingdom",
            int durationMinutes = 120)
        {
            return new TemporalCountryBlockCommand
            {
                Code = code,
                Name = name,
                Duration = durationMinutes
            };
        }

        public static Country BuildCountry(string code = "US", string name = "United States", bool isBlocked = true)
        {
            return new Country
            {
                Code = code,
                Name = name,
                IsBlocked = isBlocked
            };
        }

        public static PagedResult<Country> BuildPagedCountries(int count = 3, int totalCount = 0)
        {
            var countries = new List<Country>();
            for (int i = 0; i < count; i++)
            {
                countries.Add(new Country
                {
                    Code = $"C{i}",
                    Name = $"Country {i}",
                    IsBlocked = true
                });
            }

            return new PagedResult<Country>
            {
                Items = countries,
                TotalCount = totalCount > 0 ? totalCount : count
            };
        }
    }
}
