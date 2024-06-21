namespace NetDream.Shared.Interfaces.Database
{
    public interface IMigrationProvider
    {
        public void Migration(IMigration migration);
    }
}
