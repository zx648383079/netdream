namespace NetDream.Modules.Gzo.Writers
{
    public interface IWriter
    {
        public string Mkdir(string folder);

        public void Write(string file, string content);
    }
}
