using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Battle.Domain.Concrete
{
    public partial class Army
    {
        public Army(string _name, int ridcountry, int ridgamer)
        {
            var armyTable = new Battle.Domain.Concrete.ArmyContext();

            int maxid = 0;
            try
            {
                maxid = armyTable.Database.SqlQuery<int>("Select max(id) from Army").FirstOrDefault<int>();
            }
            catch
            {
                maxid = 0;
            }
            maxid++;

            int i = armyTable.Database.ExecuteSqlCommand(
                "Insert into Army(id, name,ridcountry,ridgamer) Values(' "
                + maxid.ToString() + "','" + _name + "','" + ridcountry.ToString() + "','" + ridgamer.ToString() + "')");

            // Получить rid 
            int ridarmy = armyTable.Database.SqlQuery<int>(
                "Select rid from Army where id=" + maxid.ToString()).FirstOrDefault<int>();

            this.id = maxid;
            this.rid = ridarmy;
            this.name = _name;
        }

        // Получить список мехов в армии
        public List<Battlemech> GetBattleMechs()
        {
            var battleMechsTable = new Battle.Domain.Concrete.BattleMechContext();
            return battleMechsTable.BattleMechs.SqlQuery(
                "Select * from Battlemechs where ridarmy = '" + this.rid.ToString() + "'"
                ).ToList();
        }

        // Получить список живых мехов в армии
        public List<Battlemech> GetLiveBattleMechs()
        {
            var battleMechsTable = new Battle.Domain.Concrete.BattleMechContext();
            return battleMechsTable.BattleMechs.SqlQuery(
                "Select * from Battlemechs where ridarmy = '" + this.rid.ToString() + "' and qnt > 0"
                ).ToList();
        }

        // Перемещаем мехов из этой армии в армию в указанной стране
        public void MoveTo(Country targetCountry)
        {
            Army targetArmy = targetCountry.GetAttackArmy((int)this.ridgamer);
            var mechsList = GetBattleMechs();
            foreach (var mech in mechsList)
            {
                mech.MoveTo(targetArmy.rid);
            }
        }

        public Boolean IsEmpty()
        {
            List<Battlemech> mechs = GetBattleMechs();
            foreach (var item in mechs)
            {
                if (item.qnt > 0) { return false; }
            }
            return true;
        }

        //public Boolean IsEmpty(Journal journal)
        //{
        //    List<Battlemech> mechs = GetBattleMechs();
        //    journal.SetMessage("IsEmpty:: Получен список мехов в армии:");
        //    foreach (var item in mechs)
        //    {
        //        journal.SetMessage("IsEmpty:: " + item.name + " qnt = " + item.qnt.ToString());
        //        if (item.qnt > 0) { return false; }
        //    }
        //    journal.SetMessage("IsEmpty:: Боеспособных мехов не найдено");
        //    return true;
        //}


        public int GetQntMechs()
        {
            var battleMechsTable = new Battle.Domain.Concrete.BattleMechContext();
            List<Battlemech> mechs = battleMechsTable.BattleMechs.SqlQuery(
                "Select * from Battlemechs where ridarmy = '" + this.rid.ToString() + "'"
                ).ToList();
            return mechs.Count();
        }

        public Army GetTargetArmy(List<Army> armies)
        {
            if (armies.Count < 2) { return null; }
            int i = 0;
            do
            {
                i++;
                var rn = new Random();
                int target = rn.Next(armies.Count);
                if (armies[target].IsEmpty()) { continue; } // У этих не осталось мехов
                if (armies[target].id != this.id) { return armies[target]; }
                if (i > armies.Count * 2) { return null; } // Не могу прицелиться
            } while (true);
        }

        //public Army GetTargetArmy(List<Army> armies, Journal journal)
        //{
        //    if (armies.Count < 2) {
        //        journal.SetMessage("GetTargetArmy return null : armies.Count < 2 : " + armies.Count.ToString());
        //        return null; 
        //    }
        //    int i = 0;
        //    do
        //    {
        //        i++;
        //        var rn = new Random();
        //        int target = rn.Next(armies.Count);
        //        journal.SetMessage("GetTargetArmy:: target = " + target.ToString());
        //        if (armies[target].IsEmpty()) { // У этих не осталось мехов
        //            journal.SetMessage("GetTargetArmy:: В армии " + armies[target].name + " не осталось мехов");
        //            continue; 
        //        } 
        //        if (armies[target].id != this.id) {
        //            journal.SetMessage("GetTargetArmy:: Цель выбрана : " + armies[target].name);
        //            return armies[target]; 
        //        }
        //        if (i > armies.Count * 2)  { // Не могу прицелиться
        //            journal.SetMessage("GetTargetArmy:: не могу прицелиться");
        //            return null; 
        //        } 
        //    } while (true);
        //}


    }
}