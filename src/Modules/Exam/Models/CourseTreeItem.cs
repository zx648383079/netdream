using NetDream.Modules.Exam.Entities;
using NetDream.Shared.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Exam.Models
{
    public class CourseTreeItem : ITreeItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ParentId { get; set; }

        public IList<ITreeItem> Children { get; set; }

        public int QuestionCount { get; set; }
        public int Level { get; set; }

    }
}
