using Battle.Domain.Abstract;
using Battle.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Battle.WebUI.Models
{
    public class Game
    {
        public int idMenu { get; set; }
        public Country selectedCountry { get; set; }

        public Game() { 
        }

    }
}