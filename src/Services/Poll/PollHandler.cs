using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SensusAPI.Models.DB;
using SensusAPI.Models.Poll;

public class PollHandler : IPollHandler{
    private readonly SensusDbContext _dbContext;
    private readonly ILogger _logger;

    public PollHandler(SensusDbContext dbCtx, ILogger<TestController> lgr){
        _dbContext = dbCtx;
        _logger = lgr;
    }

    public bool IsValidExpiryDate(DateTime expiryDate){
        // idea: don't allow expiries of just a few minutes from now
            // e.g. posted at 1PM and expiry is at 1:02PM
        // check if expiry date is already finished
        if (DateTime.Now > expiryDate){
            return false;
        }
        return true;
    }
    public async Task<Poll> CreateBasicPoll(CreatePollDto cpDto){
        // create new poll object
        var poll = new Poll();
        poll.Title = cpDto.Title;
        poll.Description = cpDto.Description;
        poll.ExpiryDate = cpDto.ExpiryDate;
        await _dbContext.Polls.AddAsync(poll);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now}: Created poll {poll.Pollid}.");

        // create new question object
        var question = new Question();
        question.Question1 = cpDto.Question;
        question.Pollid = poll.Pollid;
        await _dbContext.Questions.AddAsync(question);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now}: Created quesiton {question.Questionid}.");

        return poll;
    }
    public async Task<RetrieveBasicPollDto?> RetrieveBasicPoll(Guid pollId){
        var poll = await (from p in _dbContext.Polls
                          join q in _dbContext.Questions on p.Pollid equals q.Pollid
                          where p.Pollid == pollId
                          select new RetrieveBasicPollDto(){
                            Title = p.Title,
                            Description = p.Description,
                            CreatedDate = (DateTime)p.CreatedDate,
                            ExpiryDate = p.ExpiryDate,
                            Question = q.Question1
                          }
                         ).FirstOrDefaultAsync();
        return poll;
    }
    public async Task<Dictionary<Guid, ImmutableList<String>>> RetrievePollAnswers(Guid pollId){
        // get list of answers attributed to poll `poll id`
        var answers = await _dbContext.Answers.Where(a => a.Pollid == pollId).ToListAsync();
        
        // get unique set of questions
        var questionIds = answers.Select(a => a.Questionid).Distinct().ToImmutableList();

        // initialize grouped answers
        var groupedAnswers = new Dictionary<Guid, ImmutableList<String>>();
        
        // group by question id
        questionIds.ForEach(q => {
            // create new array per question containing answers to each question
            var qAnswers = answers.Where(a => a.Questionid == q).Select(a => a.Ans).ToImmutableList();
            // append grouped result to answers list
            groupedAnswers.Add(q, qAnswers);
        });
        
        // return result
        return groupedAnswers;
    }
}