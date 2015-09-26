using Battle.Domain.Abstract;
using Battle.Domain.Concrete;
using Battle.WebUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Battle.WebUI.Controllers
{
    [Authorize(Roles="Gamer")]
//    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class MapController : Controller
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ICountryInitRepository repository;
        private readonly ICountryRepository countryRepository;
        private readonly IGamerRepository gamerRepository;

        public MapController(ICountryInitRepository repo, ICountryRepository countryRepo, IGamerRepository gamerRepo)
        {
            logger.Info("Map: MapController");
            repository = repo;
            countryRepository = countryRepo;
            gamerRepository = gamerRepo;
        }

        public ViewResult Index()
        {
            logger.Info("Map: Index");
            return View();
        }

        [HttpGet]
        public JsonResult GetColors()
        {
            logger.Info("Map: GetColors");
            Entities entities = new Entities();
            var colorsList = entities.GetUserColors(WebSecurity.CurrentUserId).ToList();
            var jsonData = JsonConvert.SerializeObject(colorsList);
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangeCountry(string[] cod)
        {
            logger.Info("Map: ChangeCountry");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            int userId = WebSecurity.GetUserId(User.Identity.Name);
            Game game = Session["Game"] as Game;
            if (game == null) { 
                game = new Game(); 
                Gamer gamer = gamerRepository.Gamers.FirstOrDefault(x => x.type=="human" && x.UserId == userId);
                game.selectedCountry = countryRepository.Countries.FirstOrDefault(x => x.ridgamer == gamer.rid && x.UserId == userId);
            }

            Country country = countryRepository.Countries.FirstOrDefault(x => x.cod.Trim() == cod[0].Trim() && x.UserId == userId);
            if (country != null)
            {
                game.selectedCountry = country;
            }
            Session["Game"] = game;

            return RedirectToAction("Index", "Game");
        }


    }
}
