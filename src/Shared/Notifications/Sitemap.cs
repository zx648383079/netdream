using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NetDream.Shared.Notifications
{
    public class SitemapRequest : INotification
    {
        [XmlElement("urlset")]
        public IList<SitemapNode> Items { get; private set; } = [];

        public void Add(SitemapNode node)
        {
            Items.Add(node);
        }

        public void Add(IEnumerable<SitemapNode> nodes)
        {
            foreach (var item in nodes)
            {
                Add(item);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            using var writer = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true });
            writer.WriteStartDocument();
            writer.WriteStartElement(null, "urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            foreach (var item in Items)
            {
                writer.WriteStartElement(null, "url", null);
                writer.WriteElementString(null, "loc", null, item.Url);
                writer.WriteElementString(null, "lastmod", null, item.LastModificationDate.Value.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                writer.WriteElementString(null, "changefreq", null, Enum.GetName(item.ChangeFrequency.Value).ToLower());
                writer.WriteElementString(null, "priority", null, item.Priority.Value.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            return sb.ToString();
        }
    }

    public class SitemapNode(string url)
    {

        [XmlElement("loc", Order = 1), Url]
        public string Url { get; set; } = url;

        [XmlElement("lastmod", Order = 2)]
        public DateTime? LastModificationDate { get; set; }

        [XmlElement("changefreq", Order = 3)]
        public ChangeFrequency? ChangeFrequency { get; set; }

        [XmlElement("priority", Order = 4)]
        public decimal? Priority { get; set; }
    }

    public enum ChangeFrequency
    {
        [XmlEnum("always")]
        Always,

        [XmlEnum("hourly")]
        Hourly,

        [XmlEnum("daily")]
        Daily,

        [XmlEnum("weekly")]
        Weekly,

        [XmlEnum("monthly")]
        Monthly,

        [XmlEnum("yearly")]
        Yearly,

        [XmlEnum("never")]
        Never
    }
}
