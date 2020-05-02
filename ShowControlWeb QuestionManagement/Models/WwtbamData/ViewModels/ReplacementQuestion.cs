using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowControlWeb_QuestionManagement.Models.WwtbamData.ViewModels
{
    public class ReplacementQuestion
    {
        public short Difficulty { get; set; }
        public long QuestionId { get; set; }
        public string Question { get; set; }
        public short TimesAnswered { get; set; }
    }

    public class StackQuestionReplacementViewModel
    {
        public IEnumerable<Questioncategories> questionCategories { get; set; }
        public IEnumerable<ReplacementQuestion> replacementQuestions { get; set; }
    }
}
