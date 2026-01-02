using Microsoft.AspNetCore.Mvc;
using ParallelQueries.CoreApp.Api.Customers;
using ParallelQueries.CoreApp.Models.Customers;
using Swashbuckle.AspNetCore.Annotations;

namespace ParallelQueries.Api.Customers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController(ICustomerService customerService) : ControllerBase
{
    [HttpGet("breakdown-by-age/parallel")]
    [SwaggerOperation(Summary = "Get all customers stats using queries in parallel", Tags = ["Customer"])]
    public async Task<ActionResult<CustomerBreakdownByAgeResponse>> GetBreakdownInParallelAsync(CancellationToken cancellationToken = default)
    {
        var result = await customerService.GetCustomerBreakdownAsync(QueryMode.InParallel, cancellationToken);
        return Ok(result);
    }

    [HttpGet("breakdown-by-age/series")]
    [SwaggerOperation(Summary = "Get all customers stats using queries in series", Tags = ["Customer"])]
    public async Task<ActionResult<CustomerBreakdownByAgeResponse>> GetBreakdownInSeriesAsync(CancellationToken cancellationToken = default)
    {
        var result = await customerService.GetCustomerBreakdownAsync(QueryMode.InSeries, cancellationToken);
        return Ok(result);
    }

    [HttpGet("breakdown-by-age/both")]
    [SwaggerOperation(Summary = "Get all customers stats using queries in parallel and in series", Tags = ["Customer"])]
    public async Task<ActionResult<List<CustomerBreakdownByAgeResponse>>> GetBreakdownInParallelAndSeriesAsync(CancellationToken cancellationToken = default)
    {
        var resultInParallel = await customerService.GetCustomerBreakdownAsync(QueryMode.InParallel, cancellationToken);
        var resultInSeries = await customerService.GetCustomerBreakdownAsync(QueryMode.InSeries, cancellationToken);
        var result = new List<CustomerBreakdownByAgeResponse>
        {
            resultInParallel,
            resultInSeries
        };
        return Ok(result);
    }
}
