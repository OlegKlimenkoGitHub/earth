using Battle.Domain.Abstract;
using Battle.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Battle.WebUI.Models;
using WebMatrix.WebData;

namespace Battle.WebUI.Controllers
{

   [Authorize(Roles = "Gamer")]
    public class CountryController : Controller
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ICountryRepository countryRepository;
        private readonly IGamerRepository gamerRepository;

        public CountryController(ICountryRepository countryRepo, IGamerRepository gamerRepo) {
            logger.Info("CountryController: CountryController");
            countryRepository = countryRepo;
            gamerRepository = gamerRepo;
        }

        public ActionResult CountryInfo()
        {
            logger.Info("CountryController: CountryInfo");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            int userId = WebSecurity.GetUserId(User.Identity.Name);
            Gamer gamer = gamerRepository.Gamers.FirstOrDefault(x => x.type == "human" && x.UserId == userId);

            Game game = Session["Game"] as Game;
            if (game == null) {
                game = new Game();
                game.selectedCountry = countryRepository.Countries.Where(x => x.ridgamer == gamer.rid && x.UserId == userId).FirstOrDefault();
            }
            Country country = game.selectedCountry;
            if (country == null)
            {
                country = countryRepository.Countries.Where(x => x.ridgamer == gamer.rid && x.UserId == userId).FirstOrDefault();
            }

            return View(country);
        }
    }
}
