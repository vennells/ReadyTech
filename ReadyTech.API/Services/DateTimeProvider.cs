using ReadyTech.API.Interfaces;

namespace ReadyTech.API.Services;
public class DateTimeProvider: IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}