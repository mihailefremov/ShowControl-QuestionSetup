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
        public List<QuestionsPreviewFromStack> questionsPreviewFromStack { get; set; }
    }
}
