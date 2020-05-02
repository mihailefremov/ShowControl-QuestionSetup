using System;
using System.Collections.Generic;

namespace ShowControlWeb_QuestionManagement
{
    public partial class Moneytreevalues
    {
        public int MoneyTreeId { get; set; }
        public int Level { get; set; }
        public short SafeHeaven { get; set; }
        public int? Qaway { get; set; }
        public int? CorrectMoneyValue { get; set; }
        public int? CurrentWonMoneyValue { get; set; }
        public int? WrongMoneyValue { get; set; }
        public int? AtRiskMoneyValue { get; set; }
        public string Currency { get; set; }
    }
}
