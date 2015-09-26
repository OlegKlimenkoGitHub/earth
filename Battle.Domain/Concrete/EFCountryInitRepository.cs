using Battle.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle.Domain.Concrete
{
    public class EFCountryInitRepository : ICountryInitRepository
    {
        private readonly Entities context = new Entities();

        public IEnumerable<CountryInit> InitCountries
        {
            get { return context.CountryInits; }
        }

        public void SaveCountry(CountryInit initCountry)
        {
            if (initCountry.rid == 0)
            {
                context.CountryInits.Add(initCountry);
            }
            else
            {
                CountryInit dbEntry = context.CountryInits.Find(initCountry.rid);
                if (dbEntry != null)
                {
                    dbEntry.cod = initCountry.cod;
                    dbEntry.color = initCountry.color;
                    dbEntry.id = initCountry.id;
                    dbEntry.mineral = initCountry.mineral;
                    dbEntry.nameEng = initCountry.nameEng;
                    dbEntry.nameRus = initCountry.nameRus;
                    dbEntry.population = initCountry.population;
                    dbEntry.populationreal = initCountry.populationreal;
                    dbEntry.ridgamer = initCountry.ridgamer;
                }
            }
            context.SaveChanges();
            
        }

        public CountryInit DeleteCountry(int countryId)
        {
            CountryInit dbEntry = context.CountryInits.Find(countryId);
            if (dbEntry != null)
            {
                context.CountryInits.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
