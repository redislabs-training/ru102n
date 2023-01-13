namespace section3._3.Migrations
{
    using section3._3.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.SQLite.EF6.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<section3._3.Models.SalesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override async void Seed(section3._3.Models.SalesContext context)
        {
            Console.WriteLine("seeding. . .");
            var names = new[] { "Alice", "Bob", "Carlos", "Dan", "Yves" };
            var random = new Random();

            foreach(var name in names)
            {
                var employee = new Employee { Name = name };
                context.Employees.Add(employee);
            }

            Console.WriteLine("Saving Employees");
            context.SaveChanges();
            
            foreach(var name in names)
            {
                var employee = context.Employees.First(x => x.Name == name);
                for(var i = 0; i < 10000; i++)
                {
                    employee.Sales.Add(new Sale { Total = random.Next(1000, 30000) });
                }

                context.SaveChanges();
            }
        }
    }
}
