using Avalonia.Controls;

namespace Secs.Demo.Services
{
    public interface INotificationService
    {
        void SetTopLeve(TopLevel topLevel, string token);
        void RemoveWindowNotificationManager(string token);
        void ShowError(string msg, string token);
        void ShowInfo(string msg, string token);
        void ShowSuccess(string msg, string token);
    }
}
