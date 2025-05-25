using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Exam.Forms
{
    public class UpgradeForm
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int CourseGrade { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
