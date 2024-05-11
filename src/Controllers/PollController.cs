using System.Net;
using Microsoft.AspNetCore.Mvc;
using SensusAPI.Models;
using SensusAPI.Models.DB;
using SensusAPI.Models.Poll;

[ApiController]
[Route("/v1/[controller]")]
public class PollController : ControllerBase{
    private readonly SensusDbContext _dbContext;
    private readonly IPollHandler _pollHandler;
    private readonly ILogger _logger;

    public PollController(SensusDbContext dbCtx, ILogger<TestController> lgr, IPollHandler iphdlr){
        _dbContext = dbCtx;
        _logger = lgr;
        _pollHandler = iphdlr;
    }

    [HttpPost("basic")]
    public async Task<APIResponse> CreateBasicPoll([FromBody] CreatePollDto body){
        
        // check if expiry date is valid
        if (_pollHandler.IsValidExpiryDate(body.ExpiryDate)){
            _logger.LogInformation($"{DateTime.Now}: Expiry date invalid.");
            Response.StatusCode = 404;
            return new APIResponse(){
                Detail = "Invalid Expiry Date. Enter expiry date later than current date."
            };
        }

        // save
        var poll = await _pollHandler.CreateBasicPoll(body);

        return new APIResponse(){
            Detail = "Poll was created successfully.",
            Data = new {
                pollId = poll.Pollid
            }
        };
    }

    [HttpGet("{pollId}")]
    public async Task<APIResponse> RetrievePoll([FromRoute] Guid pollId){
        // get poll
        var poll = await _pollHandler.RetrieveBasicPoll(pollId);

        // check if poll exists
        if (poll == null){
            _logger.LogInformation($"{DateTime.Now}: Poll {pollId} not found.");
            Response.StatusCode = 404;
            return new APIResponse(){
                Detail = "Poll not found. Try a different poll id."
            };
        }

        _logger.LogInformation($"{DateTime.Now}: Retrieved poll {pollId}.");
        return new APIResponse(){
            Detail = "Poll found.",
            Data = poll
        };
    }

}