

namespace LukNotificator.Entities
{
    internal class OwnCurrencyDb
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public Guid UserId { get; set; }

        public string Props { get; set; }
    }
}
