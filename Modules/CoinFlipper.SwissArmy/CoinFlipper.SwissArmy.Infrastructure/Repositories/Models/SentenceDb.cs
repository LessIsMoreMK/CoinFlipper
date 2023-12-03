namespace CoinFlipper.SwissArmy.Infrastructure.Repositories.Models;

public class SentenceDb
{
    public int Id { get; set; }
    
    public string Sentence { get; set; } = null!;

    public string Author { get; set; } = null!;
}