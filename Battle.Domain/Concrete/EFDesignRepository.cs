using Battle.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle.Domain.Concrete
{
    public class EFDesignRepository : IDesignRepository
    {
        private readonly Entities context = new Entities();

        public IEnumerable<Design> Designs
        {
            get { return context.Designs; }
        }

        public void SaveDesign(Design design)
        {
            if (design.rid == 0)
            {
                context.Designs.Add(design);
            }
            else
            {
                Design dbEntry = context.Designs.Find(design.rid);
                if (dbEntry != null)
                {
                    dbEntry.caliber = design.caliber;
                    dbEntry.guns = design.guns;
                    dbEntry.id = design.id;
                    dbEntry.name = design.name;
                    dbEntry.ridgamer = design.ridgamer;
                    dbEntry.shield = design.shield;
                    dbEntry.weight = design.weight;
                }
            }
            context.SaveChanges();
        }

        public Design DeleteDesign(int designId)
        {
            Design dbEntry = context.Designs.Find(designId);
            if (dbEntry != null)
            {
                context.Designs.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

    }
}
