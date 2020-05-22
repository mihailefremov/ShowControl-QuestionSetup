using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUpdaterGameQuestions
{
    class Program
    {


        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Clear();

            Console.WriteLine("Enter desired task to be executed: (mssqlimport/mssqlexport/mysqlimport)");
            var userOptions = Console.ReadLine().ToLower().Trim();

            if (userOptions == "mssqlimport")
            {
                new MsSqlDataWriterLayer().ImportAllQuestions();
                return;
            }
            else if (userOptions == "mssqlexport")
            {
                new MsSqlDataReaderLayer().ExportAllQuestions();
                return;
            }
            else if (userOptions == "mysqlimport")
            {
                new MySqlDataWriterLayer().ImportAllQuestions();
                return;
            }

            Console.ReadKey();

            #region "notneeded"

            //public int Priviligies = 0;
            //public String Level = "0";
            //public String Type = "0";
            //public String Difficulty = "0";
            //public static String QuestionID = "-1";
            //public String CategoryID = "0";
            //public String SubcategoryID = "0";
            //public String Stack = "-1";
            //public String CorrectAnswer = "0";
            //public String Used = "0";
            ////while (true)
            ////{
            ////    setq();
            ////    countq();
            ////    System.Threading.Thread.Sleep(10);
            ////    Console.WriteLine("\r\nEnter difficulty:");
            ////    string dif = Console.ReadLine();
            ////    short diff=-1;
            ////    if (short.TryParse(dif, out diff))
            ////    {
            ////        if (diff == 46)
            ////        {
            ////            UpdateQuestionInDB("401");
            ////        } else if (diff == 47)
            ////        {
            ////            UpdateQuestionInDB("400");
            ////        } else {
            ////            UpdateQuestionInDB(Convert.ToString(diff));
            ////        }
            ////    }
            ////    Console.WriteLine("\r\n");
            ////    System.Threading.Thread.Sleep(10);
            ////}

            //private static void setq()
            //{
            //    MySqlDataAdapter SDA = new MySqlDataAdapter();

            //    try
            //    {
            //        DataTable dbDataSet = new DataTable();
            //        MySqlConnection.ConnectionString = Properties.Settings.Default.DBConnectionString;


            //        MySqlConnection.Open();
            //        string Query;
            //        Query = Properties.Settings.Default.QueryForQSelection;
            //        MySqlCommand COMM;
            //        COMM = new MySqlCommand(Query, MySqlConnection);
            //        SDA.SelectCommand = COMM;
            //        SDA.Fill(dbDataSet);
            //        MySqlConnection.Close();
            //        FillQuestion(dbDataSet);

            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //    finally
            //    {
            //        SDA.Dispose();
            //        MySqlConnection.Dispose();
            //    }
            //}

            ////(SELECT COUNT(QuestionID) FROM `gamequestions_newlev` WHERE Difficulty=4)

            //private static void countq()
            //{
            //    MySqlDataAdapter SDA = new MySqlDataAdapter();

            //    try
            //    {
            //        DataTable dbDataSet = new DataTable();
            //        MySqlConnection.ConnectionString = Properties.Settings.Default.DBConnectionString;

            //        MySqlConnection.Open();
            //        string Query;
            //        Query = Properties.Settings.Default.QueryQuestionCount;
            //        MySqlCommand COMM;
            //        COMM = new MySqlCommand(Query, MySqlConnection);
            //        SDA.SelectCommand = COMM;
            //        SDA.Fill(dbDataSet);
            //        MySqlConnection.Close();
            //        Console.WriteLine("LEFT:" + Convert.ToString(dbDataSet.Rows[0]["CC"]));

            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //    finally
            //    {
            //        SDA.Dispose();
            //        MySqlConnection.Dispose();
            //    }
            //}

            //public static void UpdateQuestionInDB(string difficulty)
            //{
            //    MySqlConnection.ConnectionString = Properties.Settings.Default.DBConnectionString;
            //    try
            //    {
            //        MySqlConnection.Open();
            //        String gameqTable = Properties.Settings.Default.gameqTable;
            //        cmd = MySqlConnection.CreateCommand();
            //        cmd.CommandText = $"update {gameqTable} set Difficulty={difficulty} where QuestionID={QuestionID}";
            //        cmd.ExecuteNonQuery();
            //        MySqlConnection.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //    finally
            //    {
            //        MySqlConnection.Dispose();
            //    }
            //}

            //SELECT COUNT(QuestionID) AS CC FROM `gamequestions_newlev` WHERE Difficulty = 4

            //SELECT* FROM  `gamequestions_newlev` qs where qs.Difficulty=4 LIMIT 1

            //public static void FillQuestion(DataTable dbDataSet)
            //{
            //    if (dbDataSet.Rows.Count > 0)
            //    {
            //        Console.WriteLine(Convert.ToString(dbDataSet.Rows[0]["QuestionID"]));
            //        QuestionID = Convert.ToString(dbDataSet.Rows[0]["QuestionID"]);
            //        Console.WriteLine(Convert.ToString(dbDataSet.Rows[0]["Question"]));
            //        Console.WriteLine(Convert.ToString(dbDataSet.Rows[0]["Answer1"]));
            //        Console.WriteLine(Convert.ToString(dbDataSet.Rows[0]["Answer2"]));
            //        Console.WriteLine(Convert.ToString(dbDataSet.Rows[0]["Answer3"]));
            //        Console.WriteLine(Convert.ToString(dbDataSet.Rows[0]["Answer4"]));
            //        Console.WriteLine(Convert.ToString(dbDataSet.Rows[0]["CorrectAnswer"]));
            //        //Pronunciation_textBox.Text = Convert.ToString(dbDataSet.Rows[0]["Pronunciation"]);
            //        //ExplanationQ_textBox.Text = Convert.ToString(dbDataSet.Rows[0]["MoreInformation"]);
            //        //Used = Convert.ToString(dbDataSet.Rows[0]["TimesAnswered"]);
            //        //QuestionReferenceID_Textbox.Text = QuestionID;
            //        //Console.WriteLine(Convert.ToString(dbDataSet.Rows[0]["Difficulty"]));
            //        //Level_comboBox.Text = Level;

            //        //Category_comboBox.SelectedValue = CategoryID;

            //        //if (Type == "1")
            //        //{
            //        //    if (CorrectAnswer == "1") { Answer1correct_checkBox.CheckState = CheckState.Checked; }
            //        //    if (CorrectAnswer == "2") { Answer2correct_checkBox.CheckState = CheckState.Checked; }
            //        //    if (CorrectAnswer == "3") { Answer3correct_checkBox.CheckState = CheckState.Checked; }
            //        //    if (CorrectAnswer == "4") { Answer4correct_checkBox.CheckState = CheckState.Checked; }
            //        //    CorrectOrder_textBox.Enabled = false;
            //        //}
            //        //else if (Type == "2")
            //        //{
            //        //    CorrectOrder_textBox.Text = CorrectAnswer;
            //        //    CorrectOrder_textBox.Enabled = true;
            //        //}

            //        //if (Used == "0")
            //        //{
            //        //    QuestionUsage_checkBox.CheckState = CheckState.Unchecked;
            //        //}
            //        //else
            //        //{
            //        //    QuestionUsage_checkBox.CheckState = CheckState.Checked;
            //        //}


            //    }
            //}

            #endregion
        }
    }
}