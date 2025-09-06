namespace NetDream.Modules.Counter.Models
{
    public interface IWithClientModel
    {
        public int LogId { get; }

        public ClientLabelItem? Client { set; }

    }
}
