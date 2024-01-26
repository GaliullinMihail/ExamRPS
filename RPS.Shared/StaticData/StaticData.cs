namespace RPS.Shared.StaticData;

public static class StaticData
{
    public static readonly int GamesPerPage = 50;
    public static readonly int PointsForWinner = 3;
    public static readonly int PointsForLooser = -1;
    public static readonly int PointsForDraw = 1;
    public static readonly Dictionary<string, List<string>> GameHubConnections = new Dictionary<string, List<string>>(); 
}