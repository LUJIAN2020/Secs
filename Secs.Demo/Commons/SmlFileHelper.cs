using Secs.Demo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Secs.Demo.Commons
{
    public class SmlFileHelper
    {
        public static SmlItem[] LoadToSmlItems(string path)
        {
            var smlItems = new List<SmlItem>();
            var content = File.ReadAllText(path, Encoding.UTF8);
            var lines = File.ReadAllLines(path, Encoding.UTF8);
            int offset = 0;
            int totalLength = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                totalLength += lines[i].Length;
                if (lines[i] == ".")
                {
                    string sml = content.Substring(offset, totalLength);
                    smlItems.Add(new SmlItem(sml));
                }
            }
            return smlItems.ToArray();
        }
        public static SmlItem[] LoadToSmlItemsFromText(string text)
        {
            var lines = text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var smlItems = new List<SmlItem>();
            var sb = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                sb.AppendLine(lines[i]);
                if (lines[i] == ".")
                {
                    string sml = sb.ToString();
                    smlItems.Add(new SmlItem(sml));
                    sb.Clear();
                }
            }
            return smlItems.ToArray();
        }
        public static void SaveToSml(string path, SmlItem[] items)
        {
            var sb = new StringBuilder();
            foreach (var item in items)
            {
                sb.Append($"{item.Name} S{item.Stream}F{item.Function}");
                if (item.IsReply)
                {
                    sb.Append(" W\r\n");
                }
                else
                {
                    sb.Append("\r\n");
                }
                sb.Append(item.Sml).Append("\r\n").Append(".");
            }
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }
    }
}
