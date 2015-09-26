using Battle.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle.Domain.Abstract
{
    public interface ICountryInitRepository
    {
        IEnumerable<CountryInit> InitCountries { get; }

        void SaveCountry(CountryInit initCountry);

        CountryInit DeleteCountry(int countryId);

    }
}
