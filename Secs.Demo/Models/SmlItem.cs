using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Secs.Demo.Models
{
    public partial class SmlItem : ObservableObject
    {
        public SmlItem()
        {

        }
        public SmlItem(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return;

            var rows = content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            if (rows.Length > 1)
            {
                var cols = rows[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (cols.Length >= 2)
                {
                    Name = cols[0];
                    var sf = cols[1].Replace("S", "").Split("F", StringSplitOptions.RemoveEmptyEntries);
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

                    if (cols.Length == 3 && cols[2] == "W")
                    {
                        IsReply = true;
                    }
                    else
                    {
                        IsReply = false;
                    }
                    Sml = content.Substring(rows[0].Length + 2).Trim();
                }
            }
        }
        [ObservableProperty] private string sml = string.Empty;
        [ObservableProperty] private bool isReply = true;
        [ObservableProperty] private byte stream;
        [ObservableProperty] private byte function;
        [ObservableProperty] private string name = string.Empty;
    }
}
