using Battle.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle.Domain.Abstract
{
    public interface IBattlemechRepository
    {
        IEnumerable<Battlemech> Battlemechs { get; }

        void SaveBattlemech(Battlemech battlemech);

        Battlemech DeleteBattlemech(int battlemechId);
    }
}
