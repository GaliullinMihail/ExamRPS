namespace RPS.Shared.GameResult;

public class GameResultDto
{
    public bool Draw { get; set; }
    public string WinnerNickName { get; set; } = default!;
    public string LooserNickName { get; set; } = default!;
}