using Battle.Domain.Abstract;
using Battle.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Battle.WebUI.Models
{
    public class GameOld
    {
        private readonly ICountryRepository countryRepository;

        public CountryContext countryTable;
        public CountryInitContext countryInitTable;
        public GamerContext gamersTable;
        public DesignContext designTable;
        public ManufactureContext manufactureTable;
        public ArmyContext armyTable;
        public BattleMechContext battleMechsTable;
        //public JournalContext journalTable;

        public Gamer activeGamer { get; set; } // Игрок, который сейчас ходит
        public List<Gamer> gamers; // Массив игроков
        public String[] palette; // Цвета игроков

        public Design design;
        public Manufacture manufacture;
        public List<Design> designes; // Список дизайнов по умолчанию

        public Country selectedCountry { get; set; } // Страна, которую игрок выбрал в текущий момент
        public List<Country> countryList; // Список всех стран
        public List<Country> countryCompList; // Список всех стран под властью компов

        //public List<Battlemech> mechsInCurrentCountry; // Список механизмов в текущей стране
        //public Journal journal { get; set; } // Журнал событий
        private readonly ICountryInitRepository countryInitRepository;

        //public Entities entities;

        public GameOld() {
            //countryRepository = countryRepo;

            //Entities entities = new Entities();
            //countryInitRepository = new EFCountryInitRepository();
            //countryRepository = new EFCountryRepository();

            //countryTable = new CountryContext();
            //countryInitTable = new CountryInitContext();
            //gamersTable = new GamerContext();
            //designTable = new DesignContext();
            //manufactureTable = new ManufactureContext();
            //armyTable = new ArmyContext();
            //battleMechsTable = new BattleMechContext();
            //countryList = new List<Country>();
            //countryCompList = new List<Country>();
            //gamers = new List<Gamer>();
            //selectedCountry = new Country();
            //design = new Design();
            //manufacture = new Manufacture();

            //ClearTables();
            //palette = CreatePalette();
            //countryList = GetCountryList();
            //CreateGamers();
 
        }

        //public void GetMechanismesForCountry(Country country) {
        //    mechsInCurrentCountry = battleMechsTable.BattleMechs.SqlQuery(
        //        "Select * from BattleMechs INNER JOIN  Army ON Battlemechs.ridarmy = Army.rid " + 
        //        "INNER JOIN  Country ON Country.rid = Army.ridcountry WHERE Country.id = " + country.id.ToString() 

        //        ).ToList();
        //}

        //public void GetMechanismesForCountry(Country country)
        //{
        //    mechsInCurrentCountry = battleMechsTable.BattleMechs.SqlQuery(
        //        "Select * from BattleMechs INNER JOIN  Army ON Battlemechs.ridarmy = Army.rid " +
        //        "INNER JOIN  Country ON Country.rid = Army.ridcountry WHERE Country.id = " + country.id.ToString() +
        //        " and Army.ridgamer = Country.ridgamer"
        //        ).ToList();
        //}

        public List<Country> GetCountryList()
        {
            return countryTable.Countries.SqlQuery("Select * from Country where ridgamer is null").ToList();
        }

        public Country GetCountryById(int? id)
        {
            if (id == null) { return new Country(); }
            return countryTable.Countries.SqlQuery("Select * from Country where id = " + id.ToString()).FirstOrDefault();
        }

        public Country GetCountryByCod(string cod)
        {
            if (cod == null) { return new Country(); }
            return countryTable.Countries.SqlQuery("Select * from Country where cod = '" + cod.ToString() + "'").FirstOrDefault();
        }

        public List<Country> GetCompCountryList()
        {
            return countryTable.Countries.SqlQuery(
                "Select * from Country inner join Gamers on Country.ridgamer = Gamers.rid where Gamers.type = 'computer'").ToList();
        }

        public void IncreaseMinerals()
        {
            int i = countryTable.Database.ExecuteSqlCommand("Update Country set mineral = mineral + population ");
        }

        public void DecreaseMinerals(int w, Country country)
        {
            int i = countryTable.Database.ExecuteSqlCommand(
                "Update Country set mineral = mineral - " + w.ToString() + " where rid = " + country.rid.ToString());
        }

        public void ClearBattleMechs()
        {
            int i = battleMechsTable.Database.ExecuteSqlCommand(
                "Delete from Battlemechs where qnt = 0");
        }

        public void BindCountryToGamer(int ridgamer, int idCountry)
        {
            int i = countryTable.Database.ExecuteSqlCommand(
                "Update Country set ridgamer = '" + ridgamer.ToString() + "' where id=" + idCountry.ToString());
        }

        public Gamer GetGamerbyRid(int ridGamer)
        {
            return gamersTable.Gamers.SqlQuery(
                "Select * from Gamers where rid = " + ridGamer.ToString()).FirstOrDefault();
        }

        public int GetRidArmyForCountry(Country country)
        {
            // Проверить, есть ли армия у хозяина территории
            return armyTable.Database.SqlQuery<int>(
                    "Select rid from Army where ridcountry = " + country.rid.ToString() +
                                            " and ridgamer = " + country.ridgamer.ToString()).FirstOrDefault<int>();
        }

        public int GetRidArmyForGamerInCountry(Country country, int ridGamer)
        {
            // Проверить, есть ли армия у игрока на этой территории
            return armyTable.Database.SqlQuery<int>(
                    "Select rid from Army where ridcountry = " + country.rid.ToString() +
                                            " and ridgamer = " + ridGamer.ToString()).FirstOrDefault<int>();
        }

        // Создать палитру цветов, которые будут назначены игрокам
        public string[] CreatePalette()
        {
            string[] palette = {
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
            return palette;
        }

        // Очистить таблицы
        public void ClearTables()
        {
            int i;
            i = countryTable.Database.ExecuteSqlCommand("Update Country set ridgamer = NULL ");
            i = countryTable.Database.ExecuteSqlCommand("Update Country set mineral = 0 ");
            i = manufactureTable.Database.ExecuteSqlCommand("Delete from Manufacture");
            i = battleMechsTable.Database.ExecuteSqlCommand("Delete from Battlemechs");
            i = armyTable.Database.ExecuteSqlCommand("Delete from Army");
            i = designTable.Database.ExecuteSqlCommand("Delete from Design");
            i = gamersTable.Database.ExecuteSqlCommand("Delete from Gamers");
            //i = journalTable.Database.ExecuteSqlCommand("Delete from Journal");
        }

        // Вернуть игрока - человека (только для однопользовательского режима)
        public Gamer GetHuman()
        {
            return gamers.Where(x => x.type.Equals("human")).FirstOrDefault();
        }

        // Получить игрока по его rid
        public Gamer GetGamerByRid(int _rid)
        {
            return gamers.Where(x => x.rid.Equals(_rid)).FirstOrDefault();
        }

        // Инициировать массив дизайнов механизмов по умолчанию (Design)
        public void CreateDefaultDesignes(Gamer gamer)
        {
            designes = new List<Design>();
            designes.Add(new Design("Soldier", 0, 1, 1));
            designes[designes.Count - 1].AddRowInBase(gamer.rid);
            designes.Add(new Design("Sergant", 5, 4, 5));
            designes[designes.Count - 1].AddRowInBase(gamer.rid);
            designes.Add(new Design("Sniper", 0, 1, 100));
            designes[designes.Count - 1].AddRowInBase(gamer.rid);
        }

        // Инициировать массив игроков
        public void CreateGamers()
        {
            // Завести игрока - человека
            gamers.Add(new Gamer("GameMaster", "red", "human", "alive"));
            // Привязать ему случайную страну
            Random rn = new Random();
            int idCountry = rn.Next(176); // 176 - количество стран
            BindCountryToGamer(gamers[gamers.Count - 1].rid, idCountry);

            // Завести игроков компьютеров
            // Пройтись по странам и для каждой создать нового игрока
            foreach (var country in countryList)
            {
                if (country.id == idCountry)
                {
                    // Эта страна уже привязана за человеком
                    country.ridgamer = gamers[gamers.Count - 1].rid;
                    selectedCountry = country;
                    continue;
                }
                gamers.Add(new Gamer(palette[(int)country.id], "computer", "alive"));
                BindCountryToGamer(gamers[gamers.Count - 1].rid, (int)country.id);
                country.ridgamer = gamers[gamers.Count - 1].rid;
            }
        }

        public List<Gamer> GetGamers()
        {
            return gamersTable.Gamers.SqlQuery("Select * from Gamers").ToList();
        }

        public int CreateArmy(Country country, Gamer gamer)
        {
            string nameArmy = gamer.name + " in " + country.nameEng;
            Army army = new Army(nameArmy, country.rid, gamer.rid);
            return army.rid;
        }

        // Получить количество живых армий во время сражения
        public int GetQntLeaveArmies(List<Army> listOfArmies)
        {
            int leaveArmies = 0;
            foreach (var army in listOfArmies) { if (!army.IsEmpty()) { leaveArmies++; } }
            return leaveArmies;
        }

        // Обработать следующий ход
        public void NextTurn()
        {

            // Пройтись по всем странам и увеличить запасы руды 
            IncreaseMinerals();

            // Построить мехов на территории компов
            countryCompList = GetCompCountryList();
            foreach (var country in countryCompList)
            {
                // Сгенерировать случайным образом дизайн мехов для компов
                Random rn = new Random();
                int minerals = 100; //(int)country.mineral; // ресурс руды по умолчанию
                int qnt = rn.Next(1, 5); // Количество мехов. 
                int res = (int)(minerals / qnt); // Ресурс на одного меха
                int c = rn.Next(1, res); // Калибр
                int g = rn.Next(1, (int)(res / c)); // Количество пушек
                int s = res - g * c; // броня
                int w = s + g * c; // вес 
                int totalWeight = qnt * w; // вес всей партии
                string nameMech = "Shield:" + s.ToString() + ",Guns:" + g.ToString() + ",Caliber:" + c.ToString();
                Design design = new Design(nameMech, s, g, c);
                design.AddRowInBase((int)country.ridgamer);
                //journal.SetMessage(country.nameEng + ": Created design " + nameMech);

                // Проверить, есть ли у хозяина территории в этой стране армия
                int ridArmy = GetRidArmyForCountry(country);

                // Если армии нет - создать
                if (ridArmy == 0)
                {
                    Gamer gamer = GetGamerByRid((int)country.ridgamer);
                    ridArmy = CreateArmy(country, gamer);
                }

                // Производим мехов. Увеличиваем количество мехов в таблице Battlemech	
                for (int qi = 1; qi <= qnt; qi++)
                {
                    Battlemech battlemech = new Battlemech(design.rid, ridArmy);
                }
                //journal.SetMessage(country.nameEng + ": Created " + qnt.ToString() + " battlemechs");

                // уменьшаем ресурсы в текущей стране
                DecreaseMinerals(totalWeight, country);
            }

            // Узнать сколько стало механизмов в стране выбранной игроком
            // TODO: свойство List<Battlemech> mechsInCurrentCountry сделать свойством страны а не игры
            // GetMechanismesForCountry(selectedCountry);

            // Перебрать все страны управляемые компами
            foreach (var country in countryCompList)
            //foreach (var country in countryCompList.Where(x => x.id == 1))
            {
                // Случайно решить будет ли комп двигать мехов
                Random rn = new Random();
                int move = rn.Next(2); // 0 - не двигает, 1 - двигает
                if (move == 0)
                { // Эта армия остается дома
                    //journal.SetMessage(country.nameEng + ": Do not move the army");
                    continue;
                }

                // Если двигает, случайно решить в какую страну
                int idTargetCountry = rn.Next(176) + 1; // 176 - количество стран
                Country countryTarget = GetCountryById(idTargetCountry);
                if (countryTarget == null)
                {
                    //journal.SetMessage("Target country is not defined id = " + idTargetCountry.ToString());
                }
                //journal.SetMessage(country.nameEng + ": Move the army to " + countryTarget.nameEng);
                Army army = country.GetArmy();

                // Проверить, есть ли у игрока армия в той стране
                int ridArmy = GetRidArmyForGamerInCountry(countryTarget, (int)army.ridgamer);
                // Если армии нет - создать
                if (ridArmy == 0)
                {
                    ridArmy = CreateArmy(countryTarget, GetGamerbyRid((int)army.ridgamer));
                }
                // Перемещаем мехов из этой армии в армию в указанной стране
                army.MoveTo(countryTarget);
            }

            // Проанализировать Страны и Армии, если несколько армий оказались в одной стране устроить сражение.
            foreach (var country in countryList)
            {
                List<Army> listOfArmies = country.GetReadyArmies();
                if (listOfArmies.Count < 2)
                { // Сражаться некому
                    //journal.SetMessage(country.nameEng + ": Сражаться некому");
                    continue;
                }
                //journal.SetMessage(country.nameEng + ": Битва!!! " + listOfArmies.Count.ToString() + " армий:");
                foreach (var army in listOfArmies)
                {
                    //journal.SetMessage(army.name);
                }
                Boolean endBattle = false;

                int maxturn = 0;
                foreach (var army in listOfArmies)
                {
                    int qnt = army.GetQntMechs();
                    if (qnt > maxturn) { maxturn = qnt; } // Определить максимальное число мехов, столько и будет залпов
                }
                //journal.SetMessage("Число залпов: " + maxturn.ToString());

                int hour = 0;
                int turn = 0;
                int leave = 0;
                do
                {
                    foreach (var army in listOfArmies)
                    {
                        //journal.SetMessage("Ход армии " + army.name);
                        if (army.IsEmpty())
                        { // В армии нет мехов
                            //journal.SetMessage("В армии " + army.name + " нет мехов");
                            continue;
                        }
                        if (turn > army.GetBattleMechs().Count - 1)
                        { // У этой армии мехи в этом ходу уже отстрелялись
                            //journal.SetMessage("Армия " + army.name + " все мехи отстрелялись");
                            continue;
                        }
                        Battlemech mech = army.GetBattleMechs()[turn]; // Взять из списка мехов с номером turn
                        //journal.SetMessage("Стреляет " + mech.name);
                        Army targetArmy = army.GetTargetArmy(listOfArmies); // Выбрать случайно армию по которой будет стрелять

                        if (targetArmy == null)
                        { // не могу прицелиться
                            //journal.SetMessage("Не может прицелиться.");
                            endBattle = true; break;
                        }
                        //journal.SetMessage("Огонь по армии " + targetArmy.name);
                        mech.FireTo(targetArmy); // Отстреляться по мехам выбранной армии
                        // Убитому меху обнуляем количество, если его убили перед тем как успел выстрелить - селяви
                    }
                    turn++;
                    hour++;
                    if (turn == maxturn)
                    { // все отстрелялись, начинаем сначала
                        turn = 0;
                        //journal.SetMessage("все отстрелялись, начинаем сначала");
                    }
                    leave = GetQntLeaveArmies(listOfArmies); // Получить количество боеспособных армий
                    if (leave < 2)
                    { // сражаться больше некому
                        endBattle = true;
                        //journal.SetMessage("сражаться больше некому");
                    }
                    if (hour == 100)
                    { // сутки закончились, никто не смог добиться успеха (взаимные непробивахи?)
                        //journal.SetMessage("сутки закончились, никто не смог добиться успеха (взаимные непробивахи?)");
                        endBattle = true;
                    }
                } while (!endBattle);

                //journal.SetMessage("Битва закончилась!!!");

                // Проверить не поменялся ли у страны хозяин
                if (leave < 2)
                {
                    Army armyOfVictory = null;
                    foreach (var army in listOfArmies)
                    {
                        if (!army.IsEmpty())
                        { // Армия победитель
                            armyOfVictory = army;
                            //journal.SetMessage("Армия победитель: " + army.name);
                            country.ChangeOwner((int)army.ridgamer);
                        }
                    }
                    if (armyOfVictory == null)
                    {
                        //journal.SetMessage(country.nameEng + ": Победитель не определен, armyOfVictory == null");
                    }
                }
                else
                {
                    //journal.SetMessage(country.nameEng + ": Победитель не определен, живых армий >= 2");
                }
            }

            // После всех сражений чистим таблицу BattleMech от убитых мехов. Удаляем все строки с qnt = 0
            ClearBattleMechs();
        }
    }
}