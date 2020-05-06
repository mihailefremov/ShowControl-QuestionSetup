using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUpdaterGameQuestions
{
    class MsSqlDataLayer
    {
        private static SqlConnection SqlConnection = new SqlConnection();
        private static SqlCommand cmd = new SqlCommand();

        private static SqlTransaction myTrans; // Start a local transaction

       public static void ExecuteMsSql()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.Clear();

            string[] rowsofquestion = { "" };
            try
            {
                rowsofquestion = File.ReadAllLines(string.Format("{0}/{1}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Properties.Settings.Default.FileNameWithExtensionForQuestionImport));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " " + e.InnerException);
                Console.ReadKey();
                return;
            }

            //SELECT REPLACE(gamequestions2.Question,'\r\n','|') from gamequestions2 where Question REGEXP "\r\n";
            //SELECT REPLACE(gamequestions2.Question,'\r','|') from gamequestions2 where Question REGEXP "\r";
            //SELECT REPLACE(gamequestions2.Question,'\n','|') from gamequestions2 where Question REGEXP "\n";

            int successes = 0;

            string errors = "";

            try
            {
                SqlConnection.ConnectionString = DatabasesSettings.Default.showtimeDBconnectionMSSQL;
                SqlConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " " + e.InnerException);
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"MS SQL Importing questions located on desktop file ({Properties.Settings.Default.FileNameWithExtensionForQuestionImport})\r\nto server/datasource: {SqlConnection.DataSource} database: {SqlConnection.Database} ?");

            writeallq(rowsofquestion);

            Console.WriteLine("------------Press any key to continue...------------");
            Console.ReadKey();

            Console.Clear();

            try
            {
                ResetAutoIncrement0();
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                Console.WriteLine(e.Message + " " + e.InnerException);
                Console.ReadKey();
                return;
            }

            myTrans = SqlConnection.BeginTransaction();

            for (int i = 0; i < rowsofquestion.Length; i++)
            {
                string[] questionrow = rowsofquestion.ElementAt(i).Split('\t');

                Console.ForegroundColor = ConsoleColor.White;

                if (!checkfornewline(questionrow))
                {
                    Console.ForegroundColor = ConsoleColor.Green;

                    try
                    {
                        ImportQuestionInDatabase(questionrow);
                        successes += 1;

                        Console.WriteLine($"-------------OK ROW {(i + 1).ToString()} ---------------");

                    }
                    catch (Exception e)
                    {
                        errors += (i + 1).ToString() + "; ";

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"-----------ERROR AT ROW {(i + 1).ToString()} -------------");
                        Console.WriteLine(e.Message + " " + e.InnerException);

                        //break;
                    }
                }
                else
                {
                    Console.WriteLine($"New line must not be part of a question: line {(i + 1).ToString()}");
                    break;
                }

            }

            Console.WriteLine();

            if (successes != rowsofquestion.Length) //ima greski! napravi rollback
            {
                Console.ForegroundColor = ConsoleColor.Red;
                try
                {
                    myTrans.Rollback();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    if (myTrans.Connection != null)
                    {
                        Console.WriteLine("An exception of type " + ex.GetType() +
                        " was encountered while attempting to roll back the transaction.");
                    }
                }
                finally
                {
                    SqlConnection.Close();
                    SqlConnection.Dispose();
                }

                Console.Write($"Error while importing at row(s): {errors.Trim()} \r\nNo question is imported.");
            }
            else
            {
                try
                {
                    myTrans.Commit();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Successfuly imported: {successes}/{rowsofquestion.Length} questions");
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    Console.WriteLine(e.Message + " " + e.InnerException);
                }
                finally
                {
                    SqlConnection.Close();
                    SqlConnection.Dispose();
                }

                try
                {
                    string path = string.Format("{0}/{1}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Properties.Settings.Default.FileNameWithExtensionForQuestionImport);
                    // This text is added only once to the file.
                    if (!File.Exists(path))
                    {
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine("");
                            sw.WriteLine("Hello");
                            sw.WriteLine("And");
                            sw.WriteLine("Welcome");
                        }
                    }

                    // This text is always added, making the file longer over time
                    // if it is not deleted.
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("This");
                        sw.WriteLine("is Extra");
                        sw.WriteLine("Text");
                    }
                }
                catch (Exception e)
                {

                } //damage file intentionally to not be imported again

            }

            Console.ReadKey();
        }


        private static bool ImportQuestionInDatabase(string[] questionrow)
        {
            try
            {
                //TODO: Validation
                UpdateQuestionInDB(questionrow);
            }
            catch (Exception e)
            {
                throw;// Console.WriteLine(e.Message + " " + e.InnerException);
                return false;
            }
            return true;
        }

        public static void ResetAutoIncrement0()
        {
            try
            {
                cmd = SqlConnection.CreateCommand();
                string questiontable = DatabasesSettings.Default.QuestionDB_setTable1;
                cmd.CommandText = $"SELECT MAX(QuestionID) FROM {questiontable}";
                var maxQID = cmd.ExecuteScalar();
                Console.WriteLine("MAX-QID"+maxQID);
                cmd.CommandText = $"DBCC CHECKIDENT ({questiontable}, RESEED, {maxQID})";
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void UpdateQuestionInDB(string[] questionrow)
        {
            if ((questionrow.Length < 14)) { throw new Exception($"Question must have at least 14 fields. {questionrow.Length.ToString()} supplied"); }

            string Pronunciation = "";
            if (questionrow.Length > 14)
            {
                Pronunciation = questionrow.ElementAt(14);
            }

            try
            {
                String gameqTable = DatabasesSettings.Default.QuestionDB_setTable1;
                cmd = SqlConnection.CreateCommand();
                cmd.Transaction = myTrans;
                cmd.CommandText = "insert into " + gameqTable
                + " (Difficulty,Type,Question,Answer1,Answer2,Answer3,Answer4,CorrectAnswer,CategoryID,SubcategoryID,AdditionalCategoryID,AdditionalSubcategoryID,MoreInformation,Pronunciation,TimesAnswered) "
                + " values (@Difficulty,@Type,@Question,@Answer1,@Answer2,@Answer3,@Answer4,@CorrectAnswer,@CategoryID,@SubcategoryID,@AdditionalCategoryID,@AdditionalSubcategoryID,@MoreInformation,@Pronunciation,@TimesAnswered) ";
                //+ " on duplicate key update QuestionID=@QuestionID,Difficulty=@Difficulty,Type=@Type,Question=@Question,Answer1=@Answer1,Answer2=@Answer2,Answer3=@Answer3,Answer4=@Answer4,CorrectAnswer=@CorrectAnswer,CategoryID=@CategoryID,SubcategoryID=@SubcategoryID,MoreInformation=@MoreInformation,Pronunciation=@Pronunciation,Comments=@Comments,TimesAnswered=@TimesAnswered";
                cmd.Parameters.AddWithValue("@Difficulty", questionrow.ElementAt(4));
                cmd.Parameters.AddWithValue("@Type", questionrow.ElementAt(5));
                cmd.Parameters.AddWithValue("@Question", questionrow.ElementAt(6));
                cmd.Parameters.AddWithValue("@Answer1", questionrow.ElementAt(7));
                cmd.Parameters.AddWithValue("@Answer2", questionrow.ElementAt(8));
                cmd.Parameters.AddWithValue("@Answer3", questionrow.ElementAt(9));
                cmd.Parameters.AddWithValue("@Answer4", questionrow.ElementAt(10));
                cmd.Parameters.AddWithValue("@CorrectAnswer", questionrow.ElementAt(11));
                cmd.Parameters.AddWithValue("@CategoryID", questionrow.ElementAt(0));
                cmd.Parameters.AddWithValue("@SubcategoryID", questionrow.ElementAt(1));
                cmd.Parameters.AddWithValue("@AdditionalCategoryID", questionrow.ElementAt(2));
                cmd.Parameters.AddWithValue("@AdditionalSubcategoryID", questionrow.ElementAt(3));
                cmd.Parameters.AddWithValue("@MoreInformation", questionrow.ElementAt(12));
                cmd.Parameters.AddWithValue("@Pronunciation", Pronunciation);
                cmd.Parameters.AddWithValue("@TimesAnswered", questionrow.ElementAt(13));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            //finally
            //{
            //    MySqlConnection.Dispose();
            //}
        }

        private static void writeallq(string[] readlines)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            foreach (string q in readlines)
            {
                Console.WriteLine(q.Replace("\t", " "));
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static bool checkfornewline(string[] questionrownext)
        {
            foreach (string p in questionrownext)
            {
                if (p.Contains("\r\n") || p.Contains("\r") || p.Contains("\n"))
                {
                    return true;
                }
            }
            return false;
        }

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
