using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Battle.Domain.Abstract;
using Battle.Domain.Concrete;

namespace Battle.WebUI.Models
{
    public class Journal
    {
        readonly private IEventRepository evRepository;
        readonly private int userId;

        public Journal(IEventRepository evRepo, int userId) {
            evRepository = evRepo;
            this.userId = userId;
        }

        public void AddEvent(Country country, string mes) { 
            Ev ev = new Ev{
                message = mes,
                ridcountry = country.rid,
                ridgamer = null,
                turn = null,
                UserId = this.userId
            };
            evRepository.SaveEvent(ev);
        }
    }
}