namespace NetDream.Shared.Interfaces
{
    public interface ICaptchaContext
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Level { get; set; }
    }
}
