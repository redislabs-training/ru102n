using Redis.OM.Modeling;

namespace section5._4;

[Document(StorageType = StorageType.Json, Prefixes = new []{"Employee"}, IndexName = "employees")]
public class Employee
{
    [RedisIdField]
    [Indexed]
    public string? Id { get; set; }
    [Indexed]
    public List<string>? Sales { get; set; }
    [Indexed(JsonPath = "$.Location")]
    [Indexed(JsonPath = "$.PostalCode")]
    public Address? Address { get; set; }
    [Indexed(Sortable = true)]
    public string? Name { get; set; }
    [Indexed(Sortable = true)] 
    public int Age { get; set; }
}

[Document(StorageType = StorageType.Json)]
public class Sale
{
    [RedisIdField]
    [Indexed]
    public string? Id { get; set; }
    [Indexed]
    public string? EmployeeId { get; set; }
    [Indexed]
    public int Total { get; set; }
    [Indexed(CascadeDepth = 2)]
    public Address? Address { get; set; }
}

public class Address
{
    [Searchable]
    public string? StreetAddress { get; set; }
    [Indexed]
    public string? PostalCode { get; set; }
    [Indexed(Sortable = true)]
    public GeoLoc Location { get; set; }
    [Indexed(CascadeDepth = 1)]
    public Address? ForwardingAddress { get; set; }
}