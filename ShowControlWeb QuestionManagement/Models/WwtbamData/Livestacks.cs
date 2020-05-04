using System;
using System.Collections.Generic;

namespace ShowControlWeb_QuestionManagement
{
    public partial class Livestacks
    {
        public short StackType { get; set; }
        public int StackId { get; set; }
        public byte IsReplacement { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
