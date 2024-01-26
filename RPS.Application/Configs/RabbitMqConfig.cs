namespace RPS.Application.Configs;

public class RabbitMqConfig
{
    public string Username { get; set; } = default!;
    public string Hostname { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Port { get; set; } = default!;
}