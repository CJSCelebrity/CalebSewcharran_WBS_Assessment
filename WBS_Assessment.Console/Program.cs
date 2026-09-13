
/*
 * The following WBS assessment entails that
 * this be built for bookings - people going on holiday
 *
 * Ranging from a variety of bookings. Apartments, Vehicles, Shows etc
 * There will always be availability of whatever needs to be booked
 *
 * Basic Solution:
 * Allow someone to add a booking, edit, delete (CRUD)
 * Add a unit test or frontend to show that the application works
 *
 * Swagger api - extensible choice (PUT, POST, DELETE, GET)
 *
 * Console application for frontend requirements
 *
 * Unit tests in conjunction to those requirements
 *
 * DESIGN.md doc in the docs file with a draw.io doc
 *
 *
 */

using Microsoft.Extensions.DependencyInjection;
using WBS_Assessment.Application.ServiceRegistration;
using WBS_Assessment.Console;
using WBS_Assessment.Infrastructure.ServiceRegistration;

var services = new ServiceCollection()
    .AddApplication()
    .AddInfrastructure();

services.AddSingleton<ConsoleMenu>();

using var provider = services.BuildServiceProvider();

provider.GetRequiredService<ConsoleMenu>().Run();