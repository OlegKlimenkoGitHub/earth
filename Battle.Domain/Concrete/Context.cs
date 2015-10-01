using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace Battle.Domain.Concrete
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
            //: base("Entities")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }


    public class CountryInitContext : DbContext
    {
        // В данном случае "DefaultConnection" - имя подключения к базе данных
        // которое настроено в файле web.config в корне MVC проекта.
        public CountryInitContext() : base("DefaultConnection") { }
        public DbSet<CountryInit> InitCountries { get; set; }
    }

    public class CountryContext : DbContext
    {
        // В данном случае "DefaultConnection" - имя подключения к базе данных
        // которое настроено в файле web.config в корне MVC проекта.
        public CountryContext() : base("DefaultConnection") { }
        public DbSet<Country> Countries { get; set; }
    }

    public class GamerContext : DbContext
    {
        // В данном случае "DefaultConnection" - имя подключения к базе данных
        // которое настроено в файле web.config в корне MVC проекта.
        public GamerContext() : base("DefaultConnection") { }
        public DbSet<Gamer> Gamers { get; set; }
    }

    public class DesignContext : DbContext
    {
        // В данном случае "DefaultConnection" - имя подключения к базе данных
        // которое настроено в файле web.config в корне MVC проекта.
        public DesignContext() : base("DefaultConnection") { }
        public DbSet<Design> Designes { get; set; }
    }

    public class ArmyContext : DbContext
    {
        // В данном случае "DefaultConnection" - имя подключения к базе данных
        // которое настроено в файле web.config в корне MVC проекта.
        public ArmyContext() : base("DefaultConnection") { }
        public DbSet<Army> Armies { get; set; }
    }

    public class BattleMechContext : DbContext
    {
        // В данном случае "DefaultConnection" - имя подключения к базе данных
        // которое настроено в файле web.config в корне MVC проекта.
        public BattleMechContext() : base("DefaultConnection") { }
        public DbSet<Battlemech> BattleMechs { get; set; }
    }

    public class EvContext : DbContext
    { 
        public EvContext() : base("DefaultConnection") {}
        public DbSet<Ev> Events {get; set;}
    }

    public class ManufactureContext : DbContext
    {
        // В данном случае "DefaultConnection" - имя подключения к базе данных
        // которое настроено в файле web.config в корне MVC проекта.
        public ManufactureContext() : base("DefaultConnection") { }
        public DbSet<Manufacture> Manufactures { get; set; }
    }

}
