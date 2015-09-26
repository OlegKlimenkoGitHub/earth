using Battle.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle.Domain.Abstract
{
    public interface IArmyRepository
    {
        IEnumerable<Army> Armies { get; }

        void SaveArmy(Army army);

        Army DeleteArmy(int armyId);

    }
}
