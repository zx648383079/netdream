namespace NetDream.Shared.Models
{
    public class AgreeResult
    {
        public bool IsAgree => AgreeType == 1;
        public bool IsDisagree => AgreeType == 2;

        public byte AgreeType { get; set; }

        public int AgreeCount { get; set; }
        public int DisagreeCount { get; set; }

        public AgreeResult()
        {
            
        }

        public AgreeResult(bool isAgree, int agreeCount, int disagreeCount)
        {
            AgreeType = (byte)(isAgree ? 1 : 2);
            AgreeCount = agreeCount;
            DisagreeCount = disagreeCount;
        }
    }
}
