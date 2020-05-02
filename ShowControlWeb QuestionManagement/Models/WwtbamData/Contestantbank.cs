using System;
using System.Collections.Generic;

namespace ShowControlWeb_QuestionManagement
{
    public partial class Contestantbank
    {
        public long ContestantId { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Location { get; set; }
        public DateTime Birthday { get; set; }
        public string Education { get; set; }
        public string Preferences { get; set; }
        public string Biography { get; set; }
        public string PersonalInfo { get; set; }
    }
}
