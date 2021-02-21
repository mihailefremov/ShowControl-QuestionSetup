using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowControlWeb_QuestionManagement.Models.WwtbamData.ViewModels
{
    public class ReplacementQuestion
    {
        public long QuestionId { get; set; }
        public short Difficulty { get; set; }
        public short MappedLevel { get; set; }
        public short CategoryId { get; set; }
        public short SubcategoryId { get; set; }
        public string Question { get; set; }
        public DateTime? LastTimeAnswered { get; set; }
        public short TimesAnswered { get; set; }
    }

    public class StackQuestionReplacementViewModel
    {
        private IEnumerable<Questioncategories> _questionCategories { get; set; }
        public IEnumerable<Questioncategories> questionCategories
        {
            get
            {
                if (_questionCategories == null)
                {
                    List<Questioncategories> questioncategories1 = new List<Questioncategories>();
                    questioncategories1.Add(new Questioncategories { Category = "", CategoryId = -1 });
                    return questioncategories1.ToList();
                }
                return _questionCategories;
            }
            set
            {
                _questionCategories = value;
            }
        }

        private IEnumerable<Questionsubcategories>  _questionSubcategories { get; set; }
        public IEnumerable<Questionsubcategories> questionSubcategories
        {
            get
            {
                if (_questionSubcategories == null)
                {
                    List<Questionsubcategories> questionsubcategories1 = new List<Questionsubcategories>();
                    questionsubcategories1.Add(new Questionsubcategories { Subcategory = "", CategoryId = -1, SubcategoryId = -1 });
                    return questionsubcategories1.ToList();
                }
                return _questionSubcategories;
            }
        }

        private IEnumerable<ReplacementQuestion> _replacementQuestions;
        public IEnumerable<ReplacementQuestion> ?replacementQuestions
        {
            get
            {
                if (_replacementQuestions == null) return null;

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

        public string? Title { get; set; }
        public int StackId { get; set; }
        public int QuestionId { get; set; }
        public int Level { get; set; }
        public int? DownValue { get; set; }
        public int? UpValue { get; set; }

        private int _SelectedCategoryId = 0;
        public int SelectedCategoryId
        {
            get
            {
                return _SelectedCategoryId;
            }
            set
            {
                _SelectedCategoryId = value;
            }
        }
        private int _SelectedSubcategoryId = 0;
        public int SelectedSubcategoryId
        {
            get
            {
                return _SelectedSubcategoryId;
            }
            set
            {
                _SelectedSubcategoryId = value;
            }
        }
        public DateTime? LastTimeAnsweredDateTo { get; set; }
        public bool UseOldQuestions { get; set; }
    }
}      