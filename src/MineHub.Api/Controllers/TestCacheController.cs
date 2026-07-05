using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace MineHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestCacheController : ControllerBase
{
    private readonly IDistributedCache _cache;

    public TestCacheController(IDistributedCache distributedCache)
    {
        _cache = distributedCache;
    }

    [HttpGet]
    public async Task<IActionResult> Test()
    {
        //await _cache.SetStringAsync("test2", "hello redis2");

        var value = await _cache.GetStringAsync("test2");

        return Ok(value);
    }
}
