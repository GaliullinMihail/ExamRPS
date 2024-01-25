namespace RPS.Domain.Entities;

public class Room
{
    public string Id { get; set; } = default!;
    public string Owner { get; set; } = default!;
    public int MaxRating { get; set; }
    public DateTime CreationTime { get; set; }
    public string FirstPlayerId { get; set; } = default!;
    public string? SecondPlayerId { get; set; }
    
    public ICollection<Message> Messages { get; set; } = default!;
}