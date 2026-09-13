using System.Globalization;
using FluentValidation;
using WBS_Assessment.Application.Dto;
using WBS_Assessment.Application.Exceptions;
using WBS_Assessment.Application.Repositories;
using WBS_Assessment.Application.Services;
using WBS_Assessment.Core.Interfaces;
using WBS_Assessment.Core.Models;

namespace WBS_Assessment.Console;

/// <summary>
/// Console front end. The following has been adapted from the following links
/// - https://medium.com/@kenslearningcurve/workshop-build-a-console-menu-in-c-9828ee88795f, https://codereview.stackexchange.com/questions/288207/best-practices-for-a-console-menu-app
/// </summary>
public class ConsoleMenu(
    ApartmentBookingService apartments,
    VehicleBookingService vehicles,
    ShowBookingService shows,
    BookingManagementService management,
    IRepository<Apartment> apartmentCatalogue,
    IRepository<Vehicle> vehicleCatalogue,
    IRepository<Show> showCatalogue,
    IUserRepository users)
{
    public void Run()
    {
        while (true)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("=== Booking System ===");
            System.Console.WriteLine("1. Book an apartment");
            System.Console.WriteLine("2. Book a vehicle");
            System.Console.WriteLine("3. Book a show");
            System.Console.WriteLine("4. View bookings for a user");
            System.Console.WriteLine("5. Amend a booking");
            System.Console.WriteLine("6. Cancel a booking");
            System.Console.WriteLine("0. Exit");
            System.Console.Write("Choose an option: ");
 
            var choice = System.Console.ReadLine();
            if (choice is null) 
                return;
            
            try
            {
                switch (choice)
                {
                    case "1": BookApartment(); break;
                    case "2": BookVehicle(); break;
                    case "3": BookShow(); break;
                    case "4": ViewBookings(); break;
                    case "5": AmendBooking(); break;
                    case "6": CancelBooking(); break;
                    case "0": return;
                    default: System.Console.WriteLine("Unknown option."); break;
                }
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                    System.Console.WriteLine($"Invalid input: {error.ErrorMessage}");
            }
            catch (NotFoundException ex)
            {
                System.Console.WriteLine($"Not found: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                // Domain rule violation, e.g. amending a cancelled booking.
                System.Console.WriteLine($"Cannot do that: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                System.Console.WriteLine($"Cannot do that: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                System.Console.WriteLine($"Operation cancelled: {ex.Message}");
            }
        }
    }

    #region Booking Helpers

    private void BookApartment()
    {
        var userId = SelectUser();
        var itemId = SelectItem(apartmentCatalogue.GetAll(), "apartment");
        var checkIn = PromptForDate("Check-in date");
        var checkOut = PromptForDate("Check-out date");
 
        var id = apartments.Create(
            new CreateApartmentBookingRequest(userId, itemId, checkIn, checkOut));
 
        System.Console.WriteLine($"Booked. Reference: {id}");
    }
 
    private void BookVehicle()
    {
        var userId = SelectUser();
        var itemId = SelectItem(vehicleCatalogue.GetAll(), "vehicle");
        var pickup = PromptForDate("Pickup date");
        var dropoff = PromptForDate("Drop-off date");
 
        var id = vehicles.Create(
            new CreateVehicleBookingRequest(userId, itemId, pickup, dropoff));
 
        System.Console.WriteLine($"Booked. Reference: {id}");
    }
 
    private void BookShow()
    {
        var userId = SelectUser();
        var itemId = SelectItem(showCatalogue.GetAll(), "show");
        var performance = PromptForDate("Performance date");
 
        var id = shows.Create(
            new CreateShowBookingRequest(userId, itemId, performance));
 
        System.Console.WriteLine($"Booked. Reference: {id}");
    }
 
    private void ViewBookings()
    {
        var userId = SelectUser();
        var bookings = management.GetForUser(userId);
 
        if (bookings.Count == 0)
        {
            System.Console.WriteLine("No bookings for this user.");
            return;
        }
 
        foreach (var booking in bookings)
            System.Console.WriteLine(Describe(booking));
    }
 
    private void AmendBooking()
    {
        var bookingId = PromptForGuid("Booking reference");
        var booking = management.GetById(bookingId);
 
        switch (booking)
        {
            case ApartmentBooking:
                apartments.Reschedule(bookingId,
                    PromptForDate("New check-in date"),
                    PromptForDate("New check-out date"));
                break;
 
            case VehicleBooking:
                vehicles.Reschedule(bookingId,
                    PromptForDate("New pickup date"),
                    PromptForDate("New drop-off date"));
                break;
 
            case ShowBooking:
                shows.Reschedule(bookingId,
                    PromptForDate("New performance date"));
                break;
        }
 
        System.Console.WriteLine("Booking amended.");
    }
 
    private void CancelBooking()
    {
        var bookingId = PromptForGuid("Booking reference");
        management.Cancel(bookingId);
        System.Console.WriteLine("Booking cancelled.");
    }

    #endregion

    #region Input Helpers

    private Guid SelectUser()
    {
        var userList = users.GetAll().ToList();
        System.Console.WriteLine("Users:");
        for (var i = 0; i < userList.Count; i++)
            System.Console.WriteLine($"  {i + 1}. {userList[i].DisplayName}");
 
        return userList[PromptForIndex("Select user", userList.Count)].Id;
    }
 
    private static Guid SelectItem<T>(IReadOnlyCollection<T> items, string label)
        where T : IBookingInformation
    {
        var list = items.ToList();
        System.Console.WriteLine($"Available {label}s:");
        for (var i = 0; i < list.Count; i++)
            System.Console.WriteLine($"  {i + 1}. {list[i].Name}");
 
        return list[PromptForIndex($"Select {label}", list.Count)].Id;
    }
 
    private static int PromptForIndex(string label, int count)
    {
        while (true)
        {
            System.Console.Write($"{label} (1-{count}): ");
            var input = System.Console.ReadLine();
            
            if (input is null)
                throw new OperationCanceledException("Input stream closed.");
            
            if (int.TryParse(System.Console.ReadLine(), out var choice)
                && choice >= 1 && choice <= count)
                return choice - 1;
 
            System.Console.WriteLine($"Please enter a number between 1 and {count}.");
        }
    }
 
    private static DateTime PromptForDate(string label)
    {
        while (true)
        {
            System.Console.Write($"{label} (yyyy-MM-dd): ");
            var input = System.Console.ReadLine();

            if (input is null)
                throw new OperationCanceledException("Input stream closed.");

            if (DateTime.TryParseExact(input, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                return date;

            System.Console.WriteLine("Not a valid date. Please use yyyy-MM-dd.");
        }
    }
 
    private static Guid PromptForGuid(string label)
    {
        while (true)
        {
            System.Console.Write($"{label}: ");
            var input = System.Console.ReadLine();
            
            if (input is null)
                throw new OperationCanceledException("Input stream closed.");
            
            if (Guid.TryParse(System.Console.ReadLine(), out var id))
                return id;
 
            System.Console.WriteLine("Not a valid reference.");
        }
    }

    #endregion
 
    private static string Describe(Booking booking)
    {
        var dates = booking switch
        {
            ApartmentBooking a => $"{a.CheckIn:yyyy-MM-dd} to {a.CheckOut:yyyy-MM-dd}",
            VehicleBooking v => $"{v.Pickup:yyyy-MM-dd} to {v.Dropoff:yyyy-MM-dd}",
            ShowBooking s => $"{s.PerformanceTime:yyyy-MM-dd}",
            _ => "unknown"
        };
 
        return $"  {booking.Id} | {booking.GetType().Name} | {dates} | {booking.Status}";
    }
}
