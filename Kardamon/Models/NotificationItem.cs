namespace Kardamon.Models;

public class NotificationItem
{
    /// <summary>
    /// Header notification
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// Text
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// The time (in seconds) after which the message disappears from the screen
    /// </summary>
    public double Delay { get; set; }
}