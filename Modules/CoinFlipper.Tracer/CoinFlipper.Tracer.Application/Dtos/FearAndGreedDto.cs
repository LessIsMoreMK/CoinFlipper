namespace CoinFlipper.Tracer.Application.Dtos;

public class FearAndGreedDto
{
    public DateTime DateTime { get; set; }
    
    public int Value { get; set; }
    
    public string Classification { get; set; } = null!;
}