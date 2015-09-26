using Battle.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Battle.Domain.Concrete
{
    public partial class Gamer
    {
        public GamerContext gamersTable;

        public Gamer(string _name, string _color, string _type, string _status)
        {
            this.color = _color;
            this.type = _type;
            this.status = _status;
            this.id = GetNewId();
            this.name = _name;
            //AddRowInBase();
        }

        public Gamer(string _color, string _type, string _status)
        {
            this.color = _color;
            this.type = _type;
            this.status = _status;
            this.id = GetNewId();
            if (_type == "computer")
            {
                this.name = "Comp" + this.id.ToString();
            }
            else
            {
                this.name = _type + this.id.ToString();
            }
            //AddRowInBase();
        }

        public int GetNewId()
        {
            gamersTable = new GamerContext();
            int maxid = 0;
            try
            {
                maxid = gamersTable.Database.SqlQuery<int>("Select max(id) from Gamers").FirstOrDefault<int>();
            }
            catch
            {
                maxid = 0;
            }
            maxid++;
            return maxid;
        }

        public int AddRowInBase()
        {
            int i = gamersTable.Database.ExecuteSqlCommand(
                "Insert into Gamers(id,name,color,type,status) Values('"
                + id.ToString() + "','" + name + "','" + color + "','" + type + "','" + status + "')");

            // Получить rid этого человека
            int ridgamer = gamersTable.Database.SqlQuery<int>(
                "Select rid from Gamers where id=" + id.ToString()).FirstOrDefault<int>();

            this.rid = ridgamer;

            return ridgamer;
        }
    }
}