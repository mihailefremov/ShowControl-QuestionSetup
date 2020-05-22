using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUpdaterGameQuestions
{
    class MsSqlDataReaderLayer
    {
        private static SqlConnection SqlConnection = new SqlConnection();
        private static SqlCommand cmd = new SqlCommand();

        public void ExportAllQuestions()
        {
            
            SqlConnection.ConnectionString = DatabasesSettings.Default.showtimeDBconnectionMSSQL;
            SqlConnection.Open();
            cmd = SqlConnection.CreateCommand();

            string questiontable = DatabasesSettings.Default.QuestionDB_setTable1;
            cmd.CommandText = $"SELECT * FROM {questiontable}";

            string exportdatapath = string.Format("{0}/{1}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Properties.Settings.Default.FileNameWithoutExtensionForQuestionExport);
            exportdatapath = exportdatapath + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            System.IO.File.WriteAllText(exportdatapath, ""); //isprazni go fajlot

            //List<string> questions = new List<string>();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                    var line = ReadSingleRow(reader);
                    //questions.Add(line);

                    try
                    {
                        // This text is always added, making the file longer over time
                        // if it is not deleted.
                        using (StreamWriter sw = File.AppendText(exportdatapath))
                        {
                            sw.WriteLine(line);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
            }

            SqlConnection.Close();

        }

        private static string ReadSingleRow(System.Data.IDataRecord reader)
        {
            var line = (String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}\t{18}\t{19}",
                reader[0].ToString().Replace("\t", " "), reader[1].ToString().Replace("\t", " "), reader[2].ToString().Replace("\t", " "), reader[3].ToString().Replace("\t", " "),
                reader[4].ToString().Replace("\t", " "), reader[5].ToString().Replace("\t", " "), reader[6].ToString().Replace("\t", " "), reader[7].ToString().Replace("\t", " "),
                reader[8].ToString().Replace("\t", " "), reader[9].ToString().Replace("\t", " "), reader[10].ToString().Replace("\t", " "), reader[11].ToString().Replace("\t", " "), reader[12].ToString().Replace("\t", " "), reader[13].ToString().Replace("\t", " "),
                reader[14].ToString().Replace("\t", " "), reader[15].ToString().Replace("\t", " "), reader[16].ToString().Replace("\t", " "), reader[17].ToString().Replace("\t", " "),
                reader[18].ToString().Replace("\t", " "), reader[19])
                );
            Console.WriteLine(line);
            return line;
        }

    }
}
