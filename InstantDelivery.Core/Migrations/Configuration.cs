using System;
using System.Collections.Generic;
using System.Linq;
using InstantDelivery.Core.Entities;

namespace InstantDelivery.Core.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<InstantDelivery.Core.InstantDeliveryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "InstantDelivery.Core.InstantDeliveryContext";
        }

        protected override void Seed(InstantDelivery.Core.InstantDeliveryContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            GenerateTestData(context);

            context.SaveChanges();
        }

        private static void GenerateTestData(InstantDeliveryContext context)
        {
            GenerateTestEmployees(context);
            GenerateTestVehicles(context);
            GenerateTestVehicleModels(context);
            GenerateTestPackages(context);

            GeneratePackageEmployeeRelations(context);
            GenerateVehicleVehicleModelRelations(context);
        }

        private static void GenerateVehicleVehicleModelRelations(InstantDeliveryContext context)
        {
            var randomNumber = new Random();
            for (var i = 0; i < 30; i++)
            {
                var next = randomNumber.Next();
                var i1 = i + 1;
                var firstOrDefault = context.Vehicles.FirstOrDefault(e => e.VehicleId == i1);
                if (firstOrDefault != null)
                    firstOrDefault.VehicleModel = context.VehicleModels.FirstOrDefault(e => e.VehicleModelId == ((next % 10) + 1));
            }
        }

        private static void GeneratePackageEmployeeRelations(InstantDeliveryContext context)
        {
            for (var i = 1; i < 30; i++)
            {
                var i1 = i;
                var firstOrDefault = context.Employees.FirstOrDefault(e => e.EmployeeId == i1);
                if (firstOrDefault != null)
                    firstOrDefault.Packages = new List<Package>() { context.Packages.FirstOrDefault(e => e.PackageId == i1) };
            }
            var orDefault = context.Employees.FirstOrDefault(e => e.EmployeeId == 30);
            if (orDefault != null)
                orDefault.Packages = new List<Package>()
                {
                    context.Packages.FirstOrDefault(e => e.PackageId ==31),
                    context.Packages.FirstOrDefault(e => e.PackageId ==32),
                };
            var employee = context.Employees.FirstOrDefault(e => e.EmployeeId == 31);
            if (employee != null)
                employee.Packages = new List<Package>()
                {
                    context.Packages.FirstOrDefault(e => e.PackageId ==33),
                    context.Packages.FirstOrDefault(e => e.PackageId ==34),
                    context.Packages.FirstOrDefault(e => e.PackageId ==35),
                };
        }

        private static void GenerateTestPackages(InstantDeliveryContext context)
        {
            var randomNumber = new Random();
            var testPackages = new List<Package>();
            for (var i = 0; i < 40; i++)
            {
                var tmp = new Package
                {
                    Height = randomNumber.Next() % 1000,
                    PackageId = i + 1,
                    ShippingAddress =
                    {
                        City = "Warsaw",
                        Country = "Poland",
                        Number = (randomNumber.Next()%500).ToString(),
                        PostalCode = ((randomNumber.Next()%99 + 1).ToString() + "-" + (randomNumber.Next()%999 + 1)),
                        State = randomNumber.Next().ToString(),
                        Street = randomNumber.Next().ToString()
                    },
                    Weight = i % 300,
                    Width = i % 1000,
                };
                testPackages.Add(tmp);
            }
            context.Packages.AddOrUpdate(testPackages.ToArray());
        }

        private static void GenerateTestVehicleModels(InstantDeliveryContext context)
        {
            var randomNumber = new Random();
            var brands = new[] { "Opel", "Fiat", "Audi", "Toyota", "Mercedez", "Opel", "Fiat", "Audi", "Toyota", "Mercedez" };
            var models = new[] { "Astra", "Jaki�", "A4", "Yaris", "Benz", "Vectra", "Punto", "A6", "Corolla", "Classic" };

            var testVehicleModels = new List<VehicleModel>();
            for (var i = 0; i < 10; i++)
            {
                testVehicleModels.Add(new VehicleModel()
                {
                    Brand = brands[i],
                    Model = models[i],
                    Payload = (double)(randomNumber.Next() % 1000) / 2,
                    VehicleModelId = i + 1,
                });
            }
            context.VehicleModels.AddOrUpdate(testVehicleModels.ToArray());
        }

        private static void GenerateTestVehicles(InstantDeliveryContext context)
        {
            var randomNumber = new Random();
            var testVehicle = new List<Vehicle>();
            for (var i = 0; i < 30; i++)
            {
                testVehicle.Add(new Vehicle()
                {
                    RegistrationNumber = "WWW" + (randomNumber.Next() % 100 + 4000).ToString() + "PL",
                    VehicleId = i + 1,
                });
            }
            context.Vehicles.AddOrUpdate(testVehicle.ToArray());
        }

        private static void GenerateTestEmployees(InstantDeliveryContext context)
        {
            var firstName = new[]
            {
                "Jessie", "Jessie", "Justin", "Britney", "Ariana", "David", "Adam", "Enrique", "Ashton", "Lisa", "Bruno", "Jan",
                "Denis", "Richard", "Witold", "Cezary", "Bart�omiej", "Anastazja", "Mariusz", "Janusz", "Helena", "Dorota",
                "Anna", "Henryk", "Markus", "�ukasz", "Mateusz", "Adrian", "Micha�", "Ted", "Robin", "Marshall", "Lily",
                "Barney", "Brad", "Elliot", "John", "Jan", "Norbert", "Damian"
            };

            var lastName = new[]
            {
                "James", "Rogers", "Timberlake", "Spears", "Grande", "Beckham", "Levine", "Iglesias", "Kutcher", "Row", "Mars",
                "Andrzejewski", "Rozrabiaka", "Marx", "Tacikiewicz", "�ak", "D�browski", "B�k", "Kolonko", "Swojski", "Roma�ski",
                "Zawadzka", "Mucha", "Sienkiewicz", "Ellen", "Jak�biak", "Piasecki", "Dudek", "Nachtman", "Mosby", "Scherbatsky",
                "Erickson", "Aldrin", "Stinson", "Pitt", "Reid", "Dorian", "Andrzejewski", "Gierczak", "Damil"
            };
            var randomNumber = new Random();
            var testEmployees = new List<Employee>();

            for (var i = 0; i < 40; i++)
            {
                testEmployees.Add(new Employee
                {
                    EmployeeId = i + 1,
                    FirstName = firstName[i],
                    LastName = lastName[i],
                    Gender = (Gender)(randomNumber.Next() % 2),
                    DateOfBirth =
                        new DateTime((randomNumber.Next() % 100) + 1900, (randomNumber.Next() % 12) + 1, (randomNumber.Next() % 30) + 1),
                    PhoneNumber =
                        ((randomNumber.Next() % 300) + 700).ToString() + ((randomNumber.Next() % 300) + 700).ToString() +
                        ((randomNumber.Next() % 300) + 700).ToString(),
                    Salary = randomNumber.Next() % 1000 + 2000,
                    HireDate =
                        new DateTime((randomNumber.Next() % 100) + 1900, (randomNumber.Next() % 12) + 1, (randomNumber.Next() % 30) + 1),
                });
            }

            context.Employees.AddOrUpdate(testEmployees.ToArray());
        }
    }
}