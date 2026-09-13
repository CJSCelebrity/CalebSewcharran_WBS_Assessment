using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WBS_Assessment.Application.Services;
using WBS_Assessment.Application.Utilities;
using WBS_Assessment.Application.Validators;

namespace WBS_Assessment.Application.ServiceRegistration;

public class ApplicationServiceRegistration
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(IServiceCollection services)
        {
            services.AddSingleton<BookingGuard>();
            services.AddSingleton<ApartmentBookingService>();
            services.AddSingleton<VehicleBookingService>();
            services.AddSingleton<ShowBookingService>();
            services.AddSingleton<BookingManagementService>();

            services.AddValidatorsFromAssemblyContaining<CreateApartmentBookingValidator>();
            return  services;
        }
    }
}