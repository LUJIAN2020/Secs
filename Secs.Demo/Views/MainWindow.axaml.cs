using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Secs.Demo.Services;
using Secs.Demo.ViewModels;

namespace Secs.Demo.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.GetService<MainWindowViewModel>();
        }
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e); 
            var top = GetTopLevel(this);
            if (top != null)
            {
                var noti = App.GetService<INotificationService>();
                noti?.SetTopLeve(top, nameof(MainWindow));
            }
        }
    }
}