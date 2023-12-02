namespace CoinFlipper.Shared.DateTimeHelpers;

public enum DateTimePrecision : long
{
    Millisecond = TimeSpan.TicksPerMillisecond, 
    Second = TimeSpan.TicksPerSecond, 
    Hour = TimeSpan.TicksPerHour, 
    Minute = TimeSpan.TicksPerMinute, 
    Day = TimeSpan.TicksPerDay
}