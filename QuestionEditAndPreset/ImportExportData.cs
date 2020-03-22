using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QuestionEditAndPreset
{
    class ImportExportData
    {
        public static string qTxtPath = "";

        public static void ParseQFile()
        {
            string Qst = "", Ans1 = "", Ans2 = "", Ans3 = "", Ans4 = "";
            int Cor = 0;
            string Exp = "", Com = "";
            int Cat1 = 0, Subcat1 = 0, Cat2 = 0, Subcat2 = 0;

            string wholefile = File.ReadAllText(qTxtPath);
            wholefile = wholefile.Trim();
            wholefile += "\r\n\r\n";
            File.Delete(qTxtPath);
            File.AppendAllText(qTxtPath, wholefile);

            string[] readText = File.ReadAllLines(qTxtPath);
            string result = "";

            foreach (string myString in readText)
            {
                if (myString.ToUpper().Contains("QST:") || myString.ToUpper().Contains("QUESTION:"))
                {
                    Qst = Regex.Replace(myString, "QST:", "", RegexOptions.IgnoreCase).Trim();
                    if (Qst.Contains("|"))
                    {
                        Qst = Qst.Replace("|", "\n");
                    }
                    //continue;
                }
                if (myString.ToUpper().Contains("ANS1:") || myString.ToUpper().Contains("ANSWER1:"))
                {
                    Ans1 = Regex.Replace(myString, "ANS1:", "", RegexOptions.IgnoreCase).Trim();
                    //continue;
                }
                if (myString.ToUpper().Contains("ANS2:") || myString.ToUpper().Contains("ANSWER2:"))
                {
                    Ans2 = Regex.Replace(myString, "ANS2:", "", RegexOptions.IgnoreCase).Trim();
                    //continue;
                }
                if (myString.ToUpper().Contains("ANS3:") || myString.ToUpper().Contains("ANSWER3:"))
                {
                    Ans3 = Regex.Replace(myString, "ANS3:", "", RegexOptions.IgnoreCase).Trim();
                    //continue;
                }
                if (myString.ToUpper().Contains("ANS4:") || myString.ToUpper().Contains("ANSWER4:"))
                {
                    Ans4 = Regex.Replace(myString, "ANS4:", "", RegexOptions.IgnoreCase).Trim();
                    //continue;
                }
                if (myString.ToUpper().Contains("COR:") || myString.ToUpper().Contains("CORRECT:"))
                {
                    result = Regex.Replace(myString, "COR:", "", RegexOptions.IgnoreCase).Trim();
                    Int32.TryParse(result, out Cor);
                    //continue;
                }
                if (myString.ToUpper().Contains("EXP:") || myString.ToUpper().Contains("EXPLANATION:"))
                {
                    Exp = Regex.Replace(myString, "EXP:", "", RegexOptions.IgnoreCase).Trim();
                    //continue;
                }
                if (myString.ToUpper().Contains("CAT:") || myString.ToUpper().Contains("CATEGORY:"))
                {
                    result = Regex.Replace(myString, "CAT:", "", RegexOptions.IgnoreCase).Trim();
                    Int32.TryParse(result, out Cat1);
                    //continue;
                }
                if (myString.ToUpper().Contains("SUBCAT:") || myString.ToUpper().Contains("SUBCATEGORY:"))
                {
                    result = Regex.Replace(myString, "SUBCAT:", "", RegexOptions.IgnoreCase).Trim();
                    Int32.TryParse(result, out Subcat1);
                    //continue;
                }
                if (myString.ToUpper().Contains("COM:") || myString.ToUpper().Contains("COMMENT:"))
                {
                    Com = Regex.Replace(myString, "COM:", "", RegexOptions.IgnoreCase).Trim();
                    //continue;
                }
                if (myString.Trim() == "")
                {
                    if (CheckQIfValid(Qst, Ans1, Ans2, Ans3, Ans4, Cor))
                    {
                        InsertQintoDB(Qst, Ans1, Ans2, Ans3, Ans4, Cor, Exp, Cat1, Subcat1, Com);
                        Qst = Ans1 = Ans2 = Ans3 = Ans4 = "";
                        Exp = Com = "";
                        Cor = 0;
                        Cat1 = Cat2 = 0;
                        Subcat1 = Subcat2 = 0;
                    }
                }
            }
        }

        private static void InsertQintoDB(string qst, string ans1, string ans2, string ans3, string ans4, int cor, string exp, int cat1, int subcat1, string com)
        {
            QuestionStackManipulation qstInsertDB = new QuestionStackManipulation();
            //   
            CheckIfDuplicateQ(qst, ans1, ans2, ans3, ans4);
            //throw new NotImplementedException();
        }

        private static bool CheckIfDuplicateQ(string qst, string ans1, string ans2, string ans3, string ans4)
        {
            QuestionStackManipulation qstCheckDuplicatetDB = new QuestionStackManipulation();
            return false;
            //throw new NotImplementedException();
        }

        public static bool CheckQIfValid(String Qst, String Ans1, String Ans2, String Ans3, String Ans4, int Cor)
        {
            if (Qst.Trim() == "" || Ans1.Trim() == "" || Ans2.Trim() == "" || Ans3.Trim() == "" || Ans4.Trim() == "" || Cor < 1 || Cor > 4)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void SetFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                qTxtPath = openFileDialog1.FileName;
                ParseQFile();
            }
        }
    }
}

