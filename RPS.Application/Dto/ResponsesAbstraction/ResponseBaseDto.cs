namespace RPS.Application.Dto.ResponsesAbstraction;

public abstract class ResponseBaseDto
{
    public int StatusCode { get; set; }
    public bool Successful { get; set; }
    
    public string? Message { get; set; }
}