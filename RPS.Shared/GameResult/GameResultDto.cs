namespace RPS.Shared.GameResult;

public class GameResultDto
{
    public bool Draw { get; set; }
    public string WinnerId { get; set; } = default!;
    public string LooserId { get; set; } = default!;
}