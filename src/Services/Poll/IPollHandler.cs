
using SensusAPI.Models.DB;
using SensusAPI.Models.Poll;

public interface IPollHandler{
    /// <summary>
    /// Validates expiry date.
    /// </summary>
    /// <param name="expiryDate"></param>
    /// <returns></returns>
    public bool IsValidExpiryDate(DateTime expiryDate);
    /// <summary>
    /// Creates a new basic poll.
    /// </summary>
    /// <param name="cpDto"></param>
    /// <returns></returns>
    public Task<Poll> CreateBasicPoll(CreatePollDto cpDto);
    /// <summary>
    /// Retrieves a poll. Returns null when not found.
    /// </summary>
    /// <param name="pollId"></param>
    /// <returns></returns>
    public Task<RetrieveBasicPollDto?> RetrieveBasicPoll(Guid pollId);

}