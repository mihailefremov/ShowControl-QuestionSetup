using System;
using System.Collections.Generic;

namespace ShowControlWeb_QuestionManagement
{
    public partial class Stackconfigurations
    {
        public int StackConfigurationId { get; set; }
        public byte? UseNewQuestionsForStackBuildUp { get; set; }
        public byte? UseOldQuestionsForStackBuildUp { get; set; }
        public byte? UseOldQuestionsForReplacementSearch { get; set; }
        public int? QualificationQuestionsPerStack { get; set; }
        public int? StandardQuestionsPerStack { get; set; }
    }
}
