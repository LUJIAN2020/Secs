using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Secs.Demo.Services;
using Secs.Demo.ViewModels;
using Secs.Demo.Views;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Secs.Demo
{
    public partial class App : Application
    {
        [NotNull] public static IHost? GlobalHost { get; private set; }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public static Window MainWindow => GetMainWindow();
        public override void OnFrameworkInitializationCompleted()
        {
            HostBuild();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = GetService<MainWindow>();
                if (mainWindow != null)
                {
                    mainWindow.Closing += async (_, _) =>
                    {
                        await Task.Delay(1000);
                        _ = GlobalHost.StopAsync();
                    };
                    mainWindow.DataContext = GetService<MainWindowViewModel>();
                    desktop.MainWindow = mainWindow;
                }
            }
            _ = GlobalHost.StartAsync();
            base.OnFrameworkInitializationCompleted();
        }
        private static void Register(IServiceCollection services)
        {
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<INotificationService, NotificationService>();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();

            services.AddTransient<ConfigWindow>();
            services.AddTransient<ConfigWindowViewModel>();

            services.AddTransient<SmlSendWindow>();
            services.AddTransient<SmlSendWindowViewModel>();
        }
        private static void HostBuild()
        {
            var builder = Host.CreateApplicationBuilder();
            Register(builder.Services);
            GlobalHost = builder.Build();
        }
        public static T? GetService<T>()
        {
            return GlobalHost.Services.GetService<T>();
        }
        public static MainWindow GetMainWindow()
        {
            return GetService<MainWindow>() ?? throw new NullReferenceException("MianWindow is null");
        }
    }
}