using System.ComponentModel.DataAnnotations;

namespace RPS.Application.Dto.Authentication.Register;

public class RegisterRequestDto
{
    [Required]
    [Display(Name = "Nickname")]
    public string UserName { get; set; } = default!;

    [Required]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;
}