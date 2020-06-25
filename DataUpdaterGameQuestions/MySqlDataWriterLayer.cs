using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUpdaterGameQuestions
{
    class MySqlDataWriterLayer
    {
        public static MySqlConnection MySqlConnection = new MySqlConnection();
        public static MySqlCommand cmd = new MySqlCommand();

        public void ImportAllQuestions()
        {
            //SELECT REPLACE(gamequestions2.Question,'\r\n','|') from gamequestions2 where Question REGEXP "\r\n";
            //SELECT REPLACE(gamequestions2.Question,'\r','|') from gamequestions2 where Question REGEXP "\r";
            //SELECT REPLACE(gamequestions2.Question,'\n','|') from gamequestions2 where Question REGEXP "\n";

            int successes = 0;
            string errors = "";

            try
            {
                MySqlConnection.ConnectionString = DatabasesSettings.Default.showtimeDBconnectionMYSQL;
                MySqlConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " " + e.InnerException);
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Importing questions located on desktop file ({Properties.Settings.Default.FileNameWithExtensionForQuestionImport})\r\nto server/datasource: {MySqlConnection.DataSource} database: {MySqlConnection.Database} ?");
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

            MySqlTransaction myTrans; // Start a local transaction
            myTrans = MySqlConnection.BeginTransaction();

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
                    MySqlConnection.Close();
                    MySqlConnection.Dispose();
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
                    MySqlConnection.Close();
                    MySqlConnection.Dispose();
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
                            sw.WriteLine("Hello");
                            sw.WriteLine("And");
                            sw.WriteLine("Welcome");
                        }
                    }

                    // This text is always added, making the file longer over time
                    // if it is not deleted.
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("");
                        sw.WriteLine("This");
                        sw.WriteLine("is Extra");
                        sw.WriteLine("Text");
                    }
                }
                catch (Exception e)
                {

                } //damage file intentionally to not be imported again

            }

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
                cmd = MySqlConnection.CreateCommand();
                string database = DatabasesSettings.Default.QuestionDB_setTable1;
                cmd.CommandText = $"ALTER TABLE {database} AUTO_INCREMENT = 0";
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
                cmd = MySqlConnection.CreateCommand();
                cmd.CommandText = "insert into " + gameqTable
                + "(Difficulty,Type,Question,Answer1,Answer2,Answer3,Answer4,CorrectAnswer,CategoryID,SubcategoryID,AdditionalCategoryID,AdditionalSubcategoryID,MoreInformation,Pronunciation,TimesAnswered) "
                + " values(@Difficulty,@Type,@Question,@Answer1,@Answer2,@Answer3,@Answer4,@CorrectAnswer,@CategoryID,@SubcategoryID,@AdditionalCategoryID,@AdditionalSubcategoryID,@MoreInformation,@Pronunciation,@TimesAnswered) ";
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

    }

}