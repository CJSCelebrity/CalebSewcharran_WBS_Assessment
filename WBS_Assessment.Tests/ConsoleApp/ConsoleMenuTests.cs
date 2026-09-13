using WBS_Assessment.Core.Enums;
using WBS_Assessment.Core.Models;

namespace WBS_Assessment.Tests.ConsoleApp;

[TestFixture]
[NonParallelizable]
public class ConsoleMenuTests
{
    [Test]
    public void Run_WithApartmentBookingFlow_AddsApartmentBooking()
    {
        using var sut = new ConsoleMenuHarness("1", "1", "1", "2026-01-01", "2026-01-05", "0");

        sut.Menu.Run();

        sut.Bookings.Verify(r => r.Add(It.IsAny<ApartmentBooking>()), Times.Once);
        sut.Output.ShouldContain("Booked. Reference:");
    }

    [Test]
    public void Run_WithVehicleBookingFlow_AddsVehicleBooking()
    {
        using var sut = new ConsoleMenuHarness("2", "1", "1", "2026-01-01", "2026-01-05", "0");

        sut.Menu.Run();

        sut.Bookings.Verify(r => r.Add(It.IsAny<VehicleBooking>()), Times.Once);
        sut.Output.ShouldContain("Booked. Reference:");
    }

    [Test]
    public void Run_WithShowBookingFlow_AddsShowBooking()
    {
        using var sut = new ConsoleMenuHarness("3", "1", "1", "2026-03-01", "0");

        sut.Menu.Run();

        sut.Bookings.Verify(r => r.Add(It.IsAny<ShowBooking>()), Times.Once);
        sut.Output.ShouldContain("Booked. Reference:");
    }

    [Test]
    public void Run_WithApartmentBooking_UsesTheSelectedUserAndApartment()
    {
        using var sut = new ConsoleMenuHarness("1", "2", "1", "2026-01-01", "2026-01-05", "0");

        sut.Menu.Run();

        sut.Bookings.Verify(r => r.Add(It.Is<ApartmentBooking>(b =>
            b.UserId == ConsoleMenuHarness.SecondUser.Id
            && b.ItemId == ConsoleMenuHarness.Apartment.Id
            && b.CheckIn == new DateTime(2026, 1, 1)
            && b.CheckOut == new DateTime(2026, 1, 5))), Times.Once);
    }

    [Test]
    public void Run_WithCheckOutBeforeCheckIn_WritesValidationError()
    {
        using var sut = new ConsoleMenuHarness("1", "1", "1", "2026-01-05", "2026-01-01", "0");

        sut.Menu.Run();

        sut.Output.ShouldContain("Invalid input:");
        sut.Bookings.Verify(r => r.Add(It.IsAny<Booking>()), Times.Never);
    }

    [Test]
    public void Run_WithMalformedDate_RepromptsUntilValid()
    {
        using var sut = new ConsoleMenuHarness("1", "1", "1", "not-a-date", "2026-01-01", "2026-01-05", "0");

        sut.Menu.Run();

        sut.Output.ShouldContain("Not a valid date. Please use yyyy-MM-dd.");
        sut.Bookings.Verify(r => r.Add(It.IsAny<ApartmentBooking>()), Times.Once);
    }

    [Test]
    public void Run_WithOutOfRangeSelection_RepromptsUntilValid()
    {
        using var sut = new ConsoleMenuHarness("1", "9", "1", "1", "2026-01-01", "2026-01-05", "0");

        sut.Menu.Run();

        sut.Output.ShouldContain("Please enter a number between 1 and 2.");
        sut.Bookings.Verify(r => r.Add(It.IsAny<ApartmentBooking>()), Times.Once);
    }

    [Test]
    public void Run_WhenUserHasNoBookings_WritesNoBookingsMessage()
    {
        using var sut = new ConsoleMenuHarness("4", "1", "0");

        sut.Menu.Run();

        sut.Output.ShouldContain("No bookings for this user.");
    }

    [Test]
    public void Run_WhenUserHasBookings_WritesEachBooking()
    {
        using var sut = new ConsoleMenuHarness("4", "1", "0");
        var booking = new ApartmentBooking(
            ConsoleMenuHarness.FirstUser.Id, ConsoleMenuHarness.Apartment.Id,
            new DateTime(2026, 1, 1), new DateTime(2026, 1, 5));

        sut.Bookings.Setup(r => r.GetByUser(ConsoleMenuHarness.FirstUser.Id)).Returns([booking]);

        sut.Menu.Run();

        sut.Output.ShouldContain(booking.Id.ToString());
        sut.Output.ShouldContain(nameof(ApartmentBooking));
        sut.Output.ShouldContain("2026-01-01 to 2026-01-05");
        sut.Output.ShouldContain(nameof(BookingStatus.Reserved));
    }

    [Test]
    public void Run_WhenAmendingApartmentBooking_ReschedulesBooking()
    {
        var booking = new ApartmentBooking(
            ConsoleMenuHarness.FirstUser.Id, ConsoleMenuHarness.Apartment.Id,
            new DateTime(2026, 1, 1), new DateTime(2026, 1, 5));

        using var sut = new ConsoleMenuHarness("5", booking.Id.ToString(), "2026-02-01", "2026-02-10", "0");
        sut.Bookings.Setup(r => r.GetById(booking.Id)).Returns(booking);

        sut.Menu.Run();

        booking.CheckIn.ShouldBe(new DateTime(2026, 2, 1));
        booking.CheckOut.ShouldBe(new DateTime(2026, 2, 10));
        sut.Output.ShouldContain("Booking amended.");
    }

    [Test]
    public void Run_WhenAmendingCancelledBooking_WritesCannotDoThat()
    {
        var booking = new ApartmentBooking(
            ConsoleMenuHarness.FirstUser.Id, ConsoleMenuHarness.Apartment.Id,
            new DateTime(2026, 1, 1), new DateTime(2026, 1, 5));
        booking.Cancel(DateTime.UtcNow);

        using var sut = new ConsoleMenuHarness("5", booking.Id.ToString(), "2026-02-01", "2026-02-10", "0");
        sut.Bookings.Setup(r => r.GetById(booking.Id)).Returns(booking);

        sut.Menu.Run();

        sut.Output.ShouldContain("Cannot do that:");
        booking.CheckIn.ShouldBe(new DateTime(2026, 1, 1));
    }

    [Test]
    public void Run_WhenCancellingBooking_MarksBookingCancelled()
    {
        var booking = new ApartmentBooking(
            ConsoleMenuHarness.FirstUser.Id, ConsoleMenuHarness.Apartment.Id,
            new DateTime(2026, 1, 1), new DateTime(2026, 1, 5));

        using var sut = new ConsoleMenuHarness("6", booking.Id.ToString(), "0");
        sut.Bookings.Setup(r => r.GetById(booking.Id)).Returns(booking);

        sut.Menu.Run();

        booking.Status.ShouldBe(BookingStatus.Cancelled);
        sut.Output.ShouldContain("Booking cancelled.");
    }

    [Test]
    public void Run_WhenCancellingUnknownBooking_WritesNotFound()
    {
        using var sut = new ConsoleMenuHarness("6", Guid.NewGuid().ToString(), "0");

        sut.Menu.Run();

        sut.Output.ShouldContain("Not found:");
    }

    [Test]
    public void Run_WithMalformedBookingReference_RepromptsUntilValid()
    {
        var booking = new ApartmentBooking(
            ConsoleMenuHarness.FirstUser.Id, ConsoleMenuHarness.Apartment.Id,
            new DateTime(2026, 1, 1), new DateTime(2026, 1, 5));

        using var sut = new ConsoleMenuHarness("6", "not-a-guid", booking.Id.ToString(), "0");
        sut.Bookings.Setup(r => r.GetById(booking.Id)).Returns(booking);

        sut.Menu.Run();

        sut.Output.ShouldContain("Not a valid reference.");
        booking.Status.ShouldBe(BookingStatus.Cancelled);
    }

    [Test]
    public void Run_WithUnknownMenuOption_WritesUnknownOption()
    {
        using var sut = new ConsoleMenuHarness("7", "0");

        sut.Menu.Run();

        sut.Output.ShouldContain("Unknown option.");
    }

    [Test]
    public void Run_WithExitOption_DoesNotTouchTheRepositories()
    {
        using var sut = new ConsoleMenuHarness("0");

        sut.Menu.Run();

        sut.Output.ShouldContain("=== Booking System ===");
        sut.Bookings.VerifyNoOtherCalls();
        sut.Users.VerifyNoOtherCalls();
    }

    [Test]
    public void Run_WhenInputStreamClosesMidPrompt_WritesOperationCancelled()
    {
        using var sut = new ConsoleMenuHarness("1");

        sut.Menu.Run();

        sut.Output.ShouldContain("Operation cancelled:");
        sut.Bookings.Verify(r => r.Add(It.IsAny<Booking>()), Times.Never);
    }
}
