namespace ParallelQueries.CoreApp.Models.Customers;

public record CustomerBreakdownByAgeModel
{
    public int All { get; set; } = 0;
    public int Male { get; set; } = 0;
    public int Female { get; set; } = 0;
    public int Young { get; set; } = 0;
    public int MiddleAged { get; set; } = 0;
    public int Old { get; set; } = 0;
}