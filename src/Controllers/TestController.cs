using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SensusAPI.Models.DB;

[ApiController]
[Route("/test")]
public class TestController : ControllerBase{
    
    private readonly SensusDbContext _dbContext;
    private readonly ILogger _logger;

    public TestController(SensusDbContext dbCtx, ILogger<TestController> lgr){
        _dbContext = dbCtx;
        _logger = lgr;
    }

    [HttpGet]
    public async Task<int> Index(){
        _logger.LogInformation("Attained poll count.");

        var pollCount = await _dbContext.Polls.CountAsync();

        return pollCount;
    }
}