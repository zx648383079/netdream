using NetDream.Modules.Blog.Repositories;
using NetDream.Shared.Models;
using NetDream.Shared.Providers.Models;

namespace NetDream.Modules.Blog.Models
{
    public class EditOption
    {

        public OptionItem<string>[] Localizes { get; set; }
        public string[] Languages { get; set; } = [
            "Html", "Css", "Sass", "Less", "TypeScript", "JavaScript", "PHP", "Go", "C#", "ASP.NET", ".NET Core", "Python", "C", "C++", "Java", "Kotlin", "Swift", "Objective-C", "Dart", "Flutter"
        ];
        public OptionItem<string>[] Weathers { get; set; }
        public LinkOptionItem[] Licenses { get; set; }
        public TagListItem[] Tags { get; set; }
        public CategoryLabelItem[] Categories { get; set; }
        public OptionItem<int>[] OpenTypes { get; set; } = [
            new("Is Public", PublishRepository.OPEN_PUBLIC),
            new("Need Login", PublishRepository.OPEN_LOGIN),
            new("Need Password", PublishRepository.OPEN_PASSWORD),
            new("Need Buy", PublishRepository.OPEN_BUY)
        ];
        public OptionItem<int>[] PublishStatus { get; set; } = [
            new("As a draft", PublishRepository.PUBLISH_STATUS_DRAFT),
            new("As a publish", PublishRepository.PUBLISH_STATUS_POSTED),
            new("As a trash", PublishRepository.PUBLISH_STATUS_TRASH)
        ];
    }
}
