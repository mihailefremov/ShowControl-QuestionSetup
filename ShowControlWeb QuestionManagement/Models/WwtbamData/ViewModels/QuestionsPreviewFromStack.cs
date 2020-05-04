using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShowControlWeb_QuestionManagement.Models.WwtbamData.ViewModels
{
    public class QuestionsPreviewFromStack
    { 
        public short QuestionLevel { get; set; }        
        public int  StackId { get; set; }
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public short TimesAnswered { get; set; }
    }

    public class StackQuestionPreviewViewModel
    {
        public Questionstacks stack { get; set; }

        private IEnumerable<QuestionsPreviewFromStack> _questionsPreviewFromStack;
        public IEnumerable<QuestionsPreviewFromStack> questionsPreviewFromStack
        {
            get
            {
                foreach (QuestionsPreviewFromStack p in _questionsPreviewFromStack)
                {
                    if (p.QuestionId <= 0) p.QuestionId = -1;
                    if (!string.IsNullOrWhiteSpace(p.Question))
                    {
                        p.Question = p.Question.Replace("|", " ").Replace("  ", " ");
                    }
                }
                return _questionsPreviewFromStack;
            }
            set
            {
                _questionsPreviewFromStack = value;
            }
        }

        public bool IsStackLive { get; set; }
        public bool IsStackReplacement { get; set; }
    }
}
