using WBS_Assessment.Core.Models;

namespace WBS_Assessment.Infrastructure;

public static class SeedData
{
    public static readonly User[] Users =
    [
        new() { Id = Guid.Parse("c452cd02-d694-45dd-9f62-63c1658e7fdf"), DisplayName = "Jane Doe" },
        new() { Id = Guid.Parse("ed60ba0c-63b6-4633-9bea-c6ac7b8f026c"), DisplayName = "John Doe" },
        new() { Id = Guid.Parse("435b3b76-9f38-4b14-b4a2-1da57784edc3"), DisplayName = "Dave Mustaine" },
        new() { Id = Guid.Parse("e7781049-cc4a-4566-9d80-ca538482ce06"), DisplayName = "Ronnie James Dio" },
        new() { Id = Guid.Parse("c43c6cc5-ed8a-4dd0-8d8e-23cdf072d416"), DisplayName = "Bruce Dickinson" }
    ];
    
    public static readonly Apartment[] Apartments =
    [
        new() { Id = Guid.Parse("17414245-3c81-4095-a58e-fffef3aa3f7d"), Name = "Sea Point" },
        new() { Id = Guid.Parse("29ea754f-dcb6-4337-a7ab-16bbb68b3b0b"), Name = "Hillcrest" },
        new() { Id = Guid.Parse("7831a8c0-3651-46d2-a620-bb6c64efc9a1"), Name = "Howick" }
    ];
    
    public static readonly Vehicle[] Vehicles =
    [
        new() { Id = Guid.Parse("48c32d17-8610-4792-8bec-8d5cf8fbab03"), Name = "Cavalcade", Registration = "CAA123456"},
        new() { Id = Guid.Parse("a7b8be1f-d6cd-4230-8ec7-cd38fa7fb183"), Name = "Pontiac", Registration = "CAA15436"},
        new() { Id = Guid.Parse("7e9694b5-0070-431f-9ebd-76fc1a343ca0"), Name = "Chevrolet", Registration = "CAA87632"}
    ];
    
    public static readonly Show[] Shows =
    [
        new() { Id = Guid.Parse("fc476c9b-b836-4e66-acf5-1442b2c51561"), Name = "Comedy Club", Venue = "Cape Town City Center"},
        new() { Id = Guid.Parse("ecf56167-2b5e-4936-bbeb-c950f8cabe17"), Name = "Heavy Metal Concert", Venue = "Cape Town City Center"},
        new() { Id = Guid.Parse("bcff6d67-d516-4111-8d1f-18181c16f668"), Name = "Battle of the Bands", Venue = "Cape Town City Center"}
    ];
}