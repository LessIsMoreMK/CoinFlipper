using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoinFlipper.Shared.DateTimeHelpers;

public class DateTimeKindValueConverter : ValueConverter<DateTime, System.DateTime>
{
    public DateTimeKindValueConverter(DateTimeKind kind, ConverterMappingHints? mappingHints = null)
        : base(v => DateTime.SpecifyKind(v, kind), v => DateTime.SpecifyKind(v, kind), mappingHints)
    {
    }
}