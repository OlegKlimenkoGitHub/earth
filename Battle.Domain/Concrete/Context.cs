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
            //: base("EFDbContext")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }


    public class CountryInitContext : DbContext
    {
        // В данном случае "EFDbContext" - имя подключения к базе данных
        // которое настроено в файле web.config в корне MVC проекта.
        public CountryInitContext() : base("EFDbContext") { }
        public DbSet<CountryInit> InitCountries { get; set; }
    }

    public class CountryContext : DbContext
    {
        // В данном случае "EFDbContext" - имя подключения к базе данных
        // которое настроено в файле web.config в корне MVC проекта.
        public CountryContext() : base("EFDbContext") { }
        public DbSet<Country> Countries { get; set; }
    }

    public class GamerContext : DbContext
    {
        // В данном случае "EFDbContext" - имя подключения к базе данных
        // которое настроено в файле web.config в корне MVC проекта.
        public GamerContext() : base("EFDbContext") { }
        public DbSet<Gamer> Gamers { get; set; }
    }

    public class DesignContext : DbContext
    {
        // В данном случае "EFDbContext" - имя подключения к базе данных
        // которое настроено в файле web.config в корне MVC проекта.
        public DesignContext() : base("EFDbContext") { }
        public DbSet<Design> Designes { get; set; }
    }

    public class ArmyContext : DbContext
    {
        // В данном случае "EFDbContext" - имя подключения к базе данных
        // которое настроено в файле web.config в корне MVC проекта.
        public ArmyContext() : base("EFDbContext") { }
        public DbSet<Army> Armies { get; set; }
    }

    public class BattleMechContext : DbContext
    {
        // В данном случае "EFDbContext" - имя подключения к базе данных
        // которое настроено в файле web.config в корне MVC проекта.
        public BattleMechContext() : base("EFDbContext") { }
        public DbSet<Battlemech> BattleMechs { get; set; }
    }

    public class EvContext : DbContext
    { 
        public EvContext() : base("EFDbContext") {}
        public DbSet<Ev> Events {get; set;}
    }

    public class ManufactureContext : DbContext
    {
        // В данном случае "EFDbContext" - имя подключения к базе данных
        // которое настроено в файле web.config в корне MVC проекта.
        public ManufactureContext() : base("EFDbContext") { }
        public DbSet<Manufacture> Manufactures { get; set; }
    }

}
