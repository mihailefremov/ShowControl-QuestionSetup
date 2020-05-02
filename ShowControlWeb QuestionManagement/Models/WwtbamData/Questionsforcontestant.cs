using System;
using System.Collections.Generic;

namespace ShowControlWeb_QuestionManagement
{
    public partial class Questionsforcontestant
    {
        public int Level { get; set; }
        public long QuestionId { get; set; }
        public int Type { get; set; }
        public string Question { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string CorrectAnswer { get; set; }
        public string MoreInformation { get; set; }
        public string Pronunciation { get; set; }
        public short Answered { get; set; }
        public string QuestionCreator { get; set; }
        public string Comments { get; set; }
    }
}
