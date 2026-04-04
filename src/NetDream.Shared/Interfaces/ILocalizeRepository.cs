namespace NetDream.Shared.Interfaces
{
    public interface ILocalizeRepository
    {
        /// <summary>
        /// 默认语言
        /// </summary>
        public string Default { get; }
        /// <summary>
        /// 当前语言
        /// </summary>
        public string Language { get; }
        /// <summary>
        /// 所有语言
        /// </summary>
        public string[] Keys { get; }
        /// <summary>
        /// 所有语言包括名称
        /// </summary>
        public IOptionItem<string>[] Items { get; }
    }
}
