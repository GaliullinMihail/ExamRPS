namespace RPS.Domain.Entities;

public class Room
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string FirstUserId { get; set; } = default!;
    public string SecondUserId { get; set; } = default!;
    public ICollection<Message> Messages { get; set; } = default!;
}