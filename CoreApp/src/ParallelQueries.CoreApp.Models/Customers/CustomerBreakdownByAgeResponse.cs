namespace ParallelQueries.CoreApp.Models.Customers;

public record CustomerBreakdownByAgeResponse
{
    public string QueryMode { get; set; } = string.Empty;
    public DateTime ReportExecutionStart { get; set; }
    public DateTime ReportExecutionEnd { get; set; }
    public int ReportExecutionDurationInSeconds { get; set; }

    public List<string> Labels { get; set; } = [
        "All",
        "Male",
        "Female",
        "Young",
        "MiddleAged",
        "Old"
    ];

    public CustomerBreakdownByAgeModel CurrentMonth { get; set; } = new CustomerBreakdownByAgeModel();
    public CustomerBreakdownByAgeModel PreviousMonth { get; set; } = new CustomerBreakdownByAgeModel();
    public CustomerBreakdownByAgeModel Diffs { get; set; } = new CustomerBreakdownByAgeModel();
}

public enum QueryMode
{
    InSeries,
    InParallel
}
