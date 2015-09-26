using Battle.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle.Domain.Abstract
{
    public interface IEventRepository
    {
        IEnumerable<Ev> events { get; }

        void SaveEvent(Ev ev);

        Ev DeleteEvent(int evId);
    }
}
