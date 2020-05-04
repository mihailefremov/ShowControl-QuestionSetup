using System;
using System.Collections.Generic;

namespace ShowControlWeb_QuestionManagement
{
    public partial class Questionstacks
    {
        public int StackId { get; set; }
        public string Stack { get; set; }
        public int Type { get; set; }
        public DateTime Timestamp { get; set; }

        public QuestionTypeDescription StackType
        {
            get
            {
                switch (Type)
                {
                    case (int)QuestionTypeDescription.Standard:
                        return QuestionTypeDescription.Standard;

                    case (int)QuestionTypeDescription.Qualification:
                        return QuestionTypeDescription.Qualification;

                    default:
                        return QuestionTypeDescription.Undefined;
                }
            }
        }
        public short MaximumQuestionNumberPerStack
        {
            get
            {
                switch (StackType)
                {
                    case QuestionTypeDescription.Standard:
                        return 15;
                    case QuestionTypeDescription.Qualification:
                        return 10;
                    default:
                        return -1;
                }
            }
        }

    }

}
