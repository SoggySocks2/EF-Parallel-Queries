namespace ParallelQueries.CoreApp.Domain.Customers;

public class Person
{
    public string Gender { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;

    private Person() { }

    public Person(string gender, string title, string firstName, string lastName)
    {
        Gender = gender;
        Title = title;
        FirstName = firstName;
        LastName = lastName;
    }
}
