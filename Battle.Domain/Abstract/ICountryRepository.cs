using Battle.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle.Domain.Abstract
{
    public interface ICountryRepository
    {
        IEnumerable<Country> Countries { get; }

        void SaveCountry(Country country);

        void UpdateCountry(String sQuery);

        Country DeleteCountry(int countryId);
    }
}
