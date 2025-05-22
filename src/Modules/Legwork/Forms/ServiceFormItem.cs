namespace NetDream.Modules.Legwork.Forms
{
    public class ServiceFormItem
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public byte Required { get; set; }
        public byte Only { get; set; }
    }

    public class ServiceFormValue
    {
        public string Name { get; set; }
        public string Label { get; set; }

        public string Value { get; set; }
        public byte Only { get; set; }
    }
}
