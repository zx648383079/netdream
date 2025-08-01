namespace NetDream.Modules.Career.Models
{
    public class ProfileFormatted
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }

        public LinkFormatted[] Links { get; set; }

        public SkillFormatted[] Skills { get; set; }
    }

    public class LinkFormatted
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
    }


    public class SkillFormatted
    {
        public string Name { get; set; }
        public byte Proficiency { get; set; }
        public string Duration { get; set; }

        public string FormattedProficiency => Proficiency switch
        {
            >= 90 => "专家",
            >= 70 => "精通",
            >= 60 => "高级",
            >= 40 => "中级",
            >= 20 => "初级",
            _ => "入门"
        };
        public LinkFormatted[] Links { get; set; }

    }
}
