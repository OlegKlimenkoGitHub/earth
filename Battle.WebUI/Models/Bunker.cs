using Battle.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Battle.WebUI.Models
{
    public class Bunker
    {
        public List<Country> listOfCountries {get; set;}
        public List<Battlemech> listOfMechs { get; set; }

        public Bunker() { 
        }
    }
}