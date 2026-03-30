namespace NetDream.Shared.Models
{
    public class AgreeResult
    {
        public bool IsAgree => AgreeType == AgreeType.Agree;
        public bool IsDisagree => AgreeType == AgreeType.Disagree;

        public AgreeType AgreeType { get; set; }

        public int AgreeCount { get; set; }
        public int DisagreeCount { get; set; }

        public AgreeResult()
        {
            
        }

        public AgreeResult(bool isAgree, int agreeCount, int disagreeCount)
        {
            AgreeType = isAgree ? AgreeType.Agree : AgreeType.Disagree;
            AgreeCount = agreeCount;
            DisagreeCount = disagreeCount;
        }
    }

    public enum AgreeType: byte
    {
        None,
        Agree,
        Disagree,
    }
}
