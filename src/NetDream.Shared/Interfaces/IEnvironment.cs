namespace NetDream.Shared.Interfaces
{
    public interface IEnvironment
    {
        public string Root { get; }

        public string PublicRoot { get; }
        public string AssetRoot { get; }
        public string OnlineDiskRoot { get; }

    }
}
