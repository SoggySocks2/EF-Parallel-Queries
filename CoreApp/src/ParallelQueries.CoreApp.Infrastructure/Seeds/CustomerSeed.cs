using ParallelQueries.CoreApp.Domain.Customers;

namespace ParallelQueries.CoreApp.Infrastructure.Seeds;

public static class CustomerSeed
{
    public static readonly string AdminRole = "Admin";

    public static List<Customer> GetCustomers()
    {
        var customers = new List<Customer>();
        var rng = new Random();

        for (var i = 0; i < 1000000; i++)
        {
            var dob = RandomDate(rng, minAge: 17, maxAge: 110);
            var created = RandomCreated();
            customers.Add(new Customer(GeneratePerson(i), dob, created));
        }

        return customers;
    }

    private static Person GeneratePerson(int counter)
    {
        var random = new Random();
        var isFemale = random.Next(2) == 1;
        if (isFemale)
            return GenerateFemale(counter);

        return GenerateMale(counter);
    }

    private static Person GenerateFemale(int counter)
    {
        var title = GenerateFemaleTitle(counter);
        var firstName = GenerateFemaleFirstName(counter);
        var lastName = GenerateLastName(counter);

        return new Person("Female", title, firstName, lastName);
    }
    private static Person GenerateMale(int counter)
    {
        var title = GenerateMaleTitle(counter);
        var firstName = GenerateMaleFirstName(counter);
        var lastName = GenerateLastName(counter);

        return new Person("Male", title, firstName, lastName);
    }
    private static string GenerateFemaleTitle(int counter)
    {
        if (counter % 3 == 0) return "Ms";
        if (counter % 5 == 0) return "Miss";

        return "Mrs";
    }
    private static string GenerateMaleTitle(int counter)
    {
        if (counter % 3 == 0) return "Master";
        if (counter % 5 == 0) return "Dr";

        return "Mr";
    }

    private static string GenerateFemaleFirstName(int counter)
    {
        if (counter % 3 == 0) return "Pauline";
        if (counter % 5 == 0) return "Emma";
        if (counter % 7 == 0) return "Sarah";
        if (counter % 11 == 0) return "Michelle";

        return "Diane";
    }
    private static string GenerateMaleFirstName(int counter)
    {
        if (counter % 3 == 0) return "John";
        if (counter % 5 == 0) return "Derek";
        if (counter % 7 == 0) return "Paul";

        return "David";
    }

    private static string GenerateLastName(int counter)
    {
        if (counter % 3 == 0) return "Williams";
        if (counter % 5 == 0) return "Humphreys";
        if (counter % 7 == 0) return "Johnson";
        if (counter % 11 == 0) return "Davies";
        if (counter % 13 == 0) return "Wilson";
        if (counter % 17 == 0) return "Smith";
        if (counter % 19 == 0) return "Edwards";

        return "Jones";
    }

    private static DateOnly RandomDate(Random random, int minAge, int maxAge)
    {
        ArgumentNullException.ThrowIfNull(random);
        ArgumentOutOfRangeException.ThrowIfNegative(minAge);
        ArgumentOutOfRangeException.ThrowIfLessThan(maxAge, minAge);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var maxBirth = today.AddYears(-minAge);
        var minBirth = today.AddYears(-maxAge);

        var minBirthDateTime = minBirth.ToDateTime(new TimeOnly(0, 0));
        var maxBirthDateTime = maxBirth.ToDateTime(new TimeOnly(0, 0));
        var rangeDays = (maxBirthDateTime - minBirthDateTime).Days;
        var offset = random.Next(0, rangeDays + 1);

        return minBirth.AddDays(offset);
    }

    private static DateTime RandomCreated()
    {
        var now = DateTime.UtcNow;
        var start = now.AddMonths(-24);

        var maxMillis = (long)(now - start).TotalMilliseconds;
        var offsetMillis = (long)(Random.Shared.NextDouble() * maxMillis);

        return start.AddMilliseconds(offsetMillis);
    }
}


