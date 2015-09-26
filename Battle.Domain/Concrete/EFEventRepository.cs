using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle.Domain.Abstract;

namespace Battle.Domain.Concrete
{
    public class EFEventRepository : IEventRepository
    {
        private readonly Entities context = new Entities();

        public IEnumerable<Ev> events
        {
            get { return context.Evs; }
        }

        public void SaveEvent(Ev ev)
        {
            if (ev.id == 0)
            {
                context.Evs.Add(ev);
            }
            else
            {
                Ev dbEntry = context.Evs.Find(ev.id);
                if (dbEntry != null)
                {
                    dbEntry.message = ev.message;
                    dbEntry.ridcountry = ev.ridcountry;
                    dbEntry.ridgamer = ev.ridgamer;
                    dbEntry.turn = ev.turn;
                    dbEntry.UserId = ev.UserId;
                }
            }
            context.SaveChanges();
        }

        public Ev DeleteEvent(int EventId)
        {
            Ev dbEntry = context.Evs.Find(EventId);
            if (dbEntry != null)
            {
                context.Evs.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
