using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Secs.Demo.Commons;
using Secs.Demo.Models;

namespace Secs.Demo.ViewModels
{
    public partial class ConfigWindowViewModel : ObservableObject
    {
        [ObservableProperty] private ConfigParameter _config = new ConfigParameter();
        public ConfigWindowViewModel()
        {
            Config = ConfigHelper.LoadConfig();
        }
        public void OkCommand()
        {
            ConfigHelper.SaveConfig(Config);
            CloseConfigWindow();
        }
        public void CancelCommand()
        {
            CloseConfigWindow();
        }
        private void CloseConfigWindow()
        {
            WeakReferenceMessenger.Default.Send(string.Empty, "CloseWindow");
        }
    }
}
