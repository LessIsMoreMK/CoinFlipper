namespace CoinFlipper.Tracer.Domain.Entities;

public class FearAndGreed
{
    public DateTime DateTime { get; set; }

    public int Value { get; set; }

    public string Classification { get; set; }

    public FearAndGreed(DateTime dateTime, int value, string classification)
    {
        DateTime = dateTime;
        Value = value;
        Classification = classification;
    }
}