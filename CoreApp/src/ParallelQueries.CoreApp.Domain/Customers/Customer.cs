using ParallelQueries.SharedKernel.Data;

namespace ParallelQueries.CoreApp.Domain.Customers;

public class Customer : EntityBase
{
    public Person Person { get; private set; } = default!;
    public DateOnly? Dob { get; private set; } = default!;

    private Customer() { }


    public Customer(Person person, DateOnly? dob, DateTime created)
    {
        Person = person;
        Dob = dob;
        Created = created;
    }
}
