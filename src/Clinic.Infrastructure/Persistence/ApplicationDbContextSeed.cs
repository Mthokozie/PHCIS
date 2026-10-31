namespace Clinic.Infrastructure.Persistence;

using Clinic.Domain.Entities;
using Clinic.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

/// <summary>
/// Seeds initial PHCIS roles, uMkhanyakude District clinics, and upcoming time slots when the database is empty.
/// </summary>
public static class ApplicationDbContextSeed
{
    /// <summary>Applies pending migrations and seeds initial uMkhanyakude District PHCIS reference data.</summary>
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
                        "Jozini Clinic",
                        "Main Road, Jozini, uMkhanyakude District, KwaZulu-Natal, 3969",
                        "+27 35 572 1042",
                        "jozini.clinic@kznhealth.gov.za",
                        new TimeSpan(7, 30, 0),
                        new TimeSpan(16, 0, 0),
                        30),
                    global::Clinic.Domain.Entities.Clinic.Create(
                        "Msiyane CHC",
                        "Msiyane Area, Mkuze, uMkhanyakude District, KwaZulu-Natal, 3965",
                        "+27 35 573 1180",
                        "msiyane.chc@kznhealth.gov.za",
                        new TimeSpan(7, 0, 0),
                        new TimeSpan(16, 30, 0),
                        30),
                    global::Clinic.Domain.Entities.Clinic.Create(
                        "Ophondeni Clinic",
                        "Ophondweni Road, Mtubatuba, uMkhanyakude District, KwaZulu-Natal, 3935",
                        "+27 35 550 1420",
                        "ophondeni.clinic@kznhealth.gov.za",
                        new TimeSpan(8, 0, 0),
                        new TimeSpan(16, 0, 0),
                        30),
                    global::Clinic.Domain.Entities.Clinic.Create(
                        "Shemula Clinic",
                        "Shemula Reserve, Ingwavuma, uMkhanyakude District, KwaZulu-Natal, 3968",
                        "+27 35 591 0815",
                        "shemula.clinic@kznhealth.gov.za",
                        new TimeSpan(8, 0, 0),
                        new TimeSpan(16, 0, 0),
                        30),
                    global::Clinic.Domain.Entities.Clinic.Create(
                        "KwaMbuzi Clinic",
                        "KwaMbuzi Area, Jozini, uMkhanyakude District, KwaZulu-Natal, 3969",
                        "+27 35 572 1390",
                        "kwambuzi.clinic@kznhealth.gov.za",
                        new TimeSpan(8, 0, 0),
                        new TimeSpan(16, 0, 0),
                        30),
                    global::Clinic.Domain.Entities.Clinic.Create(
                        "Ndumo Clinic",
                        "Ndumo Reserve, Jozini, uMkhanyakude District, KwaZulu-Natal, 3972",
                        "+27 35 591 0612",
                        "ndumo.clinic@kznhealth.gov.za",
                        new TimeSpan(7, 30, 0),
                        new TimeSpan(16, 0, 0),
                        30),
                    global::Clinic.Domain.Entities.Clinic.Create(
                        "Mahlabeni Clinic",
                        "Mahlabeni Area, Mtubatuba, uMkhanyakude District, KwaZulu-Natal, 3935",
                        "+27 35 550 1834",
                        "mahlabeni.clinic@kznhealth.gov.za",
                        new TimeSpan(8, 0, 0),
                        new TimeSpan(16, 0, 0),
                        30),
                    global::Clinic.Domain.Entities.Clinic.Create(
                        "Manyise Clinic",
                        "Manyiseni Area, Ingwavuma, uMkhanyakude District, KwaZulu-Natal, 3968",
                        "+27 35 591 0940",
                        "manyise.clinic@kznhealth.gov.za",
                        new TimeSpan(8, 0, 0),
                        new TimeSpan(16, 0, 0),
                        30),
                    global::Clinic.Domain.Entities.Clinic.Create(
                        "Enkungwini Clinic",
                        "Enkungwini Area, Hlabisa, uMkhanyakude District, KwaZulu-Natal, 3937",
                        "+27 35 838 1125",
                        "enkungwini.clinic@kznhealth.gov.za",
                        new TimeSpan(8, 0, 0),
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
                logger.LogInformation("Seeded {ClinicCount} uMkhanyakude District clinics and {SlotCount} time slots.", clinics.Length, slots.Count);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Database seeding skipped or encountered an issue.");
        }
    }
}
