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

        public IEnumerable<ReplacementQuestion> _replacementQuestions;
        public IEnumerable<ReplacementQuestion> replacementQuestions
        {
            get
            {
                foreach (ReplacementQuestion p in _replacementQuestions)
                {
                    if (p.QuestionId <= 0) p.QuestionId = -1;
                    if (!string.IsNullOrWhiteSpace(p.Question))
                    {
                        p.Question = p.Question.Replace("|", " ").Replace("  ", " ");
                    }
                }
                return _replacementQuestions;
            }
            set
            {
                _replacementQuestions = value;
            }
        }
    
    }
}
