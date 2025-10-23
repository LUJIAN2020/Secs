using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using System.Collections.Generic;

namespace Secs.Demo.Services
{
    public class NotificationService : INotificationService
    {
        private readonly Dictionary<string, WindowNotificationManager> managers = new();
        public void SetTopLeve(TopLevel topLevel, string token)
        {
            if (!managers.TryGetValue(token, out var maneger))
            {
                var manager = new WindowNotificationManager(topLevel)
                {
                    Position = NotificationPosition.BottomRight,
                    MaxItems = 3
                };
                managers.Add(token, manager);
            }
        }
        public void RemoveWindowNotificationManager(string token)
        {
            managers.Remove(token);
        }
        private WindowNotificationManager? GetWindowNotificationManager(string token)
        {
            if (managers.TryGetValue(token, out var maneger))
            {
                return maneger;
            }
            return default;
        }
        public void ShowError(string msg, string token)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                var manager = GetWindowNotificationManager(token);
                manager?.Show(new Notification("ERROR", msg, NotificationType.Error));
            }, DispatcherPriority.Background);
        }
        public void ShowInfo(string msg, string token)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                var manager = GetWindowNotificationManager(token);
                manager?.Show(new Notification("INFO", msg, NotificationType.Information));
            });
        }
        public void ShowSuccess(string msg, string token)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                var manager = GetWindowNotificationManager(token);
                manager?.Show(new Notification("Success", msg, NotificationType.Success));
            });
        }
    }
}
