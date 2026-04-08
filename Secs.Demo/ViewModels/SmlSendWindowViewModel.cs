using CommunityToolkit.Mvvm.ComponentModel;
using Secs.Demo.Commons;
using Secs.Demo.Models;
using Secs.Demo.Services;
using Secs.Demo.Views;
using Secs.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Secs.Demo.ViewModels
{
    public partial class SmlSendWindowViewModel(INotificationService notificationService, ILogService log) : ObservableObject
    {
        public Action? SaveHandler;
        public Action<byte, byte, HsmsBody>? SendHandler;
        public string FilePath { get; set; } = string.Empty;
        [ObservableProperty] private ObservableCollection<SmlItem> smlItems = new();
        [ObservableProperty] private SmlItem? selectedSmlItem;

        public void SendCommand()
        {
            try
            {
                if (SelectedSmlItem != null && SelectedSmlItem.Sml != null)
                {
                    var body = new HsmsBody(SelectedSmlItem.Sml);
                    SendHandler?.Invoke(SelectedSmlItem.Stream, SelectedSmlItem.Function, body);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                notificationService.ShowError(ex.Message, nameof(SmlSendWindow));
            }
        }
        public void SaveCommand()
        {
            try
            {
                var groups = SmlItems.GroupBy(c => c.Name);
                foreach (var group in groups)
                {
                    if (group.Count() > 1)
                        throw new Exception($"There are identical terms `{group.Key}`");
                }

                for (int i = SmlItems.Count - 1; i >= 0; i--)
                {
                    if (string.IsNullOrWhiteSpace(SmlItems[i].Name) || string.IsNullOrWhiteSpace(SmlItems[i].Sml))
                    {
                        SmlItems.RemoveAt(i);
                    }
                }

                SmlFileHelper.SaveToSml(FilePath, SmlItems.ToArray());
                SaveHandler?.Invoke();
                notificationService.ShowSuccess("Save success", nameof(SmlSendWindow));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                notificationService.ShowError(ex.Message, nameof(SmlSendWindow));
            }
        }
        public void AddCommand()
        {
            var item = new SmlItem();
            SmlItems.Add(item);
            SelectedSmlItem = item;
        }
        public void RemoveCommand(object arg)
        {
            if (arg is IList list && list.Count > 0)
            {
                var names = new List<string>();
                foreach (SmlItem item in list)
                {
                    names.Add(item.Name);
                }

                foreach (var name in names)
                {
                    var item = SmlItems.FirstOrDefault(c => c.Name == name);
                    if (item != null)
                        SmlItems.Remove(item);
                }
                notificationService.ShowSuccess("Remove success", nameof(SmlSendWindow));
            }
        }
    }
}
