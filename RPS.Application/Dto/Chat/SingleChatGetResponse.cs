namespace RPS.Application.Dto.Chat;

public class SingleChatGetResponse
{
    public string SenderName { get; set; } = default!;
    public string ReceiverName { get; set; } = default!;
    public string RoomName { get; set; } = default!;
}