using Battle.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle.Domain.Concrete
{
    public class EFArmyRepository : IArmyRepository
    {
        private readonly Entities context = new Entities();

        public IEnumerable<Army> Armies
        {
            get { return context.Armies; }
        }

        public void SaveArmy(Army army)
        {
            if (army.rid == 0)
            {
                context.Armies.Add(army);
            }
            else
            {
                Army dbEntry = context.Armies.Find(army.rid);
                if (dbEntry != null)
                {
                    dbEntry.name = army.name;
                    dbEntry.ridcountry = army.ridcountry;
                    dbEntry.ridgamer = army.ridgamer;
                }
            }
            context.SaveChanges();
        }

        public Army DeleteArmy(int armyId)
        {
            Army dbEntry = context.Armies.Find(armyId);
            if (dbEntry != null)
            {
                context.Armies.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

    }
}
