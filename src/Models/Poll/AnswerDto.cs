namespace SensusAPI.Models.Poll;

public class AnswerDto {
    public required Guid AnswerId {get; set;}
    public required Guid QuestionId {get; set;}
    public required Guid PollId {get; set;}
    public required string Answer {get; set;}
}