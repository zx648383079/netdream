namespace NetDream.Shared.Interfaces
{
    public interface ICaptcha
    {
        /// <summary>
        /// 是否就是当前图片，能直接输出
        /// </summary>
        public bool IsOnlyImage { get; }
        /// <summary>
        /// 生成并返回结果
        /// </summary>
        /// <returns></returns>
        public ICaptchaOutput Generate();
        /// <summary>
        /// 使用生成的结果和用户提交的结果进行验证
        /// </summary>
        /// <param name="value"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool Verify(object value, object source);
    }
}
