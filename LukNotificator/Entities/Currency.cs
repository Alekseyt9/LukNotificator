

namespace LukNotificator.Entity
{
    public class Currency
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public double Price { get; set; }

        public Guid UserId { get; set; }

        public bool IsTriggered { get; set; }
    }
}
