using Battle.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Battle.Domain.Concrete
{
    public partial class Manufacture
    {
        public Manufacture() { }

        public Manufacture(Country _country, Design _design, int? _qnt)
        {
            qnt = _qnt;
            ridcountry = _country.rid;
            riddesign = _design.rid;
        }

        public void AddRowInBase()
        {
            var manufactureTable = new ManufactureContext();
            int maxid = 0;
            try
            {
                maxid = manufactureTable.Database.SqlQuery<int>("Select max(id) from Manufacture").FirstOrDefault<int>();
            }
            catch
            {
                maxid = 0;
            }
            maxid++;

            int i = manufactureTable.Database.ExecuteSqlCommand(
                "Insert into Manufacture(id,qnt,ridcountry,riddesign) Values('"
                + maxid.ToString() + "','" + qnt.ToString() + "','" + ridcountry.ToString() + "','" + riddesign.ToString() + "')");

            // Получить rid 
            int ridmanufacture = manufactureTable.Database.SqlQuery<int>(
                "Select rid from Manufacture where id=" + maxid.ToString()).FirstOrDefault<int>();

            this.id = maxid;
            this.rid = ridmanufacture;

        }
    }
}