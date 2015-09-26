using Battle.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle.Domain.Abstract
{
    public interface IGamerRepository
    {
        IEnumerable<Gamer> Gamers { get; }

        void SaveGamer(Gamer gamer);

        Gamer DeleteGamer(int gamerId);
    }
}
