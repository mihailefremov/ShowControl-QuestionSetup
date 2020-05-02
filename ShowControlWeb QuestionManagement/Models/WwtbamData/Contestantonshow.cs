using System;
using System.Collections.Generic;

namespace ShowControlWeb_QuestionManagement
{
    public partial class Contestantonshow
    {
        public long ContestantId { get; set; }
        public long ShowId { get; set; }
        public int SeatPosition { get; set; }
        public short? Finished { get; set; }
        public double MoneyWon { get; set; }
    }
}
