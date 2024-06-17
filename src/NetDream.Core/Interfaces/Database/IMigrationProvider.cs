namespace NetDream.Core.Interfaces.Database
{
    public interface IMigrationProvider
    {
        public void Migration(IMigration migration);
    }
}
