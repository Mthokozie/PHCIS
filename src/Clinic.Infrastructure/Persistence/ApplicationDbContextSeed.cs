namespace Clinic.Infrastructure.Persistence;

using Clinic.Domain.Entities;
using Clinic.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

/// <summary>
/// Seeds initial PHCIS roles, sample clinics, and upcoming time slots when the database is empty.
/// </summary>
public static class ApplicationDbContextSeed
{
    /// <summary>Applies pending migrations or creates schema and seeds initial PHCIS reference data.</summary>
    public static async Task SeedAsync(
        ApplicationDbContext context,
        RoleManager<IdentityRole> roleManager,
        ILogger logger)
    {
        try
        {
            await context.Database.MigrateAsync();

            string[] roles = ["Patient", "ClinicStaff", "Admin"];
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            if (!await context.Clinics.AnyAsync())
            {
                var clinics = new[]
                {
                    global::Clinic.Domain.Entities.Clinic.Create(
                        "Khayelitsha Site B Community Health Centre",
                        "Lwandle Rd, Site B, Khayelitsha, Cape Town, 7784",
                        "+27 21 360 5200",
                        "khayelitsha.chc@phcis.gov.za",
                        new TimeSpan(8, 0, 0),
                        new TimeSpan(16, 0, 0),
                        30),
                    global::Clinic.Domain.Entities.Clinic.Create(
                        "Mitchells Plain Community Health Centre",
                        "AZ Berman Dr, Mitchells Plain, Cape Town, 7785",
                        "+27 21 391 5820",
                        "mitchellsplain.chc@phcis.gov.za",
                        new TimeSpan(8, 0, 0),
                        new TimeSpan(16, 30, 0),
                        30),
                    global::Clinic.Domain.Entities.Clinic.Create(
                        "Gugulethu Community Day Centre",
                        "NY 3, Gugulethu, Cape Town, 7750",
                        "+27 21 637 1280",
                        "gugulethu.cdc@phcis.gov.za",
                        new TimeSpan(7, 30, 0),
                        new TimeSpan(16, 0, 0),
                        30)
                };

                await context.Clinics.AddRangeAsync(clinics);

                var slots = new List<TimeSlot>();
                for (int dayOffset = 0; dayOffset <= 7; dayOffset++)
                {
                    var date = DateTime.Today.AddDays(dayOffset);
                    foreach (var clinic in clinics)
                    {
                        var current = date.Add(clinic.OpeningTime);
                        var end = date.Add(clinic.ClosingTime);
                        var step = TimeSpan.FromMinutes(clinic.SlotDurationMinutes);
                        while (current.Add(step) <= end)
                        {
                            slots.Add(TimeSlot.Create(clinic.Id, current, current.Add(step), 5));
                            current = current.Add(step);
                        }
                    }
                }

                await context.TimeSlots.AddRangeAsync(slots);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {ClinicCount} PHCIS clinics and {SlotCount} time slots.", clinics.Length, slots.Count);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Database seeding skipped or encountered an issue.");
        }
    }
}
