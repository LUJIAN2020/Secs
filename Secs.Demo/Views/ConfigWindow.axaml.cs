using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Messaging;
using Secs.Demo.Services;
using Secs.Demo.ViewModels;

namespace Secs.Demo.Views;

public partial class ConfigWindow : Window
{
    public ConfigWindow()
    {
        InitializeComponent();
        DataContext = App.GetService<ConfigWindowViewModel>();

        WeakReferenceMessenger.Default.Register<string, string>(this, "CloseWindow", (o, m) =>
        {
            this.Close();
        });
    }
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var top = GetTopLevel(this);
        if (top != null)
        {
            var noti = App.GetService<INotificationService>();
            noti?.SetTopLeve(top, nameof(ConfigWindow));
        }
    }
}