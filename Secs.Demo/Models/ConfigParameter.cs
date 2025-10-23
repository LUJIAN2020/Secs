using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection.PortableExecutable;
using System.Text.Json.Serialization;

namespace Secs.Demo.Models
{
    public partial class ConfigParameter : ObservableObject
    {
        private bool passiveIsChecked;
        public bool PassiveIsChecked
        {
            get { return passiveIsChecked; }
            set { SetProperty(ref passiveIsChecked, value); }
        }

        private bool _activeIsChecked = true;
        public bool ActiveIsChecked
        {
            get { return _activeIsChecked; }
            set { SetProperty(ref _activeIsChecked, value); }
        }

        private string _remoteIP = "127.0.0.1";
        public string RemoteIP
        {
            get { return _remoteIP; }
            set { SetProperty(ref _remoteIP, value); }
        }

        private ushort _remotePort = 5000;
        public ushort RemotePort
        {
            get { return _remotePort; }
            set { SetProperty(ref _remotePort, value); }
        }

        private string _localIP = "127.0.0.1";
        public string LocalIP
        {
            get { return _localIP; }
            set { SetProperty(ref _localIP, value); }
        }

        private ushort _localPort = 5000;
        public ushort LocalPort
        {
            get { return _localPort; }
            set { SetProperty(ref _localPort, value); }
        }

        private ushort _device = 0;
        public ushort Device
        {
            get { return _device; }
            set { SetProperty(ref _device, value); }
        }

        private ushort t3 = 45;
        public ushort T3
        {
            get { return t3; }
            set { SetProperty(ref t3, value); }
        }

        private ushort t5 = 10;
        public ushort T5
        {
            get { return t5; }
            set { SetProperty(ref t5, value); }
        }

        private ushort t6 = 5;
        public ushort T6
        {
            get { return t6; }
            set { SetProperty(ref t6, value); }
        }

        private ushort t7 = 10;
        public ushort T7
        {
            get { return t7; }
            set { SetProperty(ref t7, value); }
        }

        private ushort t8 = 5;
        public ushort T8
        {
            get { return t8; }
            set { SetProperty(ref t8, value); }
        }
    }

    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(ConfigParameter))]
    internal partial class MyGenerationContext : JsonSerializerContext
    {
    }
}
