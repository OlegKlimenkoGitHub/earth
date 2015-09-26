using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Battle.WebUI.Models;
using Battle.Domain.Abstract;
using WebMatrix.WebData;
using Battle.Domain.Concrete;
using Microsoft.AspNet.SignalR;
using RealTimeProgressBar;

namespace Battle.WebUI.Controllers
{

    [System.Web.Mvc.Authorize(Roles = "Gamer")]
//    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class GameController : Controller
    {
        readonly private ICountryRepository countryRepository;
        readonly private IGamerRepository gamerRepository;
        readonly private IDesignRepository designRepository;
        readonly private IArmyRepository armyRepository;
        readonly private IBattlemechRepository battlemechRepository;
        readonly private ICountryInitRepository countryInitRepository;
        readonly private IEventRepository eventRepository;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // Создать палитру цветов, которые будут назначены игрокам
        private readonly string[] palette = {
                    "#000000","#000033","#000066","#000099","#0000cc","#0000ff","#003300","#003333","#003366","#003399",
                    "#0033cc","#0033ff","#006600","#006633","#006666","#006699","#0066cc","#0066ff","#009900","#009933",
                    "#009966","#009999","#0099cc","#0099ff","#00cc00","#00cc33","#00cc66","#00cc99","#00cccc","#00ccff",
                    "#00ff00","#00ff33","#00ff66","#00ff99","#00ffcc","#00ffff","#330000","#330033","#330066","#330099",
                    "#3300cc","#3300ff","#333300","#333333","#333366","#333399","#3333cc","#3333ff","#336600","#336633",
                    "#336666","#336699","#3366cc","#3366ff","#339900","#339933","#339966","#339999","#3399cc","#3399ff",
                    "#33cc00","#33cc33","#33cc66","#33cc99","#33cccc","#33ccff","#33ff00","#33ff33","#33ff66","#33ff99",
                    "#33ffcc","#33ffff","#660000","#660033","#660066","#660099","#6600cc","#6600ff","#663300","#663333",
                    "#663366","#663399","#6633cc","#6633ff","#666600","#666633","#666666","#666699","#6666cc","#6666ff",
                    "#669900","#669933","#669966","#669999","#6699cc","#6699ff","#66cc00","#66cc33","#66cc66","#66cc99",
                    "#66cccc","#66ccff","#66ff00","#66ff33","#66ff66","#66ff99","#66ffcc","#66ffff","#990000","#990033",
                    "#990066","#990099","#9900cc","#9900ff","#993300","#993333","#993366","#993399","#9933cc","#9933ff",
                    "#996600","#996633","#996666","#996699","#9966cc","#9966ff","#999900","#999933","#999966","#999999",
                    "#9999cc","#9999ff","#99cc00","#99cc33","#99cc66","#99cc99","#99cccc","#99ccff","#99ff00","#99ff33",
                    "#99ff66","#99ff99","#99ffcc","#99ffff","#cc0000","#cc0033","#cc0066","#cc0099","#cc00cc","#cc00ff",
                    "#cc3300","#cc3333","#cc3366","#cc3399","#cc33cc","#cc33ff","#cc6600","#cc6633","#cc6666","#cc6699",
                    "#cc66cc","#cc66ff","#cc9900","#cc9933","#cc9966","#cc9999","#cc99cc","#cc99ff","#cccc00","#cccc33",
                    "#cccc66","#cccc99","#cccccc","#ccccff","#ccff00","#ccff33","#ccff66","#ccff99","#ccffcc","#ccffff",
                };

        public GameController(ICountryRepository countryRepo, IGamerRepository gamerRepo, IDesignRepository designRepo,
                              IArmyRepository armyRepo, IBattlemechRepository battlemechRepo, ICountryInitRepository countryInitRepo,
                              IEventRepository eventRepo)
        {
            logger.Info("GameController: GameController");
            countryRepository = countryRepo;
            gamerRepository = gamerRepo;
            designRepository = designRepo;
            armyRepository = armyRepo;
            battlemechRepository = battlemechRepo;
            countryInitRepository = countryInitRepo;
            eventRepository = eventRepo;
        }

        public ActionResult NewGame()
        {
            logger.Info("GameController: NewGame");
            int userId = WebSecurity.GetUserId(User.Identity.Name);

            // Очистить таблицы
            logger.Info("Запускаем очистку таблиц");
            Entities entities = new Entities();
            entities.ClearUserTables(userId);
            logger.Info("Таблицы очищены");

            logger.Info("Новая игра");

            // Скопировать страны из CountryInit в Country с привязкой к новому игроку
            int i = 1;
            foreach (var item in countryInitRepository.InitCountries.ToList())
            {
                Country country = new Country();
                country.id = item.id;
                country.cod = item.cod;
                country.nameRus = item.nameRus.Trim();
                country.nameEng = item.nameEng.Trim();
                country.color = item.color;
                country.population = item.population;
                country.mineral = 1000;
                country.populationreal = item.populationreal;
                country.UserId = userId;
                countryRepository.SaveCountry(country);
                ProgressHub.SendMessage("Creating the game world", i);
                i++;
            }

            // Создание противников
            // Завести игрока - человека
            Gamer gamer = new Gamer
            {
                name = User.Identity.Name,
                color = "red",
                type = "human",
                status = "alive",
                UserId = userId
            };
            gamerRepository.SaveGamer(gamer);

            // Привязать игроку-человеку случайную страну
            Random rn = new Random();
            int idCountry = rn.Next(176); // 176 - количество стран
            Country humanCountry = countryRepository.Countries.Where(x => x.id == idCountry && x.UserId == userId).FirstOrDefault();
            humanCountry.ridgamer = gamer.rid;
            countryRepository.SaveCountry(humanCountry);

            // Создать игроков - компьютеров
            i = 0;
            foreach (Country country in countryRepository.Countries.Where(x => x.id != idCountry && x.UserId == userId).ToList()) { 
                // Создать нового компьютерного игрока
                Gamer compGamer = new Gamer
                {
                    name = ("Boss of " + country.nameEng).Trim(),
                    color = palette[i],
                    type = "computer",
                    status = "alive",
                    UserId = userId
                };
                gamerRepository.SaveGamer(compGamer);

                // Привязать новому игроку страну
                country.ridgamer = compGamer.rid;
                countryRepository.SaveCountry(country);

                ProgressHub.SendMessage("Creating enemies ", i);
                logger.Info("Создание противников: " + i.ToString());
                i++;
            }

            Game game = new Game();
            game.selectedCountry = humanCountry;
            game.idMenu = 1;
            Session["Game"] = game;

            //return RedirectToAction("Index");
            return View();
        }

        public ActionResult Index()
        {
            logger.Info("GameController: Index");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Game game = Session["Game"] as Game;
            if (game == null) { 
                game = new Game();
                int userId = WebSecurity.GetUserId(User.Identity.Name);
                Gamer gamer = gamerRepository.Gamers.FirstOrDefault(x => x.type == "human" && x.UserId == userId);
                game.selectedCountry = countryRepository.Countries.FirstOrDefault(x => x.UserId == userId && x.ridgamer == gamer.rid);
                game.idMenu = 1;
                Session["Game"] = game;
            }
            return View(1);
        }

        public ActionResult ChooseMenu(int? idMenu) {
            logger.Info("GameController: ChooseMenu");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (idMenu == null) { idMenu = 1; }
            return View("Index", idMenu);
        }

        public ActionResult NextTurn() {
            logger.Info("GameController: NextTurn");
            logger.Info("==== NEXT TURN ====");

            Entities entities = new Entities();
            
            int userId = WebSecurity.GetUserId(User.Identity.Name);
            Gamer gamer = gamerRepository.Gamers.FirstOrDefault(x => x.type == "human" && x.UserId == userId);

            entities.ClearUsersEvents(userId); // Очистить журнал - данные хранятся только за последний ход
            Journal journal = new Journal(eventRepository, userId);

            Random rn = new Random();

            // Пройтись по всем странам и увеличить запасы руды 
            entities.IncreaseMinerals(userId);
            logger.Info("Увеличили запасы руды во всех странах");

            logger.Info("== Строим мехов ==");
            // Выбрать все страны принадлежащие компьютерным игрокам
            List<Country> countryCompList = entities.GetCompCountries(userId).ToList();

            int k = 0;
            foreach (var country in countryCompList)
            {
                logger.Info("Страна: " + country.nameRus);
                // Сгенерировать случайным образом дизайн мехов для компов
                int minerals = 100; //(int)country.mineral; // ресурс руды по умолчанию
                //int qnt = rn.Next(1, 3); // Количество мехов. 
                int qnt = 1;
                int res = (int)(minerals / qnt); // Ресурс на одного меха
                int c = rn.Next((int)(res / 2), res); // Калибр
                int g = rn.Next(1, (int)(res / c)); // Количество пушек
                int s = res - g * c; // броня
                int w = s + g * c; // вес 
                int totalWeight = qnt * w; // вес всей партии
                string nameMech = "Shield:" + s.ToString() + ",Guns:" + g.ToString() + ",Caliber:" + c.ToString();
                Design design = new Design
                {
                    name = nameMech,
                    shield = s,
                    guns = g,
                    caliber = c,
                    ridgamer = country.ridgamer,
                    weight = s + g * c,
                    UserId = userId
                };
                designRepository.SaveDesign(design);

                // Проверить, есть ли у хозяина территории в этой стране армия
                //Army army = armyRepository.Armies.FirstOrDefault(x => x.ridcountry == country.rid && x.ridgamer == country.ridgamer && x.UserId == userId);
                Army army = entities.GetArmyInCountryForGamer(userId, country.rid, country.ridgamer).FirstOrDefault();

                // Если армии нет - создать
                if (army == null)
                {
                    //Gamer compGamer = gamerRepository.Gamers.FirstOrDefault(x => x.rid == country.ridgamer && x.UserId == userId);
                    Gamer compGamer = entities.GetGamerByRid(userId, country.ridgamer).FirstOrDefault();
                    army = new Army
                    {
                        name = compGamer.name.Trim() + " in " + country.nameEng.Trim(),
                        ridcountry = country.rid,
                        ridgamer = country.ridgamer,
                        UserId = userId
                    };
                    armyRepository.SaveArmy(army);
                }

                // Производим мехов. Увеличиваем количество мехов в таблице Battlemech	
                for (int qi = 1; qi <= qnt; qi++)
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
                    logger.Info("Построили " + battlemech.name);
                    journal.AddEvent(country, "Build " + battlemech.name);
                }

                // уменьшаем ресурсы в текущей стране
                country.mineral = country.mineral - totalWeight;
                countryRepository.SaveCountry(country);

                ProgressHub.SendMessage("Countries establish new mechanisms", k);
                k++;
            }

            // Двигаем армии управляемые компьютерными игроками
            logger.Info("== Двигаем армии ==");
            k = 0;
            int maxAttacers = 5; // Максимальное количество атакующих стран
            int countOfAttacers = 0; // Количество стран выбранных для атаки
            List<Country> ListOfTargets = new List<Country>();
            foreach (var country in countryCompList)
            {
                if (countOfAttacers == maxAttacers) { break; } // Прекращаем поиск

                // Случайно решить будет ли комп двигать мехов
                int move = rn.Next(1, 3); // 2 - не двигает, 1 - двигает
                if (move == 2)
                { // Эта армия остается дома
                    continue;
                }

                countOfAttacers++;

                // Если двигает, случайно решить в какую страну
                //int idTargetCountry = rn.Next(176) + 1; // 176 - количество стран
                int idTargetCountry = 1;
                if (country.id < 176) { idTargetCountry = (int)country.id + 1; }
                
                //Country countryTarget = countryRepository.Countries.FirstOrDefault(x => x.id == idTargetCountry && x.UserId == userId);
                Country countryTarget = entities.GetCountryById(userId, idTargetCountry).FirstOrDefault();
               
                if (countryTarget == null)
                {
                    logger.Info("Target country is not defined id = " + idTargetCountry.ToString());
                }
                logger.Info(country.nameEng + ": Move the army to " + countryTarget.nameEng);
                journal.AddEvent(country, country.nameEng + " move the army to " + countryTarget.nameEng);
                journal.AddEvent(countryTarget, country.nameEng + " move the army to " + countryTarget.nameEng);

                ListOfTargets.Add(countryTarget);

                // Определить армию из которой будут браться мехи
                //Army army = armyRepository.Armies.FirstOrDefault(x => x.ridcountry == country.rid && x.ridgamer == country.ridgamer && x.UserId == userId);
                Army army = entities.GetArmyInCountryForGamer(userId, country.rid, country.ridgamer).FirstOrDefault();

                // Проверить, есть ли у игрока армия в той стране куда планируется выступать
                //Army armyTarget = armyRepository.Armies.FirstOrDefault(x => x.ridcountry == countryTarget.rid && x.ridgamer == army.ridgamer && x.UserId == userId);
                Army armyTarget = entities.GetArmyInCountryForGamer(userId, countryTarget.rid, army.ridgamer).FirstOrDefault();

                // Если армии нет - создать
                if (armyTarget == null)
                {
                    //Gamer compGamer = gamerRepository.Gamers.FirstOrDefault(x => x.rid == country.ridgamer && x.UserId == userId);
                    Gamer compGamer = entities.GetGamerByRid(userId, country.ridgamer).FirstOrDefault();
                    armyTarget = new Army
                    {  
                        name = (compGamer.name + " in " + countryTarget.nameEng).Trim(),
                        ridcountry = countryTarget.rid,
                        ridgamer = army.ridgamer,
                        UserId = userId
                    };
                    armyRepository.SaveArmy(armyTarget);
                }

                // Перемещаем мехов из армии (army) в армию в указанной стране (armyTarget)
                //var mechsList = battlemechRepository.Battlemechs.Where(x => x.ridarmy == army.rid && x.UserId == userId).ToList();
                //foreach (var mech in mechsList)
                //{
                //    mech.ridarmy = armyTarget.rid;
                //    battlemechRepository.SaveBattlemech(mech);
                //}
                entities.MoveMechs(userId, army.rid, armyTarget.rid);

                ProgressHub.SendMessage("The movement of armies", k);
                k++;
            }

            // Проанализировать Страны и Армии, если несколько армий оказались в одной стране устроить сражение.
            logger.Info("== Обработка сражений ==");
            List<Country> countryList = entities.GetAllCountries(userId).ToList();
            //var countryList = countryRepository.Countries.Where(x => x.UserId == userId).ToList();
            k = 0;
            int ccount = 0;
            //foreach (var country in ListOfTargets)
            foreach (var country in countryList)
            {
                ccount++;
                ProgressHub.SendMessage("The battle for " + country.nameEng, ccount);
                logger.Info("Страна: " + country.nameRus);
                List<Army> listOfArmies = entities.GetReadyArmiesInCountry(userId, country.rid).ToList();
                //List<Army> listOfArmies = entities.GetArmiesInCountry(userId, country.rid).ToList();
                //List<Army> listOfArmies = armyRepository.Armies.Where(x => x.ridcountry == country.rid && x.UserId == userId).ToList();
                if (listOfArmies.Count < 2)
                { // Сражаться некому
                    logger.Info(country.nameRus + ": Сражаться некому");
                    continue;
                }
                logger.Info(country.nameRus + ": Битва!!! " + listOfArmies.Count.ToString() + " армий:");
                journal.AddEvent(country, "The battle for " + country.nameEng + " - " + listOfArmies.Count.ToString() + " armies:");
                foreach (var army in listOfArmies)
                {
                    logger.Info(army.name);
                    journal.AddEvent(country, army.name);
                    List<Battlemech> lbs = entities.GetMechsInArmy(userId, army.rid).ToList();
                    foreach (Battlemech lb in lbs) {
                        journal.AddEvent(country, lb.name);
                    }
                }
                Boolean endBattle = false;

                int maxturn = 0;
                //TODO: Переписать этот foreach через sql-запрос
                foreach (var army in listOfArmies)
                {
                    //List<Battlemech> mechs = battlemechRepository.Battlemechs.Where(x => x.ridarmy == army.rid && x.UserId == userId).ToList();
                    List<Battlemech> mechs = entities.GetMechsInArmy(userId, army.rid).ToList();
                    int qnt = mechs.Count();
                    if (qnt > maxturn) { maxturn = qnt; } // Определить максимальное число мехов, столько и будет залпов
                }
                logger.Info("Число залпов: " + maxturn.ToString());

                int hour = 0;
                int turn = 0;
                int leave = 0;
                do
                {
                    foreach (var army in listOfArmies)
                    {
                        // Получить список мехов в армии
                        //List<Battlemech> mechs = battlemechRepository.Battlemechs.Where(x => x.ridarmy == army.rid && x.UserId == userId).ToList();
                        List<Battlemech> mechs = entities.GetMechsInArmy(userId, army.rid).ToList();
                        logger.Info("Ход армии " + army.name);
                        if (mechs.Count <= 0)
                        { // В армии нет мехов
                            logger.Info("В армии " + army.name + " нет мехов");
                            continue;
                        }

                        if (turn > mechs.Count - 1)
                        { // У этой армии мехи в этом ходу уже отстрелялись
                            logger.Info("Армия " + army.name + " все мехи отстрелялись");
                            continue;
                        }
                        Battlemech mech = mechs[turn]; // Взять из списка мехов с номером turn
                        logger.Info("Стреляет " + mech.name);

                        // Выбрать случайно армию по которой будет стрелять
                        Army targetArmy = null;
                        if (listOfArmies.Count >= 2) {
                            int i = 0;
                            do
                            {
                                i++;
                                int target = rn.Next(listOfArmies.Count);
                                //List<Battlemech> mechsOfArmy = battlemechRepository.Battlemechs.Where(x => x.ridarmy == listOfArmies[target].rid && x.UserId == userId).ToList();
                                List<Battlemech> mechsOfArmy = entities.GetMechsInArmy(userId, listOfArmies[target].rid).ToList();
                                if (mechsOfArmy.Count <= 0) { continue; } // У этих не осталось мехов
                                if (listOfArmies[target].rid != army.rid) { targetArmy = listOfArmies[target]; break; }
                                if (i > listOfArmies.Count * 2) { break; } // Не могу прицелиться
                            } while (true);
                        }

                        if (targetArmy == null)
                        { // не могу прицелиться
                            logger.Info("Не может прицелиться.");
                            endBattle = true; break;
                        }
                        logger.Info("Огонь по армии " + targetArmy.name);

                        // Отстреляться (mech) по вражеской армии (targetArmy)
                        // Определить сколько у нашего меха пушек и какого калибра
                        Design designAttacker = designRepository.Designs.FirstOrDefault(x => x.rid == mech.riddesign && x.UserId == userId);

                        // Огонь из всех орудий!
                        for (int i = 1; i <= designAttacker.guns; i++)
                        {
                            // получить список живых мехов вражеской армии
                            //List<Battlemech> listOfTargets = battlemechRepository.Battlemechs.Where(x => x.ridarmy == targetArmy.rid && x.qnt > 0 && x.UserId == userId).ToList();
                            //Значение qnt не играет роли, т.к. я удаляю записи убитых мехов из базы 
                            List<Battlemech> listOfTargets = entities.GetMechsInArmy(userId, targetArmy.rid).ToList();
                            if (listOfTargets.Count == 0) { break; } // цели кончились
                            int target = rn.Next(listOfTargets.Count);
                            logger.Info(mech.name + " стреляет по " + listOfTargets[target].name);
                            journal.AddEvent(country, army.name + " " + mech.name + " fire to " + targetArmy.name + " " + listOfTargets[target].name);
                            Design designTarget = designRepository.Designs.FirstOrDefault(x => x.rid == listOfTargets[target].riddesign && x.UserId == userId);
                            if (designAttacker.caliber >= designTarget.shield)
                            {
                                //TODO: Проверить как отрабатывает удаление, если указан rid а не id
                                logger.Info(listOfTargets[target].name + " - УНИЧТОЖЕН !!!");
                                journal.AddEvent(country, listOfTargets[target].name + " - DESTROYED!!!");
                                battlemechRepository.DeleteBattlemech(listOfTargets[target].rid);  // Цель уничтожена!
                            }
                            else
                            {
                                // Броня не пробиваема!
                                logger.Info(mech.name + " не может пробить броню у " + listOfTargets[target].name);
                            }
                        }
                    }
                    turn++;
                    hour++;
                    if (turn == maxturn)
                    { // все отстрелялись, начинаем сначала
                        turn = 0;
                        logger.Info("все отстрелялись, начинаем сначала");
                    }
                    leave = 0; // Получить количество боеспособных армий
                    foreach (var army in listOfArmies)
                    {
                        //List<Battlemech> leaveMechs = battlemechRepository.Battlemechs.Where(x => x.ridarmy == army.rid && x.UserId == userId).ToList();
                        List<Battlemech> leaveMechs = entities.GetMechsInArmy(userId, army.rid).ToList();
                        if (leaveMechs.Count > 0) { leave++; }
                    }

                    if (leave < 2)
                    { // сражаться больше некому
                        endBattle = true;
                        logger.Info("сражаться больше некому");
                    }
                    if (hour == 100)
                    { // сутки закончились, никто не смог добиться успеха (взаимные непробивахи?)
                        logger.Info("сутки закончились, никто не смог добиться успеха (взаимные непробивахи?)");
                        endBattle = true;
                    }
                } while (!endBattle);

                logger.Info("Битва закончилась!!!");
                journal.AddEvent(country, "End of the battle.");

                // Проверить не поменялся ли у страны хозяин
                if (leave < 2)
                {
                    Army armyOfVictory = null;
                    foreach (var army in listOfArmies)
                    {
                        //List<Battlemech> leaveMechs = battlemechRepository.Battlemechs.Where(x => x.ridarmy == army.rid && x.UserId == userId).ToList();
                        List<Battlemech> leaveMechs = entities.GetMechsInArmy(userId, army.rid).ToList();
                        if (leaveMechs.Count > 0)
                        {   // Армия победитель
                            armyOfVictory = army;
                            logger.Info("Армия победитель: " + army.name);
                            journal.AddEvent(country, "WINNER!!! - " + army.name);
                            // Поменяем в стране хозяина
                            country.ridgamer = army.ridgamer;
                            countryRepository.SaveCountry(country);
                            logger.Info("Победитель: " + army.name);
                        }
                    }
                    if (armyOfVictory == null)
                    {
                        logger.Info(country.nameRus + ": Победитель не определен, armyOfVictory == null");
                        journal.AddEvent(country, "NO WINNER!");
                    }
                }
                else
                {
                    logger.Info(country.nameRus + ": Победитель не определен, живых армий >= 2");
                    journal.AddEvent(country, "NO WINNER!");
                }

                k++;
            }

            ModelState.Clear();
            return View();
        }
    }
}
