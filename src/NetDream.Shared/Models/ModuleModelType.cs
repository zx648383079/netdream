namespace NetDream.Shared.Repositories
{
    public enum ModuleTargetType: byte
    {
        User = 1,
        UserUpdate = 5,
        /// <summary>
        /// 给账户充值
        /// </summary>
        UserRecharge = 6,
        Blog = 10,
        BlogComment = 11,

        Book = 14,

        ExamQuestion = 20,
        ExamPage = 21,
        CMSContent = 23,

        Demo = 24,
        ForumThread = 27,
        ForumPost = 28,

        SearchSite = 30,
        SearchPage = 31,
        MicroBlog = 33,
        MicroComment = 34,
        ShopGood = 35,
        ShopAd = 36,

        Video = 40,
        VideoMusic = 41,

        BotAccount = 45,
        BotMedia = 46,

        AppStore = 50,
        ResourceStore = 55,

        Legwork = 69,
        System = 80,
        Role = 83,
        RolePermission = 84,

        SystemFriendLink = 84,
        SystemFeedback = 85,
    }
}
