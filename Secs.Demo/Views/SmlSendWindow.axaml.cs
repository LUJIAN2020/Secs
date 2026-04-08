using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Secs.Demo.Services;
using Secs.Demo.ViewModels;

namespace Secs.Demo.Views;

public partial class SmlSendWindow : Window
{
    public SmlSendWindow()
    {
        InitializeComponent();
        DataContext = App.GetService<SmlSendWindowViewModel>();
        Closed += SmlSendWindow_Closed;
    }
    private void SmlSendWindow_Closed(object? sender, System.EventArgs e)
    {
        var noti = App.GetService<INotificationService>();
        noti?.RemoveWindowNotificationManager(nameof(SmlSendWindow));
    }
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var top = GetTopLevel(this);
        if (top != null)
        {
            var noti = App.GetService<INotificationService>();
            noti?.SetTopLeve(top, nameof(SmlSendWindow));
        }
    }
}