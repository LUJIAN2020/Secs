using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using AvaloniaEdit;
using Secs.Demo.Services;
using Secs.Demo.ViewModels;

namespace Secs.Demo.Views;

public partial class SmlSendWindow : Window
{
    public SmlSendWindow()
    {
        InitializeComponent();
        DataContext = App.GetService<SmlSendWindowViewModel>();
        var _textEditor = this.FindControl<TextEditor>("Editor");
        if (_textEditor != null)
        {
            _textEditor.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
            _textEditor.Options.HighlightCurrentLine = true;
            _textEditor.Options.ShowSpaces = true;
        }

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