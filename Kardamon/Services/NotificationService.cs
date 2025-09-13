using Kardamon.Models;

namespace Kardamon.Services;

public class NotificationService
{
    /// <summary>
    /// The event occurs when sending a notification
    /// </summary>
    public event Action<NotificationItem>? Received;

    /// <summary>
    /// Raises Received event
    /// </summary>
    /// <param name="title">Notification title</param>
    /// <param name="message">Notification text</param>
    /// <param name="delay">The time (in seconds) after which the message disappears from the screen</param>
    public void Send(string title, string message, double delay)
    {
        Received?.Invoke(new NotificationItem { Title = title, Message = message, Delay = delay });
    }
}