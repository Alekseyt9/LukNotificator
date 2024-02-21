

namespace LukNotificator.Services
{
    internal interface INotificationTaskManager
    {
        /// <summary>
        /// Начальная загрузка, запуск задач
        /// </summary>
        Task Start();
    }
}
