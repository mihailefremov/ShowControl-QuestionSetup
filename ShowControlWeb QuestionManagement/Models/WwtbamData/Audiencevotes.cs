using System;
using System.Collections.Generic;

namespace ShowControlWeb_QuestionManagement
{
    public partial class Audiencevotes
    {
        public string Username { get; set; }
        public int? QuestionId { get; set; }
        public int GivenAnswer { get; set; }
        public DateTime TimeOfAnswering { get; set; }
    }
}
