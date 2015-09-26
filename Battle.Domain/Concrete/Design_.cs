using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Battle.Domain.Concrete
{
    public partial class Design
    {
        public Design(string _name, int _shield, int _guns, int _caliber)
        {
            name = _name;
            shield = _shield;
            guns = _guns;
            caliber = _caliber;
            try
            {
                weight = _shield + _guns * _caliber;
            }
            catch
            {
                weight = 0;
            }
        }

        public void AddRowInBase(int ridGamer)
        {
            int i = 0; // Количество строк обработанных sql-запросом

            var designTable = new Battle.Domain.Concrete.DesignContext();

            //Найти максимальный id среди дизайнов мехов для данного игрока
            int newid = 0;
            try
            {
                newid = designTable.Database.SqlQuery<int>(
                    "Select max(id) from Design").FirstOrDefault<int>();
            }
            catch
            {
                newid = 0;
            }
            newid++;

            // Создать новый дизайн боевого меха
            weight = shield + guns * caliber;
            i = designTable.Database.ExecuteSqlCommand(
                "Insert into Design(id,name,shield,guns,caliber,weight,ridgamer) Values('"
                + newid.ToString() + "','"
                + name + "','"
                + shield.ToString() + "','"
                + guns.ToString() + "','"
                + caliber.ToString() + "','"
                + weight.ToString() + "','"
                + ridGamer.ToString() + "')");

            this.rid = designTable.Database.SqlQuery<int>("Select rid from Design where id=" + newid.ToString()).FirstOrDefault<int>();
            this.id = newid;
            this.ridgamer = ridGamer;
        }
    }
}