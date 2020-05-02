using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowControlWeb_QuestionManagement
{
    public partial class Gamequestions
    {
        public int QuestionId { get; set; }
        public int Difficulty { get; set; }
        public int Type { get; set; }
        public string Question { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public short CorrectAnswer { get; set; }
        public int CategoryId { get; set; }
        public int SubcategoryId { get; set; }
        public int? AdditionalCategoryId { get; set; }
        public int? AdditionalSubcategoryId { get; set; }
        public string MoreInformation { get; set; }
        public string Pronunciation { get; set; }
        public string QuestionCreator { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string Comments { get; set; }
        public short TimesAnswered { get; set; }
        public DateTime? LastDateAnswered { get; set; }

    }

    public enum QuestionTypeDescription
    {
        Standard = 1, Qualification = 2
    }
}
