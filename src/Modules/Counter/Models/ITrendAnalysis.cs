using System.Collections.Generic;

namespace NetDream.Modules.Counter.Models
{
    public interface ITrendAnalysis
    {
        public int IpCount { get; set; }
        public int Pv { get; set; }
        public int Uv { get; set; }
        public string Scale { get; set; }
    }

    internal class AnalysisGroup
    {
        public int Count { get; set; }
        public HashSet<string> Ip { get; private set; } = [];
        public HashSet<string> Session { get; private set; } = [];
    }

    public class UrlTrendAnalysis(string url) : ITrendAnalysis
    {
        public string Url => url;
        public int IpCount { get; set; }
        public int Pv { get; set; }
        public int Uv { get; set; }
        public string Scale { get; set; } = "100%";
    }

    public class HostTrendAnalysis(string host) : ITrendAnalysis
    {
        public string Host => host;
        public int IpCount { get; set; }
        public int Pv { get; set; }
        public int Uv { get; set; }
        public string Scale { get; set; } = "100%";
    }

    public class SearchTrendAnalysis(string word) : ITrendAnalysis
    {
        public string Words => word;
        public int IpCount { get; set; }
        public int Pv { get; set; }
        public int Uv { get; set; }
        public string Scale { get; set; } = "100%";
    }
}
