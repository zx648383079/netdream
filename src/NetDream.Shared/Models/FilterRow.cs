using NetDream.Shared.Interfaces;

namespace NetDream.Shared.Models
{
    public class FilterGroup(string name, string label) : IFilterGroup
    {
        public string Name => name;

        public string Label => label;

        public bool Multiple {  get; set; }

        public IFilterOption[] Items { get; set; } = [];
    }

    public class RangeFilterGroup(string name, string label) : FilterGroup(name, label)
    {
        public float Min { get; set; }
        public float Max { get; set; }
    }

    public class FilterOptionItem(string label, string value) : IFilterOption
    {
        public string Label => label;

        public string Value => value;

        public bool Selected { get; set; }

        public int Count { get; set; }
    }
}
