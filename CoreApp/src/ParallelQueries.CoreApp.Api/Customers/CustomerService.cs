using Microsoft.EntityFrameworkCore;
using ParallelQueries.CoreApp.Infrastructure.Customers;
using ParallelQueries.CoreApp.Models.Customers;

namespace ParallelQueries.CoreApp.Api.Customers;

public class CustomerService(IParallelCustomerQuery parallelCustomerQuery,
    ISeriesCustomerQuery seriesCustomerQuery) : ICustomerService
{
    public async Task<CustomerBreakdownByAgeResponse> GetCustomerBreakdownAsync(QueryMode queryMode, CancellationToken cancellation)
    {
        var response = new CustomerBreakdownByAgeResponse();
        var now = DateTime.UtcNow;
        var previousMonthEnd = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMicroseconds(-1);
        response.ReportExecutionStart = now;

        if (queryMode == QueryMode.InParallel)
            await GetCustomerBreakdownInParallelAsync(response, now, previousMonthEnd, cancellation);
        else
            await GetCustomerBreakdownInSeriesAsync(response, now, previousMonthEnd, cancellation);

        response.ReportExecutionEnd = DateTime.UtcNow;
        response.ReportExecutionDurationInSeconds = CalculateSecondsElapsed(response.ReportExecutionStart, response.ReportExecutionEnd);

        return response;
    }

    private async Task GetCustomerBreakdownInParallelAsync(CustomerBreakdownByAgeResponse response, DateTime now, DateTime previousMonthEnd, CancellationToken cancellation)
    {
        response.QueryMode = "InParallel";

        var t1 = CustomerByMaxCreatedCount(QueryMode.InParallel, previousMonthEnd, cancellation);
        var t2 = CustomerByMaxCreatedCount(QueryMode.InParallel, now, cancellation);

        var t3 = CustomerByMaxCreatedAndGenderCount(QueryMode.InParallel, previousMonthEnd, "Male", cancellation);
        var t4 = CustomerByMaxCreatedAndGenderCount(QueryMode.InParallel, now, "Male", cancellation);

        var t5 = CustomerByMaxCreatedAndGenderCount(QueryMode.InParallel, previousMonthEnd, "Female", cancellation);
        var t6 = CustomerByMaxCreatedAndGenderCount(QueryMode.InParallel, now, "Female", cancellation);

        var t7 = YoungCustomerByMaxCreatedCount(QueryMode.InParallel, previousMonthEnd, cancellation);
        var t8 = YoungCustomerByMaxCreatedCount(QueryMode.InParallel, now, cancellation);

        var t9 = MiddleAgedCustomerByMaxCreatedCount(QueryMode.InParallel, previousMonthEnd, cancellation);
        var t10 = MiddleAgedCustomerByMaxCreatedCount(QueryMode.InParallel, now, cancellation);

        var t11 = OldCustomerByMaxCreatedCount(QueryMode.InParallel, previousMonthEnd, cancellation);
        var t12 = OldCustomerByMaxCreatedCount(QueryMode.InParallel, now, cancellation);

        await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t7, t7, t10, t11, t12);

        response.PreviousMonth.All = t1.Result;
        response.CurrentMonth.All = t2.Result;
        response.Diffs.All = response.CurrentMonth.All - response.PreviousMonth.All;

        response.PreviousMonth.Male = t3.Result;
        response.CurrentMonth.Male = t4.Result;
        response.Diffs.Male = response.CurrentMonth.Male - response.PreviousMonth.Male;

        response.PreviousMonth.Female = t5.Result;
        response.CurrentMonth.Female = t6.Result;
        response.Diffs.Female = response.CurrentMonth.Female - response.PreviousMonth.Female;

        response.PreviousMonth.Young = t7.Result;
        response.CurrentMonth.Young = t8.Result;
        response.Diffs.Young = response.CurrentMonth.Young - response.PreviousMonth.Young;

        response.PreviousMonth.MiddleAged = t9.Result;
        response.CurrentMonth.MiddleAged = t10.Result;
        response.Diffs.MiddleAged = response.CurrentMonth.MiddleAged - response.PreviousMonth.MiddleAged;

        response.PreviousMonth.Old = t11.Result;
        response.CurrentMonth.Old = t12.Result;
        response.Diffs.Old = response.CurrentMonth.Old - response.PreviousMonth.Old;
    }

    private async Task GetCustomerBreakdownInSeriesAsync(CustomerBreakdownByAgeResponse response, DateTime now, DateTime previousMonthEnd, CancellationToken cancellation)
    {
        response.QueryMode = "InSeries";

        response.PreviousMonth.All = await CustomerByMaxCreatedCount(QueryMode.InSeries, previousMonthEnd, cancellation);
        response.CurrentMonth.All = await CustomerByMaxCreatedCount(QueryMode.InSeries, now, cancellation);
        response.Diffs.All = response.CurrentMonth.All - response.PreviousMonth.All;

        response.PreviousMonth.Male = await CustomerByMaxCreatedAndGenderCount(QueryMode.InSeries, previousMonthEnd, "Male", cancellation);
        response.CurrentMonth.Male = await CustomerByMaxCreatedAndGenderCount(QueryMode.InSeries, now, "Male", cancellation);
        response.Diffs.Male = response.CurrentMonth.Male - response.PreviousMonth.Male;

        response.PreviousMonth.Female = await CustomerByMaxCreatedAndGenderCount(QueryMode.InSeries, previousMonthEnd, "Female", cancellation);
        response.CurrentMonth.Female = await CustomerByMaxCreatedAndGenderCount(QueryMode.InSeries, now, "Female", cancellation);
        response.Diffs.Female = response.CurrentMonth.Female - response.PreviousMonth.Female;

        response.PreviousMonth.Young = await YoungCustomerByMaxCreatedCount(QueryMode.InSeries, previousMonthEnd, cancellation);
        response.CurrentMonth.Young = await YoungCustomerByMaxCreatedCount(QueryMode.InSeries, now, cancellation);
        response.Diffs.Young = response.CurrentMonth.Young - response.PreviousMonth.Young;

        response.PreviousMonth.MiddleAged = await MiddleAgedCustomerByMaxCreatedCount(QueryMode.InSeries, previousMonthEnd, cancellation);
        response.CurrentMonth.MiddleAged = await MiddleAgedCustomerByMaxCreatedCount(QueryMode.InSeries, now, cancellation);
        response.Diffs.MiddleAged = response.CurrentMonth.MiddleAged - response.PreviousMonth.MiddleAged;

        response.PreviousMonth.Old = await OldCustomerByMaxCreatedCount(QueryMode.InSeries, previousMonthEnd, cancellation);
        response.CurrentMonth.Old = await OldCustomerByMaxCreatedCount(QueryMode.InSeries, now, cancellation);
        response.Diffs.Old = response.CurrentMonth.Old - response.PreviousMonth.Old;
    }

    private async Task<int> CustomerByMaxCreatedCount(QueryMode queryMode, DateTime maxCreated, CancellationToken cancellation)
    {
        var qry = await CustomerQuery(queryMode)
            .GetCustomerByMaxCreatedAsync(maxCreated);

        return await qry
            .Select(c => c.Id)
            .CountAsync(cancellation);
    }

    private async Task<int> CustomerByMaxCreatedAndGenderCount(QueryMode queryMode, DateTime maxCreated, string gender, CancellationToken cancellation)
    {
        var qry = await CustomerQuery(queryMode)
            .GetCustomerByMaxCreatedAndGenderAsync(maxCreated, gender);

        return await qry
            .Select(c => c.Id)
            .CountAsync(cancellation);
    }
    private async Task<int> YoungCustomerByMaxCreatedCount(QueryMode queryMode, DateTime maxCreated, CancellationToken cancellation)
    {
        var qry = await CustomerQuery(queryMode)
            .GetCustomerByMaxCreatedBetweenMinAndMaxAgeAsync(maxCreated, 0, 25);

        return await qry
            .Select(c => c.Id)
            .CountAsync(cancellation);
    }
    private async Task<int> MiddleAgedCustomerByMaxCreatedCount(QueryMode queryMode, DateTime maxCreated, CancellationToken cancellation)
    {
        var qry = await CustomerQuery(queryMode)
            .GetCustomerByMaxCreatedBetweenMinAndMaxAgeAsync(maxCreated, 26, 55);

        return await qry
            .Select(c => c.Id)
            .CountAsync(cancellation);
    }
    private async Task<int> OldCustomerByMaxCreatedCount(QueryMode queryMode, DateTime maxCreated, CancellationToken cancellation)
    {
        var qry = await CustomerQuery(queryMode)
            .GetCustomerByMaxCreatedBetweenMinAndMaxAgeAsync(maxCreated, 56, 1000);

        return await qry
            .Select(c => c.Id)
            .CountAsync(cancellation);
    }
    private static int CalculateSecondsElapsed(DateTime start, DateTime end)
    {
        return (int)(end - start).TotalSeconds;
    }
    private ICustomerQuery CustomerQuery(QueryMode queryMode)
    {
        return queryMode == QueryMode.InParallel
            ? parallelCustomerQuery
            : seriesCustomerQuery;
    }
}