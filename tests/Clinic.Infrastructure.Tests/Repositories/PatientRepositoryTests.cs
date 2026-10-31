namespace Clinic.Infrastructure.Tests.Repositories;

using Clinic.Domain.Entities;
using Clinic.Infrastructure.Persistence;
using Clinic.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class PatientRepositoryTests
{
    private static ApplicationDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task GetByEmailAsync_WhenPatientExists_ShouldReturnPatient()
    {
        using var context = CreateInMemoryContext();
        var repo = new PatientRepository(context);

        var patient = Patient.Create("John", "Doe", "john@test.com", "+2782", new DateTime(1990, 1, 1), "ID001");
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        var result = await repo.GetByEmailAsync("john@test.com");
        result.Should().NotBeNull();
        result!.Email.Should().Be("john@test.com");
    }

    [Fact]
    public async Task GetByEmailAsync_WhenPatientDoesNotExist_ShouldReturnNull()
    {
        using var context = CreateInMemoryContext();
        var repo = new PatientRepository(context);

        var result = await repo.GetByEmailAsync("nonexistent@test.com");
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPatients()
    {
        using var context = CreateInMemoryContext();
        var repo = new PatientRepository(context);

        context.Patients.Add(Patient.Create("A", "B", "a@b.com", "+1", new DateTime(2000, 1, 1), "ID1"));
        context.Patients.Add(Patient.Create("C", "D", "c@d.com", "+2", new DateTime(2000, 1, 1), "ID2"));
        await context.SaveChangesAsync();

        var result = await repo.GetAllAsync();
        result.Should().HaveCount(2);
    }
}
