namespace Clinic.Infrastructure;

using Clinic.Application.Common.Interfaces;
using Clinic.Infrastructure.Identity;
using Clinic.Infrastructure.Persistence;
using Clinic.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> to register all Infrastructure
/// services including EF Core, ASP.NET Identity, and repository implementations.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the Infrastructure layer services into the DI container.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">Application configuration used to retrieve the connection string.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddInfrastructure( this IServiceCollection services, IConfiguration configuration)
    {
        // Register EF Core with SQL Server using the "DefaultConnection" connection string.
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // Expose ApplicationDbContext via the IApplicationDbContext abstraction.
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        // Expose ApplicationDbContext via the IUnitOfWork abstraction (SaveChangesAsync).
        services.AddScoped<IUnitOfWork>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        // Register domain repository implementations.
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IClinicRepository, ClinicRepository>();
        services.AddScoped<ITimeSlotRepository, TimeSlotRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        // Configure ASP.NET Core Identity backed by EF Core.
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
