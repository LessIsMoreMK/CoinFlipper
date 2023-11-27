using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.Extensions;

internal class DateTimeKindValueConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeKindValueConverter(DateTimeKind kind, ConverterMappingHints? mappingHints = null)
        : base(v => DateTime.SpecifyKind(v, kind), v => DateTime.SpecifyKind(v, kind), mappingHints)
    {
    }
}