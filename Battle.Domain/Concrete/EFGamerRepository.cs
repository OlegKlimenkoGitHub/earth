using Battle.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle.Domain.Concrete
{
    public class EFGamerRepository : IGamerRepository
    {
        private readonly Entities context = new Entities();

        public IEnumerable<Gamer> Gamers
        {
            get { return context.Gamers; }
        }

        public void SaveGamer(Gamer gamer)
        {
            if (gamer.rid == 0)
            {
                context.Gamers.Add(gamer);
            }
            else
            {
                Gamer dbEntry = context.Gamers.Find(gamer.rid);
                if (dbEntry != null)
                {
                    dbEntry.Armies = gamer.Armies;
                    dbEntry.color = gamer.color;
                    dbEntry.Countries = gamer.Countries;
                    dbEntry.Designs = gamer.Designs;
                    dbEntry.id = gamer.id;
                    dbEntry.name = gamer.name;
                    dbEntry.status = gamer.status;
                    dbEntry.type = gamer.type;
                }
            }
            context.SaveChanges();
        }

        public Gamer DeleteGamer(int gamerId)
        {
            Gamer dbEntry = context.Gamers.Find(gamerId);
            if (dbEntry != null)
            {
                context.Gamers.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }


    }
}
