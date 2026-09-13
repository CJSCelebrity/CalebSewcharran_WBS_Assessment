using WBS_Assessment.Application.Repositories;
using WBS_Assessment.Application.Services;
using WBS_Assessment.Application.Utilities;
using WBS_Assessment.Application.Validators;
using WBS_Assessment.Console;
using WBS_Assessment.Core.Models;

namespace WBS_Assessment.Tests.ConsoleApp;

/// <summary>
/// Builds a <see cref="ConsoleMenu"/> over mocked repositories and redirects
/// System.Console so a test can script the keystrokes and read back the output.
/// </summary>
internal sealed class ConsoleMenuHarness : IDisposable
{
    public static readonly User FirstUser = new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), DisplayName = "Jane Doe" };
    public static readonly User SecondUser = new() { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), DisplayName = "John Doe" };

    public static readonly Apartment Apartment = new() { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Sea Point" };
    public static readonly Vehicle Vehicle = new() { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Cavalcade" };
    public static readonly Show Show = new() { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Comedy Club" };

    private readonly TextReader _originalIn = System.Console.In;
    private readonly TextWriter _originalOut = System.Console.Out;
    private readonly StringWriter _output = new();

    public Mock<IBookingRepository> Bookings { get; } = new();
    public Mock<IUserRepository> Users { get; } = new();
    public Mock<IRepository<Apartment>> Apartments { get; } = new();
    public Mock<IRepository<Vehicle>> Vehicles { get; } = new();
    public Mock<IRepository<Show>> Shows { get; } = new();

    public ConsoleMenu Menu { get; }

    public string Output => _output.ToString();

    public ConsoleMenuHarness(params string[] input)
    {
        Users.Setup(r => r.GetAll()).Returns([FirstUser, SecondUser]);
        Users.Setup(r => r.GetById(FirstUser.Id)).Returns(FirstUser);
        Users.Setup(r => r.GetById(SecondUser.Id)).Returns(SecondUser);

        Apartments.Setup(r => r.GetAll()).Returns([Apartment]);
        Apartments.Setup(r => r.GetById(Apartment.Id)).Returns(Apartment);

        Vehicles.Setup(r => r.GetAll()).Returns([Vehicle]);
        Vehicles.Setup(r => r.GetById(Vehicle.Id)).Returns(Vehicle);

        Shows.Setup(r => r.GetAll()).Returns([Show]);
        Shows.Setup(r => r.GetById(Show.Id)).Returns(Show);

        Bookings.Setup(r => r.GetByUser(It.IsAny<Guid>())).Returns([]);

        var guard = new BookingGuard(Users.Object);

        Menu = new ConsoleMenu(
            new ApartmentBookingService(Bookings.Object, guard, Apartments.Object, new CreateApartmentBookingValidator()),
            new VehicleBookingService(Bookings.Object, Vehicles.Object, guard, new CreateVehicleBookingValidator()),
            new ShowBookingService(Bookings.Object, guard, Shows.Object, new CreateShowBookingValidator()),
            new BookingManagementService(Bookings.Object, Users.Object),
            Apartments.Object,
            Vehicles.Object,
            Shows.Object,
            Users.Object);

        System.Console.SetIn(new StringReader(string.Join(Environment.NewLine, input)));
        System.Console.SetOut(_output);
    }

    public void Dispose()
    {
        System.Console.SetIn(_originalIn);
        System.Console.SetOut(_originalOut);
        _output.Dispose();
    }
}
