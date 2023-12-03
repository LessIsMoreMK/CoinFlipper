namespace CoinFlipper.SwissArmy.Domain.Entities;

public class SentenceEntity
{
    public int Id { get; set; }
    
    public string Sentence { get; set; } = null!;

    public string Author { get; set; } = null!;
}