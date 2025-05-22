namespace NetDream.Modules.Trade.Listeners
{
    public class PayResult
    {
        public byte SourceType { get; set; }
        public int SourceId { get; set; }

        public PayOperateStatus Status { get; set; }

        public PayOperateData? OperateData { get; set; }

        public string? FailureMessage { get; set; }
    }

    public enum PayOperateStatus
    {
        None,
        /// <summary>
        /// 需要前端继续操作
        /// </summary>
        Operate,
        Success,
        Failure
    }

    public class PayOperateData
    {
        public string? Html { get; set; }
        public string? Url { get; set; }
        public string? Form { get; set; }
        public object? Params { get; set; }
    }
}
