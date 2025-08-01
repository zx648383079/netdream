namespace NetDream.Modules.OpenPlatform
{
    public class FailureResponse
    {
        public int Code { get; set; } = 404;

        public string Message { get; set; } = string.Empty;

        public FailureResponse()
        {
            
        }

        public FailureResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public FailureResponse(string message)
        {
            Message = message;
        }
    }
}
