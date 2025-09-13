using System.Timers;
using Kardamon.Models;
using Kardamon.Services;

namespace Kardamon.ViewModels;

public partial class ToastNotificationViewModel : ViewModelBase
{
    private readonly NotificationService  _notificationService;
    private Timer? _timer;
    [ObservableProperty] NotificationItem? _notificationItem;

    public ToastNotificationViewModel(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    public void Init()
    {
        _timer = new Timer();
        
        _notificationService.Received += NotificationServiceOnReceived;
    }

    private void NotificationServiceOnReceived(NotificationItem obj)
    {
        _timer.Interval = TimeSpan.FromSeconds(obj.Delay).TotalMilliseconds;
        NotificationItem = obj;
        _timer.Elapsed += TimerOnElapsed;
        _timer.Start();
    }


    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        NotificationItem = null;
        _timer.Elapsed -= TimerOnElapsed;
        _timer.Stop();
    }
}