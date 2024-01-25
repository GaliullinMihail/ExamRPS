using Microsoft.AspNetCore.Identity;

namespace RPS.Domain.Entities;

public class Role: IdentityRole
{
    public int LikesCountAllowed { get; set; }
    public bool LocationViewAllowed { get; set; }
    
}