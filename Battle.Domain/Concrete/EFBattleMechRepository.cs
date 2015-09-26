using Battle.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle.Domain.Concrete
{
    public class EFBattleMechRepository : IBattlemechRepository
    {
        private readonly Entities context = new Entities();

        public IEnumerable<Battlemech> Battlemechs
        {
            get { return context.Battlemechs; }
        }

        public void SaveBattlemech(Battlemech battlemech)
        {
            if (battlemech.rid == 0)
            {
                context.Battlemechs.Add(battlemech);
            }
            else
            {
                Battlemech dbEntry = context.Battlemechs.Find(battlemech.rid);
                if (dbEntry != null)
                {
                    dbEntry.id = battlemech.id;
                    dbEntry.name = battlemech.name;
                    dbEntry.qnt = battlemech.qnt;
                    dbEntry.ridarmy = battlemech.ridarmy;
                    dbEntry.riddesign = battlemech.riddesign;
                }
            }
            context.SaveChanges();
        }

        public Battlemech DeleteBattlemech(int battleMechId)
        {
            Battlemech dbEntry = context.Battlemechs.Find(battleMechId);
            if (dbEntry != null)
            {
                context.Battlemechs.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

    }
}
