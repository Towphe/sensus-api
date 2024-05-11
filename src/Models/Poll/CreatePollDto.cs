namespace SensusAPI.Models.Poll;

public class CreatePollDto {
    public required string Title {get; set;}
    public required string Description {get; set;}
    public required DateTime ExpiryDate {get; set;}
    public required string Question {get; set;}
}