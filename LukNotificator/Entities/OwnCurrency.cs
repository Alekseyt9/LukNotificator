namespace LukNotificator.Entities
{
    public class OwnCurrency
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public Guid UserId { get; set; }

        public OwnCurProps Props { get; set; }
    }
}
