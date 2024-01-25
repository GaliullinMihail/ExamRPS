using RPS.Application.Dto.ResponsesAbstraction;

namespace RPS.Application.Dto.Authentication.Login;

public class LoginResponseDto: ResponseBaseDto
{
    private readonly Dictionary<LoginResponseStatus, string> _messages = new()
    {
        {LoginResponseStatus.Ok, "Login successful"},
        {LoginResponseStatus.Fail, "Invalid login attempt"}
    };
    
    private readonly Dictionary<LoginResponseStatus, int> _codes = new()
    {
        {LoginResponseStatus.Ok, 200},
        {LoginResponseStatus.Fail, 400}
    };
    
    public LoginResponseDto(LoginResponseStatus status, string? message="")
    {
        Message = message != "" ? message : _messages[status];
        Successful = status == LoginResponseStatus.Ok;
        StatusCode = _codes[status];
    }
}

