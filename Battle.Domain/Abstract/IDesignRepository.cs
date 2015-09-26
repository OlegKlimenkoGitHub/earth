using Battle.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle.Domain.Abstract
{
    public interface IDesignRepository
    {
        IEnumerable<Design> Designs { get; }

        void SaveDesign(Design design);

        Design DeleteDesign(int designId);
    }
}
