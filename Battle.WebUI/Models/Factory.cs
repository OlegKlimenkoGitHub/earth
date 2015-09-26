using Battle.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Battle.WebUI.Models
{
    public class Factory
    {
        public List<Design> designes { get; set; }
        public int qnt { get; set; }
        public Boolean ours { get; set; }
    }
}