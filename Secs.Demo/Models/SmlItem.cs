using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Secs.Demo.Models
{
    public partial class SmlItem : ObservableObject
    {
        public SmlItem() { }
        public SmlItem(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return;

            var rows = content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            if (rows.Length > 1)
            {
                var cols = rows[0].Split(":", StringSplitOptions.RemoveEmptyEntries);
                if (cols.Length >= 1)
                {
                    Name = cols[0];
                    if (cols.Length > 1)
                    {
                        var clos1 = cols[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        if (clos1.Length >= 1)
                        {
                            var sf = clos1[0].Replace("'", "").Replace("S", "").Split("F", StringSplitOptions.RemoveEmptyEntries);
                            if (sf.Length == 2)
                            {
                                if (byte.TryParse(sf[0], out var s))
                                {
                                    Stream = s;
                                }
                                if (byte.TryParse(sf[1], out var f))
                                {
                                    Function = f;
                                }
                            }
                            if (clos1.Length > 1)
                            {
                                if (clos1[1] == "W")
                                {
                                    IsReply = true;
                                }
                                else
                                {
                                    IsReply = false;
                                }
                            }
                        }
                    }
                }
                string sml = content.Substring(rows[0].Length + 2).Trim();
                Sml = sml.Remove(sml.Length - 1, 1);
            }
        }
        [ObservableProperty] private string sml = string.Empty;
        [ObservableProperty] private bool isReply = true;
        [ObservableProperty] private byte stream;
        [ObservableProperty] private byte function;
        [ObservableProperty] private string name = string.Empty;
    }
}
