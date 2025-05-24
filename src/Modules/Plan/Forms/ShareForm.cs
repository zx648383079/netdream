namespace NetDream.Modules.Plan.Forms
{
    public class ShareForm
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public byte ShareType { get; set; }
        public string ShareRule { get; set; } = string.Empty;
    }
}
