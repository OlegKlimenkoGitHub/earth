using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Battle.Domain.Concrete;

namespace Battle.Domain.Concrete
{
    public partial class Battlemech
    {

        BattleMechContext battleMechTable { get; set; }
        DesignContext designTable;
        //public int qnt { get; set; } // 1 - жив, 0 - мертв

        public Battlemech()
        {
            battleMechTable = new Battle.Domain.Concrete.BattleMechContext();
        }

        public Battlemech(int ridDesign, int ridArmy)
        {
            battleMechTable = new Battle.Domain.Concrete.BattleMechContext();
            designTable = new Battle.Domain.Concrete.DesignContext();

            int maxid = 0;
            try
            {
                maxid = battleMechTable.Database.SqlQuery<int>("Select max(id) from Battlemechs").FirstOrDefault<int>();
            }
            catch
            {
                maxid = 0;
            }
            maxid++;

            string designName = designTable.Database.SqlQuery<string>(
                "Select name from Design where rid = " + ridDesign.ToString()).FirstOrDefault<string>();

            this.name = "Mech-" + maxid.ToString() + " " + designName;

            int i = battleMechTable.Database.ExecuteSqlCommand(
                "Insert into Battlemechs(id,name,riddesign,ridarmy,qnt) Values(' "
                + maxid.ToString() + "','" + this.name + "','" + ridDesign.ToString() + "','" + ridArmy.ToString() + "','" + 1.ToString() + "')");

            // Получить rid меха
            int ridBattleMech = battleMechTable.Database.SqlQuery<int>(
                "Select rid from Battlemechs where id=" + maxid.ToString()).FirstOrDefault<int>();

            this.id = maxid;
            this.rid = ridBattleMech;
            this.qnt = 1;

        }

        // Переместить меха в другую армию
        public void MoveTo(int ridArmy)
        {
            this.ridarmy = ridArmy;
            int i = battleMechTable.Database.ExecuteSqlCommand(
                "Update Battlemechs set ridarmy = '" + ridArmy.ToString() + "' where rid = " + this.rid.ToString());
        }

        public Design GetDesign()
        {
            designTable = new Battle.Domain.Concrete.DesignContext();
            return designTable.Designes.SqlQuery(
                    "Select * from Design where rid = '" + this.riddesign.ToString() + "'").FirstOrDefault();
        }

        // Отстреляться по вражеской армии
        public void FireTo(Army targetArmy)
        {
            // Определить сколько у нашего меха пушек и какого калибра
            Design designAttacker = GetDesign();

            if (designAttacker.guns < 1) { return; } // Нечем стрелять, нет пушек
            // Огонь из всех орудий!
            for (int i = 1; i <= designAttacker.guns; i++)
            {
                // получить список живых мехов вражеской армии
                var listOfTargets = targetArmy.GetLiveBattleMechs();
                if (listOfTargets.Count == 0) { break; } // цели кончились
                var rn = new Random();
                int target = rn.Next(listOfTargets.Count);
                Design designTarget = listOfTargets[target].GetDesign();
                if (designAttacker.caliber >= designTarget.shield)
                {
                    listOfTargets[target].Kill(); // Цель уничтожена!
                }
                else
                {
                    // Броня не пробиваема!
                }
            }
        }

        // Убить меха
        public void Kill()
        {
            int i = battleMechTable.Database.ExecuteSqlCommand(
                "Update Battlemechs set qnt = 0 where rid = " + this.rid.ToString());
        }
    }
}