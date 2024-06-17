namespace NetDream.Core.Interfaces.Database
{
    public interface ITableColumn
    {
        public ITableColumn Unique();
        public ITableColumn Default(object val);

        public ITableColumn Comment(string comment);

        public ITableColumn Nullable();
        public ITableColumn Nullable(bool nullable);

        public ITableColumn Ai(int begin);
    }
}
