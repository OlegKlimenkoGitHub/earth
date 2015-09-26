using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Battle.WebUI.Models;
using Battle.Domain.Abstract;
using Battle.Domain.Concrete;
using WebMatrix.WebData;

namespace Battle.WebUI.Controllers
{

    [Authorize(Roles = "Gamer")]
    public class BunkerController : Controller
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ICountryRepository countryRepository;
        private readonly IBattlemechRepository battlemechRepository;
        private readonly IArmyRepository armyRepository;
        private readonly IGamerRepository gamerRepository;

        public BunkerController(ICountryRepository countryRepo, IBattlemechRepository battlemechRepo, IArmyRepository armyRepo,
                                IGamerRepository gamerRepo) {
            logger.Info("BunkerController: BunkerController");
            countryRepository = countryRepo;
            battlemechRepository = battlemechRepo;
            armyRepository = armyRepo;
            gamerRepository = gamerRepo;
        }

        public ActionResult Bunker()
        {
            logger.Info("BunkerController: Bunker");
            int userId = WebSecurity.GetUserId(User.Identity.Name);
            Gamer gamer = gamerRepository.Gamers.FirstOrDefault(x => x.type == "human" && x.UserId == userId);

            Game game = Session["Game"] as Game;
            Country country = game.selectedCountry;

            Bunker bunker = new Bunker();
            // Выбрать все страны кроме выделенной
            bunker.listOfCountries = countryRepository.Countries.Where(x => x.rid != country.rid && x.UserId == userId).ToList();
            // Выбрать всех мехов, что сидят в этой стране
            bunker.listOfMechs = armyRepository.Armies.Where(x => x.ridcountry == country.rid && x.ridgamer == gamer.rid && x.UserId == userId)
                .Join(battlemechRepository.Battlemechs.Where(x => x.UserId == userId), a => a.rid, b => b.ridarmy, (a, b) => b).ToList();

            return View(bunker);
        }

        [HttpPost]
        public ActionResult Bunker(int? sendAllTo, List<int?> sendOneTo){
            logger.Info("BunkerController: SendMechs");
            Game game = Session["Game"] as Game;
            int userId = WebSecurity.GetUserId(User.Identity.Name);
            Gamer gamer = gamerRepository.Gamers.FirstOrDefault(x => x.type == "human" && x.UserId == userId);

            // Выбрать всех мехов, что сидят в этой стране
            List<Battlemech> listOfMechs = armyRepository.Armies.Where(x => x.ridcountry == game.selectedCountry.rid && x.ridgamer == gamer.rid && x.UserId == userId)
                .Join(battlemechRepository.Battlemechs.Where(x => x.UserId == userId), a => a.rid, b => b.ridarmy, (a, b) => b).ToList();

            if (sendAllTo != null)
            {
                // Проверить, есть ли у игрока армия в стране куда отправляются мехи
                Country toCountry = countryRepository.Countries.FirstOrDefault(x => x.rid == sendAllTo && x.UserId == userId);
                Army toArmy = armyRepository.Armies.FirstOrDefault(x => x.ridcountry == toCountry.rid && x.ridgamer == gamer.rid && x.UserId == userId);

                // Если армии нет - создать

                if (toArmy == null)
                {
                    toArmy = new Army
                    {
                        name = (gamer.name + " in " + toCountry.nameEng).Trim(),
                        ridcountry = toCountry.rid,
                        //ridgamer = country.ridgamer,
                        ridgamer = gamer.rid,
                        UserId = userId
                    };
                    armyRepository.SaveArmy(toArmy);
                }
                
                // Отправить всех мехов по указанному адресу
                foreach (Battlemech mech in listOfMechs)
                {
                    mech.ridarmy = toArmy.rid;
                    battlemechRepository.SaveBattlemech(mech);
                }
            }
            else
            {
                int i = -1;
                foreach (var sendTo in sendOneTo)
                {
                    i++;
                    if (sendTo == null) { continue; } // Этого оставляем дома

                    // Остальных рассылаем согласно назначениям

                    // Проверить, есть ли у игрока армия в стране куда отправляются мехи
                    Country toCountry = countryRepository.Countries.FirstOrDefault(x => x.rid == sendTo && x.UserId == userId);
                    Army toArmy = armyRepository.Armies.FirstOrDefault(x => x.ridcountry == toCountry.rid && x.ridgamer == gamer.rid && x.UserId == userId);

                    // Если армии нет - создать
                    if (toArmy == null)
                    {
                        toArmy = new Army
                        {
                            name = (gamer.name + " in " + toCountry.nameEng).Trim(),
                            ridcountry = toCountry.rid,
                            //ridgamer = country.ridgamer,
                            ridgamer = gamer.rid,
                            UserId = userId
                        };
                        armyRepository.SaveArmy(toArmy);
                    }

                    // Отправить меха по указанному адресу
                    // TODO: Сомнительное решение - нужно переделать так, чтобы получать связку значений (rid-mech,rid-country)
                    listOfMechs[i].ridarmy = toArmy.rid;
                    battlemechRepository.SaveBattlemech(listOfMechs[i]);
                }
            }

            ModelState.Clear();
            Bunker bunker = new Bunker();
            // Выбрать все страны кроме выделенной
            bunker.listOfCountries = countryRepository.Countries.Where(x => x.rid != game.selectedCountry.rid && x.UserId == userId).ToList();
            // Выбрать всех мехов, что сидят в этой стране
            bunker.listOfMechs = armyRepository.Armies.Where(x => x.ridcountry == game.selectedCountry.rid && x.ridgamer == gamer.rid && x.UserId == userId)
                .Join(battlemechRepository.Battlemechs.Where(x => x.UserId == userId), a => a.rid, b => b.ridarmy, (a, b) => b).ToList();

            return View(bunker);
        }

    }
}
