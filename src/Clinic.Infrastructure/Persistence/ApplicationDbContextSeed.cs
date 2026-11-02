namespace Clinic.Infrastructure.Persistence;

using Clinic.Domain.Entities;
using Clinic.Domain.Enums;
using Clinic.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

/// <summary>
/// Seeds initial PHCIS roles, uMkhanyakude District clinics, time slots, and default users (Admin & Clinic Staff).
/// </summary>
public static class ApplicationDbContextSeed
{
    /// <summary>Applies pending migrations and seeds initial reference data and users.</summary>
    public static async Task SeedAsync(
        ApplicationDbContext context,
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager,
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

            // Seed Zethu Vilane as Admin and Clinic Staff Manager
            var adminEmail = "zethu.vilane@phcis.health.gov.za";
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            var defaultClinic = await context.Clinics.FirstOrDefaultAsync(c => c.Name == "Jozini Clinic")
                                ?? await context.Clinics.FirstOrDefaultAsync();

            Guid? adminStaffId = null;
            if (defaultClinic != null)
            {
                var existingAdminStaff = await context.ClinicStaff.FirstOrDefaultAsync(s => s.Email == adminEmail);
                if (existingAdminStaff == null)
                {
                    existingAdminStaff = ClinicStaff.Create(
                        defaultClinic.Id,
                        "Zethu",
                        "Vilane",
                        adminEmail,
                        "+27 35 572 1000",
                        StaffRole.Manager,
                        "PHCIS-ADM-001");

                    await context.ClinicStaff.AddAsync(existingAdminStaff);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Seeded ClinicStaff entity for Admin/Manager: {Name}", existingAdminStaff.FullName);
                }
                adminStaffId = existingAdminStaff.Id;
            }

            if (existingAdmin == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    PhoneNumber = "+27 35 572 1000",
                    Role = "Admin",
                    ClinicStaffId = adminStaffId,
                    EmailConfirmed = true
                };

                var createAdminResult = await userManager.CreateAsync(adminUser, "Admin@12345");
                if (createAdminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    logger.LogInformation("Seeded Admin user: {Email}", adminEmail);
                }
            }
            else if (existingAdmin.ClinicStaffId == null && adminStaffId != null)
            {
                existingAdmin.ClinicStaffId = adminStaffId;
                await userManager.UpdateAsync(existingAdmin);
                logger.LogInformation("Updated existing Admin user with ClinicStaffId: {StaffId}", adminStaffId);
            }

            // Seed Sesi Zwane as Nurse at Jozini Clinic
            var nurseEmail = "sesi.zwane@kznhealth.gov.za";
            var existingNurseUser = await userManager.FindByEmailAsync(nurseEmail);
            var joziniClinic = await context.Clinics.FirstOrDefaultAsync(c => c.Name == "Jozini Clinic")
                               ?? await context.Clinics.FirstOrDefaultAsync();

            if (joziniClinic != null)
            {
                var existingStaff = await context.ClinicStaff.FirstOrDefaultAsync(s => s.Email == nurseEmail);
                if (existingStaff == null)
                {
                    existingStaff = ClinicStaff.Create(
                        joziniClinic.Id,
                        "Sesi",
                        "Zwane",
                        nurseEmail,
                        "+27 35 572 1042",
                        StaffRole.Nurse,
                        "SANC-2024-5542");

                    await context.ClinicStaff.AddAsync(existingStaff);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Seeded ClinicStaff entity for Nurse: {Name} at {Clinic}", existingStaff.FullName, joziniClinic.Name);
                }

                if (existingNurseUser == null)
                {
                    var nurseUser = new ApplicationUser
                    {
                        UserName = nurseEmail,
                        Email = nurseEmail,
                        PhoneNumber = "+27 35 572 1042",
                        Role = "ClinicStaff",
                        ClinicStaffId = existingStaff.Id,
                        EmailConfirmed = true
                    };

                    var createNurseResult = await userManager.CreateAsync(nurseUser, "Nurse@12345");
                    if (createNurseResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(nurseUser, "ClinicStaff");
                        logger.LogInformation("Seeded Identity user for Nurse: {Email}", nurseEmail);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Database seeding skipped or encountered an issue.");
        }
    }
}
