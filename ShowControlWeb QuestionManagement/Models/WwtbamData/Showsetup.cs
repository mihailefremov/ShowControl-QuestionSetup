using System;
using System.Collections.Generic;

namespace ShowControlWeb_QuestionManagement
{
    public partial class Showsetup
    {
        public long ShowId { get; set; }
        public string ShowName { get; set; }
        public DateTime DateOfShooting { get; set; }
        public DateTime DateOfBroadcasting { get; set; }
        public string Description { get; set; }
        public short? Shooted { get; set; }
        public short? Broadcasted { get; set; }
    }
}
