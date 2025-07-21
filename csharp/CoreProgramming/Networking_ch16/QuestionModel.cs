namespace Networking_ch16;

public class QuestionModel
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public double Score { get; set; }
    public string? ImagePath { get; set; }
    public string? Explanation { get; set; }
    public string? Feedback { get; set; }
    public string SubjectType { get; set; } = string.Empty;
    public string DifficultyLevel { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public List<AnswerModel> Answers { get; set; } = new();
    public string? SessionIdentifier { get; set; }
}

public class AnswerModel
{
    public string Text { get; set; } = string.Empty;
    public char Variant { get; set; }
    public bool IsCorrect { get; set; }
}
