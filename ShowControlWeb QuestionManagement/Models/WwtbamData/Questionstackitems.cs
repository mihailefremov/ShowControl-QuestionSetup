using System;
using System.Collections.Generic;

namespace ShowControlWeb_QuestionManagement
{
    public partial class Questionstackitems
    {
        public long StackId { get; set; }
        public int StackLevel { get; set; }
        public long QuestionId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
        public int? AdditionalCategoryId { get; set; }
        public int? AdditionalSubcategoryId { get; set; }
    }

    public enum NumberOfQuestionsPerStackType
    {
        Standard = 15, Qualification = 10
    }
}
