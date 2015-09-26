using Battle.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Battle.Domain.Concrete
{
    public class EFCountryRepository : ICountryRepository
    {
        private readonly Entities context = new Entities();

        public IEnumerable<Country> Countries
        {
            get { return context.Countries; }
        }

        public void UpdateCountry(String sQuery)
        {
            int i = context.Database.ExecuteSqlCommand(sQuery);
        }

        public void SaveCountry(Country country)
        {
            if (country.rid == 0)
            {
                context.Countries.Add(country);
            }
            else
            {
                Country dbEntry = context.Countries.Find(country.rid);
                //Country dbEntry = context.Countries
                //                         .Where(x => x.rid == country.rid && x.UserId == country.UserId).FirstOrDefault();
                if (dbEntry != null)
                {
                    dbEntry.cod = country.cod;
                    dbEntry.color = country.color;
                    dbEntry.id = country.id;
                    dbEntry.mineral = country.mineral;
                    dbEntry.nameEng = country.nameEng;
                    dbEntry.nameRus = country.nameRus;
                    dbEntry.population = country.population;
                    dbEntry.populationreal = country.populationreal;
                    dbEntry.ridgamer = country.ridgamer;
                    dbEntry.UserId = country.UserId;
                    //context.Countries.Attach(country);
                    //context.Entry(country).State = EntityState.Modified;
                }

            }
            context.SaveChanges();
        }

        public Country DeleteCountry(int countryId)
        {
            Country dbEntry = context.Countries.Find(countryId);
            if (dbEntry != null)
            {
                context.Countries.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

    }
}
