using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Battle.Domain.Concrete
{
    public partial class Country
    {
        public List<Battlemech> mechsInCurrentCountry { get; set; }  // Защитники
        public List<Battlemech> enemyMechsInCurrentCountry { get; set; } // Враги

        // Определить список механизмов, которые принадлежат хозяину этой территории
        public void GetMechanismesForCountry()
        {
            var battleMechsTable = new Battle.Domain.Concrete.BattleMechContext();

            // Защитники
            mechsInCurrentCountry = battleMechsTable.BattleMechs.SqlQuery(
                "Select * from BattleMechs INNER JOIN  Army ON Battlemechs.ridarmy = Army.rid " +
                "INNER JOIN  Country ON Country.rid = Army.ridcountry WHERE " +
                "Country.id = '" + this.id.ToString() + "'" +
                " and Army.ridgamer = Country.ridgamer"
                ).ToList();

            // Нападающие
            enemyMechsInCurrentCountry = battleMechsTable.BattleMechs.SqlQuery(
                "Select * from BattleMechs INNER JOIN  Army ON Battlemechs.ridarmy = Army.rid " +
                "INNER JOIN  Country ON Country.rid = Army.ridcountry WHERE " +
                "Country.id = '" + this.id.ToString() + "'" +
                " and Army.ridgamer != Country.ridgamer"
                ).ToList();

        }


        // Получить параметры страны по коду 
        // TODO: Переделать, возвращать объект типа Country
        public void GetByCod(string cod)
        {
            if (cod.Equals(null))
            {
                cod = ""; rid = 0; id = 0; nameEng = ""; color = ""; ridgamer = 0;
            }
            else
            {
                var countryTable = new Battle.Domain.Concrete.CountryContext();
                this.cod = cod;
                rid = countryTable.Database.SqlQuery<int>("Select rid from Country where cod = '" + cod + "'").FirstOrDefault<int>();
                id = countryTable.Database.SqlQuery<int>("Select id from Country where cod = '" + cod + "'").FirstOrDefault<int>();
                nameEng = countryTable.Database.SqlQuery<string>("Select name from Country where cod = '" + cod + "'").FirstOrDefault<string>();
                color = countryTable.Database.SqlQuery<string>("Select color from Country where cod = '" + cod + "'").FirstOrDefault<string>();
                mineral = countryTable.Database.SqlQuery<int>("Select mineral from Country where cod = '" + cod + "'").FirstOrDefault<int>();
                ridgamer = countryTable.Database.SqlQuery<int>("Select ridgamer from Country where cod = '" + cod + "'").FirstOrDefault<int>();
            }
        }

        // Получить армию хозяина этой страны
        public Army GetArmy()
        {
            var armyTable = new Battle.Domain.Concrete.ArmyContext();
            return armyTable.Armies.SqlQuery(
                "Select * from Army where Army.ridcountry = '" + rid.ToString() + "' and Army.ridgamer = '" + ridgamer.ToString() + "'").FirstOrDefault();
        }

        // Получить армию другого игрока в этой стране
        public Army GetAttackArmy(int ridGamer)
        {
            var armyTable = new Battle.Domain.Concrete.ArmyContext();
            return armyTable.Armies.SqlQuery(
                "Select * from Army where Army.ridcountry = '" + rid.ToString() + "' and Army.ridgamer = '" + ridGamer.ToString() + "'").FirstOrDefault();
        }

        // Получить список всех армий, готовых сразиться за эту страну
        public List<Army> GetReadyArmies()
        {
            var armyTable = new Battle.Domain.Concrete.ArmyContext();
            return armyTable.Armies.SqlQuery(
                "Select * from Army where Army.ridcountry = '" + rid.ToString() + "'").ToList();
        }

        // Сменить владельца страны
        public void ChangeOwner(int ridGamer)
        {
            var countryTable = new Battle.Domain.Concrete.CountryContext();
            int i = countryTable.Database.ExecuteSqlCommand(
                "Update Country set ridgamer = '" + ridGamer.ToString() + "' where id=" + this.id.ToString());
        }

    }
}