using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Battle.Domain.Abstract;
using Battle.WebUI.Models;
using WebMatrix.WebData;
using Battle.Domain.Concrete;

namespace Battle.WebUI.Controllers
{
    public class ReportController : Controller
    {
        readonly private IEventRepository eventRepository;
        readonly private ICountryRepository countryRepository;
        readonly private IGamerRepository gamerRepository;

        public ReportController(IEventRepository evRepo, ICountryRepository countryRepo, IGamerRepository gamerRepo) {
            eventRepository = evRepo;
            countryRepository = countryRepo;
            gamerRepository = gamerRepo;
        }

        public ActionResult Report()
        {
            int userId = WebSecurity.GetUserId(User.Identity.Name);
            Journal journal = new Journal(eventRepository, userId);
            Game game = Session["Game"] as Game;
            if (game == null) {
                game = new Game();
                Gamer gamer = gamerRepository.Gamers.FirstOrDefault(x => x.type == "human" && x.UserId == userId);
                game.selectedCountry = countryRepository.Countries.FirstOrDefault(x => x.ridgamer == gamer.rid && x.UserId == userId);
            }
            Country country = game.selectedCountry;
            Entities entities = new Entities();
            List<Ev> events = entities.GetReportForCountry(userId, country.rid).ToList();
            return View(events);
        }

        //[HttpPost]
        //public ActionResult Report(Country country) {
        //    int userId = WebSecurity.GetUserId(User.Identity.Name);
        //    Journal journal = new Journal(eventRepository, userId);
        //    Entities entities = new Entities();
        //    entities.GetReportForCountry(userId, country.rid);
        //    List<Ev> events = entities.GetReportForCountry(userId, country.rid).ToList();
        //    return View(events);
        //}
    }
}
