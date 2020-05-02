using System;
using System.Collections.Generic;

namespace ShowControlWeb_QuestionManagement
{
    public partial class Stateofgameplay
    {
        public string MessageType { get; set; }
        public string Question { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string FinalAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public string Pronunciation { get; set; }
        public string Explanation { get; set; }
        public string ProducerChat { get; set; }
        public string PlayerNameLocation { get; set; }
        public int? GameClock { get; set; }
        public int Lifeline1 { get; set; }
        public int Lifeline2 { get; set; }
        public int Lifeline3 { get; set; }
        public int Lifeline4 { get; set; }
        public int Audience1 { get; set; }
        public int Audience2 { get; set; }
        public int Audience3 { get; set; }
        public int Audience4 { get; set; }
        public int PhoneFriend { get; set; }
        public int? MoneyTree { get; set; }
        public string ShowControlMessage { get; set; }
        public int ReadBy { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
