using RPS.Shared.Mongo;

namespace RPS.Application.Dto.PlayerRating;

public class UpdatePlayerRatingDto
{
    public PlayerRatingDto PlayerRating { get; set; } = default!;
    public int RatingDifference { get; set; }
}