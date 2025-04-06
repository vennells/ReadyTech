using ReadyTech.API.Interfaces;

namespace ReadyTech.API.Mocks
{
    public class MockDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now { get; set; } = DateTime.Now;
    }
}
