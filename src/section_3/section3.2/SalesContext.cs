using Microsoft.EntityFrameworkCore;

namespace section3._2;

public class SalesContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public string DbPath { get; set; }

    public SalesContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "Sales.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite($"Data Source={DbPath}");
}

public class Employee
{
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public List<Sale> Sales { get; } = new();
}

public class Sale
{
    public int SaleId { get; set; }
    public int Total { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
}