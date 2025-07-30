using NetDream.Shared.Converters;
using System.Text.Json.Serialization;

namespace NetDream.Shared.Models
{
    [JsonConverter(typeof(LinkRuleConverter))]
    public class LinkExtraRule
    {
        public const string IMAGE_KEY = "i";
        public const string FILE_KEY = "f";
        public const string LINK_KEY = "l";
        public const string USER_KEY = "u";
        public const string WORD_KEY = "s";
        public string Word { get; set; } = string.Empty;

        public int User { get; set; }

        public string ExtraKey { get; set; } = string.Empty;

        public string ExtraValue { get; set; } = string.Empty;

        public bool TryGet(string key, out string value)
        {
            value = string.Empty;
            switch (key.ToLower())
            {
                case WORD_KEY:
                    value = Word;
                    return true;
                case USER_KEY:
                    if (User == 0)
                    {
                        return false;
                    }
                    value = User.ToString();
                    return true;
                default:
                    if (key == ExtraKey)
                    {
                        value = ExtraValue;
                        return true;
                    }
                    return false;
            }
        }

        public void Add(string key, string value)
        {
            switch (key.ToLower())
            {
                case WORD_KEY:
                    Word = value;
                    break;
                case USER_KEY:
                    User = int.Parse(value);
                    break;
                default:
                    ExtraKey = key;
                    ExtraValue = value;
                    break;
            }
        }

        public string this[string key] {
            get => TryGet(key, out var res) ? res : string.Empty;
            set => Add(key, value);
        }

        public LinkExtraRule()
        {
            
        }

        public LinkExtraRule(string search)
        {
            Word = search;
        }

        public LinkExtraRule(string search, int user): this(search)
        {
            User = user;
        }

        public static LinkExtraRule CreateImage(string search, string image)
        {
            return new LinkExtraRule(search)
            {
                ExtraKey = IMAGE_KEY,
                ExtraValue = image
            };
        }
        public static LinkExtraRule CreateUser(string search, int user)
        {
            return new LinkExtraRule(search, user);
        }
        public static LinkExtraRule CreateFile(string search, string file)
        {
            return new LinkExtraRule(search)
            {
                ExtraKey = FILE_KEY,
                ExtraValue = file
            };
        }
        public static LinkExtraRule CreateLink(string search, string url)
        {
            return new LinkExtraRule(search)
            {
                ExtraKey = LINK_KEY,
                ExtraValue = url
            };
        }
    }
}
