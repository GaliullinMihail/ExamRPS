namespace RPS.Application.Dto.Room;

public class RoomGetResponse
{
    public string FirstPlayerName { get; set; } = default!;
    public string SecondPlayerName { get; set; } = default!;
    public string RoomId { get; set; } = default!;
    public bool IsSuccess { get; set; }
}