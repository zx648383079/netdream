namespace NetDream.Shared.Interfaces.Forms
{
    public interface IFormInput
    {
        public string Name { get; }
        public object? Value { get; set; }

        public object Filter(object value);
    }
}
