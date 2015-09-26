using Battle.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Battle.Domain.Abstract;
using WebMatrix.WebData;
using Battle.Domain.Concrete;

namespace Battle.WebUI.Controllers
{

    [Authorize(Roles = "Gamer")]
    public class FactoryController : Controller
    {

        readonly private IDesignRepository designRepository;
        readonly private IGamerRepository gamerRepository;
        readonly private IArmyRepository armyRepository;
        readonly private ICountryRepository countryRepository;
        readonly private IBattlemechRepository battlemechRepository;

        public FactoryController(IDesignRepository designRepo, IGamerRepository gamerRepo, IArmyRepository armyRepo, 
                                 ICountryRepository countryRepo, IBattlemechRepository battlemechRepo) {
            designRepository = designRepo;
            gamerRepository = gamerRepo;
            armyRepository = armyRepo;
            countryRepository = countryRepo;
            battlemechRepository = battlemechRepo;
        }

        public ActionResult CreateMech()
        {
            int userId = WebSecurity.GetUserId(User.Identity.Name);
            Gamer gamer = gamerRepository.Gamers.FirstOrDefault(x => x.type == "human" && x.UserId == userId);
            Game game = Session["Game"] as Game;
            if (game == null) {
                game = new Game();
                game.selectedCountry = countryRepository.Countries.FirstOrDefault(x => x.ridgamer == gamer.rid && x.UserId == userId);
            }
            if (game.selectedCountry == null){
                game.selectedCountry = countryRepository.Countries.FirstOrDefault(x => x.ridgamer == gamer.rid && x.UserId == userId);
            }

            Country country = game.selectedCountry;
            Gamer owner = gamerRepository.Gamers.FirstOrDefault(x => x.rid == country.ridgamer && x.UserId == userId);

            Boolean our = false;
            if (gamer.rid == owner.rid) { our = true; }

            Factory factory = new Factory
            {
                designes = designRepository.Designs.Where(x => x.UserId == userId && x.ridgamer == gamer.rid).ToList(),
                qnt = 0,
                ours = our

            };
            return View(factory);
        }

        [HttpPost]
        public ActionResult CreateMech(int designesDropDown, int qnt) {
            int manufactureQnt = qnt;

            Game game = Session["Game"] as Game;
            Country country = game.selectedCountry;

            int userId = WebSecurity.GetUserId(User.Identity.Name);
            Gamer gamer = gamerRepository.Gamers.FirstOrDefault(x => x.type == "human" && x.UserId == userId);
            Gamer owner = gamerRepository.Gamers.FirstOrDefault(x => x.rid == country.ridgamer && x.UserId == userId);
            Boolean our = false;
            if (gamer.rid == owner.rid) { our = true; } 
            // Дизайн заказанный к производству
            Design design = designRepository.Designs.FirstOrDefault(x => x.rid == designesDropDown && x.UserId == userId);

            // Проверить есть ли у игрока-человека в этой стране армия
            Army army = armyRepository.Armies.FirstOrDefault(x => x.ridcountry == country.rid && x.ridgamer == gamer.rid && x.UserId == userId);
            // Если армии нет - создать
            if (army == null) {
                army = new Army
                {
                    name = (gamer.name + " in " + country.nameEng).Trim(),
                    ridcountry = country.rid,
                    //ridgamer = country.ridgamer,
                    ridgamer = gamer.rid,
                    UserId = userId
                };
                armyRepository.SaveArmy(army);
            }

            // увеличиваем количество мехов в таблице Battlemech	
            for (int qi = 1; qi <= manufactureQnt; qi++)
            {
                // Если хватает ресурсов
                if (country.mineral >= design.weight)
                {
                    Battlemech battlemech = new Battlemech
                    {
                        riddesign = design.rid,
                        ridarmy = army.rid,
                        name = "Mech-" + design.name,
                        qnt = 1,
                        UserId = userId
                    };
                    battlemechRepository.SaveBattlemech(battlemech);
                    country.mineral = country.mineral - design.weight;
                }
                else {
                    break;
                }
            }
            countryRepository.SaveCountry(country);

            ModelState.Clear();

            Factory factory = new Factory
            {
                designes = designRepository.Designs.Where(x => x.UserId == userId && x.ridgamer == gamer.rid).ToList(),
                qnt = 0,
                ours = our
            };

            return View(factory);
        }

    }
}
