namespace NetDream.Shared.Interfaces
{
    public interface ISecurity
    {
        public string Encrypt(string data);

        public string Decrypt(string data);
    }
}
