using NetDream.Shared.Interfaces;

namespace NetDream.Shared.Securities
{
    public class NoneEncoder : ISecurity
    {
        public string Decrypt(string data)
        {
            return data;
        }

        public string Encrypt(string data)
        {
            return data;
        }
    }
}
