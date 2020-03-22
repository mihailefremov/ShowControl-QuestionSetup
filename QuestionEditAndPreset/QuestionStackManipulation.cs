using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuestionEditAndPreset
{

    class QuestionStackManipulation
    {
        public MySqlConnection mySqlQAStacksDB = new MySqlConnection();
        public MySqlCommand cmd = new MySqlCommand();
        public bool CanConnectStacks = false;
 
        public void ConnectQAStacksDB()
        {
            mySqlQAStacksDB.ConnectionString = DatabasesSettings.Default.questionDBconnectionSQL;
            try
            {
                mySqlQAStacksDB.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DisconnectDB(MySqlConnection mySql)
        {
            try
            {
                mySql.Dispose();
                mySql.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public DataTable RefreshAllQuestionsTable(string QRef, string QLevel, string QCategory, string QSubcategory, string QAtextContain, string QAlastFirst, string QAwhereToSearch)
        {
            MySqlDataAdapter SDA = new MySqlDataAdapter();
            System.Data.DataTable dbDataSet = new System.Data.DataTable();
            BindingSource bSource = new BindingSource();
            string QuestionTable = DatabasesSettings.Default.QuestionDB_setTable1;
            CanConnectStacks = false;
            ConnectQAStacksDB();
            if (mySqlQAStacksDB.State == ConnectionState.Open)
            {
                string Query;
                Query = ("SELECT " +
                        " gq.QuestionID AS `ID`, qs.StackID AS `Stack`, " +
                        "gq.Question AS `Question`, " 
                        + "gq.Answer1 AS `Answer A`, " + "gq.Answer2 AS `Answer B`, " 
                        + "gq.Answer3 AS `Answer C`, " + "gq.Answer4 AS `Answer D`, " +
                        "gq.CorrectAnswer AS `Correct`, " +
                        " gq.TimesAnswered AS `Used` " +
                        "FROM " + QuestionTable + " gq " + " left join " +
                        DatabasesSettings.Default.QuestionDB_setTable5 +
                        " qs on gq.QuestionID=qs.QuestionID " +
                        "WHERE 1 " + " order by ID asc ");
                MySqlCommand COMM;
                COMM = new MySqlCommand(Query, mySqlQAStacksDB);
                SDA.SelectCommand = COMM;
                SDA.Fill(dbDataSet);
                SDA.Dispose();
                CanConnectStacks = true;
                DisconnectDB(mySqlQAStacksDB);
                return dbDataSet;
            }
            else
            {
                return null;
            }
        }

        public void AddStackWithQuestions(int StackType, string StackName)
        {
            ConnectQAStacksDB();
            if (mySqlQAStacksDB.State == ConnectionState.Open)
            {
                string stackNameTable = DatabasesSettings.Default.QuestionDB_setTable4;
                cmd = mySqlQAStacksDB.CreateCommand();
                cmd.CommandText = "insert into " + stackNameTable + "(Stack,Type) values(@Stack,@Type)";
                cmd.Parameters.AddWithValue("@Stack", StackName);
                cmd.Parameters.AddWithValue("@Type", StackType);
                cmd.ExecuteNonQuery();
            }
            DisconnectDB(mySqlQAStacksDB);
        }

        public void BuildStackWithQuestions(string StackID, string StackType)
        {
            int j=0;
            if (StackType == "1") {
                j = AdvancedGameConfigSettings.Default.GamePlay1_TotalStackQ;
            } else if (StackType == "2") {
                j = AdvancedGameConfigSettings.Default.QualificationGame1_TotalStackQ;
            }
            ConnectQAStacksDB();
            for (int StackLev=1; StackLev<=j; StackLev++)
            {
                InsertQuestionInQuestionStack(StackID, StackLev, GetRandomlySelectedQuestion(StackID, StackLev, StackType));
            }
            DisconnectDB(mySqlQAStacksDB);
        }

        //public int GetRandomlySelectedQuestion(string StackID, int Level, string Type)
        //{
        //    MySqlDataAdapter rda = new MySqlDataAdapter();
        //    DataTable dbDataSet = new DataTable();
        //    string qaRandomID = "-1";
        //    string qaDifficulty = MapLevelDifficultyGameSettings.Default["Level" + Level].ToString();
        //    string GameQdatabase = DatabasesSettings.Default.QuestionDB_setTable1;
        //    string IsCategoryRepeatingQuery = String.Format(AdvancedGameConfigSettings.Default.CategoryNotRepeatingSelectionQuery, StackID);
        //    if (Type == "2")
        //    {
        //        qaDifficulty = (((Level+1) % 2)+1).ToString(); //odd-easy even-difficult
        //        IsCategoryRepeatingQuery = "";
        //    }

        //    if (mySqlQAStacksDB.State == ConnectionState.Open)
        //    {
        //        cmd = mySqlQAStacksDB.CreateCommand();
        //        cmd.CommandText = "Select QuestionID from "
        //            + GameQdatabase + " where Difficulty=" + qaDifficulty + " "
        //            + AdvancedGameConfigSettings.Default.UsageSelectionQuery + " and Type=" + Type
        //            + " and QuestionID not in (Select QuestionID from questionstackitems) "
        //            + IsCategoryRepeatingQuery
        //            + " order by RAND() limit 1";
        //        rda.SelectCommand = cmd;
        //        rda.Fill(dbDataSet);
        //        if (dbDataSet.Rows.Count > 0)
        //        {
        //            qaRandomID = dbDataSet.Rows[0]["QuestionID"].ToString(); //ID QUESTIONID
        //        }

        //    }
        //    return Convert.ToInt32(qaRandomID);
        //}

        /*
        DELIMITER $$
        CREATE DEFINER=`root`@`localhost` PROCEDURE `proc_GetRandomlySelectedQuestion`(IN `StackID` BIGINT, IN `Type` INT, IN `Difficulty` INT, IN `TimesAnswered` INT, OUT `QuestionID` BIGINT)
        proc_GetRandomlySelectedQuestion:BEGIN
            DECLARE count_prim INT;
            DECLARE count_sec INT;
            DECLARE count_ter INT; 
	
	        SET QuestionID = -1;
    
        SELECT COUNT(*) INTO count_prim FROM gamequestions WHERE gamequestions.Type = Type AND gamequestions.Difficulty = Difficulty AND (gamequestions.CategoryID, gamequestions.SubcategoryID, gamequestions.AdditionalCategoryID, gamequestions.AdditionalSubcategoryID) NOT IN (SELECT questionstackitems.CategoryID, questionstackitems.SubcategoryID, questionstackitems.AdditionalCategoryID, questionstackitems.AdditionalSubcategoryID FROM questionstackitems WHERE questionstackitems.StackID = StackID) AND gamequestions.QuestionID NOT IN (SELECT questionstackitems.QuestionID FROM questionstackitems) AND gamequestions.TimesAnswered=TimesAnswered;


        IF (count_prim > 0) THEN
            SELECT gamequestions.QuestionID INTO QuestionID FROM gamequestions WHERE gamequestions.Type = TYPE AND gamequestions.Difficulty = Difficulty AND (gamequestions.CategoryID, gamequestions.SubcategoryID, gamequestions.AdditionalCategoryID, gamequestions.AdditionalSubcategoryID) NOT IN (SELECT questionstackitems.CategoryID, questionstackitems.SubcategoryID, questionstackitems.AdditionalCategoryID, questionstackitems.AdditionalSubcategoryID FROM questionstackitems WHERE questionstackitems.StackID = StackID) AND gamequestions.QuestionID NOT IN (SELECT questionstackitems.QuestionID FROM questionstackitems) AND gamequestions.TimesAnswered=TimesAnswered ORDER BY RAND() LIMIT 1;
	        LEAVE proc_GetRandomlySelectedQuestion;
        end if;

        SELECT COUNT(*) INTO count_sec FROM gamequestions WHERE gamequestions.Type = TYPE AND gamequestions.Difficulty = Difficulty AND (gamequestions.CategoryID, gamequestions.SubcategoryID) NOT IN (SELECT questionstackitems.CategoryID, questionstackitems.SubcategoryID FROM questionstackitems WHERE questionstackitems.StackID = StackID) AND gamequestions.QuestionID NOT IN (SELECT questionstackitems.QuestionID FROM questionstackitems) AND gamequestions.TimesAnswered=TimesAnswered;

        IF (count_sec > 0) then
	        SELECT gamequestions.QuestionID INTO QuestionID FROM gamequestions WHERE gamequestions.Type = TYPE AND gamequestions.Difficulty = Difficulty AND (gamequestions.CategoryID, gamequestions.SubcategoryID) NOT IN (SELECT questionstackitems.CategoryID, questionstackitems.SubcategoryID FROM questionstackitems WHERE questionstackitems.StackID = StackID) AND gamequestions.QuestionID NOT IN (SELECT questionstackitems.QuestionID FROM questionstackitems) AND gamequestions.TimesAnswered=TimesAnswered ORDER BY RAND() LIMIT 1;
	        LEAVE proc_GetRandomlySelectedQuestion;
        end if;

        SELECT COUNT(*) INTO count_ter FROM gamequestions WHERE gamequestions.Type = TYPE AND gamequestions.Difficulty = Difficulty AND gamequestions.QuestionID NOT IN (SELECT questionstackitems.QuestionID FROM questionstackitems) AND gamequestions.TimesAnswered=TimesAnswered;

        IF (count_ter > 0) then
	        SELECT gamequestions.QuestionID INTO QuestionID FROM gamequestions WHERE gamequestions.Type = TYPE AND gamequestions.Difficulty = Difficulty AND gamequestions.QuestionID NOT IN (SELECT questionstackitems.QuestionID FROM questionstackitems) AND gamequestions.TimesAnswered=TimesAnswered ORDER BY RAND() LIMIT 1;
	        LEAVE proc_GetRandomlySelectedQuestion;
        end if;
 
        END$$
        DELIMITER ;
        */

        public int GetRandomlySelectedQuestion(string StackID, int Level, string Type)
        {
            MySqlDataAdapter rda = new MySqlDataAdapter();
            DataTable dbDataSet = new DataTable();
            string qaRandomID = "-1";
            string qaDifficulty = MapLevelDifficultyGameSettings.Default["Level" + Level].ToString();
            string GameQdatabase = DatabasesSettings.Default.QuestionDB_setTable1;
 
            if (Type == "2")
            {
                qaDifficulty = (((Level + 1) % 2) + 1).ToString(); //odd-easy even-difficult
            }

            //and TimesAnswered>0

            if (mySqlQAStacksDB.State == ConnectionState.Open)
            {
                cmd = mySqlQAStacksDB.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "proc_GetRandomlySelectedQuestion";

                cmd.Parameters.AddWithValue("@StackID", StackID);
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@Difficulty", qaDifficulty);
                cmd.Parameters.AddWithValue("@TimesAnswered", AdvancedGameConfigSettings.Default.UsageSelectionQuery);

                cmd.Parameters.AddWithValue("@QuestionID", MySqlDbType.Int64);
                cmd.Parameters["@QuestionID"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                qaRandomID = cmd.Parameters["@QuestionID"].Value.ToString();

                //rda.SelectCommand = cmd;
                //rda.Fill(dbDataSet);
                //if (dbDataSet.Rows.Count > 0)
                //{
                //    qaRandomID = dbDataSet.Rows[0]["QuestionID"].ToString(); //ID QUESTIONID
                //} VAKA AKO NEMA OUT PARAMETAR TUKU KOGA VRAKJA TABELA

            }
            return Convert.ToInt32(qaRandomID);
        }

        public void InsertQuestionInQuestionStack(string StackID, int StackLevel, int QuestionID)
        {
            //ConnectQAStacksDB();
            if (mySqlQAStacksDB.State == ConnectionState.Open)
            {
                string stackItemTable = DatabasesSettings.Default.QuestionDB_setTable5;
                cmd = mySqlQAStacksDB.CreateCommand();
                cmd.CommandText = "insert into " + stackItemTable +
                "(StackID,StackLevel,QuestionID) values(@StackID,@StackLevel,@QuestionID) " +
                " on duplicate key update QuestionID = @QuestionID";
                cmd.Parameters.AddWithValue("@StackID", StackID);
                cmd.Parameters.AddWithValue("@StackLevel", StackLevel);
                cmd.Parameters.AddWithValue("@QuestionID", QuestionID);
                cmd.ExecuteNonQuery();
            }
            //DisconnectDB(mySqlQAStacksDB);
        }

        public void DeleteStackWithQuestions(string StackID)
        {
            ConnectQAStacksDB();
            if (mySqlQAStacksDB.State == ConnectionState.Open)
            {
                string stackNamesTable = DatabasesSettings.Default.QuestionDB_setTable4;
                string stackItemTable = DatabasesSettings.Default.QuestionDB_setTable5;
                cmd = mySqlQAStacksDB.CreateCommand();
                cmd.CommandText = "delete from " + stackNamesTable + " where StackID = " + StackID;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "delete from " + stackItemTable + " where StackID = " + StackID;
                cmd.ExecuteNonQuery();
            }
            DisconnectDB(mySqlQAStacksDB);
        }

        public void MoveQuestionInStack(string StackID, string OldLevel, string NewLevel)
        {
            ConnectQAStacksDB();
            if (mySqlQAStacksDB.State == ConnectionState.Open)
            {
                string stackItemTable = DatabasesSettings.Default.QuestionDB_setTable5;
                cmd = mySqlQAStacksDB.CreateCommand();
                cmd.CommandText = "update " + stackItemTable + " set StackLevel=-2 " + " where StackID = " + StackID + " and StackLevel=" + OldLevel + ";";
                cmd.CommandText += "update " + stackItemTable + " set StackLevel=-4 " + " where StackID = " + StackID + " and StackLevel=" + NewLevel + ";";
                cmd.CommandText += "update " + stackItemTable + " set StackLevel=" + NewLevel + " where StackID = " + StackID + " and StackLevel=-2;";
                cmd.CommandText += "update " + stackItemTable + " set StackLevel=" + OldLevel + " where StackID = " + StackID + " and StackLevel=-4;";

                cmd.ExecuteNonQuery();
            }
            DisconnectDB(mySqlQAStacksDB);
        }

        public void MoveQuestionInLIVEStack(string LiveStackType, string OldLevel, string NewLevel)
        {
            ConnectQAStacksDB();
            if (mySqlQAStacksDB.State == ConnectionState.Open)
            {
                string liveStackTable = DatabasesSettings.Default.ShowtimeDB_defaultTable1;
                cmd = mySqlQAStacksDB.CreateCommand();
                /*
                UPDATE
                    questionsforcontestant a
                INNER JOIN questionsforcontestant b ON
                    a.Level <> b.Level
                SET
                    a.QuestionID = b.QuestionID,
                    a.Type = b.Type,
                    a.Question = b.Question,
                    a.Answer1 = b.Answer1,
                    a.Answer2 = b.Answer2,
                    a.Answer3 = b.Answer3,
                    a.Answer4 = b.Answer4,
                    a.CorrectAnswer = b.CorrectAnswer,
                    a.MoreInformation = b.MoreInformation,
                    a.Pronunciation = b.Pronunciation,
                    a.Answered = b.Answered,
                    a.QuestionCreator = b.QuestionCreator,
                    a.Comments = b.Comments
                WHERE --smeni prvo so vtoro prasanje (1,2), selektiraj od tip prasanja samo za glavna igra (Type=1) 
                    a.Level IN(1, 2) AND b.Level IN(1, 2) AND a.Type = 1 AND b.Type = 1
                */
                cmd.CommandText = " update " + liveStackTable + " set Level=-2 " + " where Type = " + LiveStackType + " and Level=" + OldLevel + ";";
                cmd.CommandText += "update " + liveStackTable + " set Level=-4 " + " where Type = " + LiveStackType + " and Level=" + NewLevel + ";";
                cmd.CommandText += "update " + liveStackTable + " set Level=" + NewLevel + " where Type = " + LiveStackType + " and Level=-2;";
                cmd.CommandText += "update " + liveStackTable + " set Level=" + OldLevel + " where Type = " + LiveStackType + " and Level=-4;";

                cmd.ExecuteNonQuery();
            }
            DisconnectDB(mySqlQAStacksDB);
        }

        public void ClearOneQuestionInStack(string StackID, string Level)
        {
            ConnectQAStacksDB();
            if (mySqlQAStacksDB.State == ConnectionState.Open)
            {
                string stackItemTable = DatabasesSettings.Default.QuestionDB_setTable5;
                cmd = mySqlQAStacksDB.CreateCommand();
                cmd.CommandText = "update " + stackItemTable + " set QuestionID=-1 " +
                    " where StackID = " + StackID + " and StackLevel=" + Level;                    
                cmd.ExecuteNonQuery();
            }
            DisconnectDB(mySqlQAStacksDB);
        }

        public void ClearAllQuestionsInStack(string StackID)
        {
            ConnectQAStacksDB();
            if (mySqlQAStacksDB.State == ConnectionState.Open)
            {
                string stackItemTable = DatabasesSettings.Default.QuestionDB_setTable5;
                cmd = mySqlQAStacksDB.CreateCommand();
                cmd.CommandText = "delete from " + stackItemTable + " where StackID = " + StackID;
                cmd.ExecuteNonQuery();
            }
            DisconnectDB(mySqlQAStacksDB);
        }

        public DataTable RefreshStackList()
        {
            MySqlDataAdapter SDA = new MySqlDataAdapter();
            System.Data.DataTable dbDataSet = new System.Data.DataTable();
            string tbl1 = DatabasesSettings.Default.QuestionDB_setTable4;
            string tbl2 = DatabasesSettings.Default.QuestionDB_setTable5;
            CanConnectStacks = false;
            ConnectQAStacksDB();
            if (mySqlQAStacksDB.State == ConnectionState.Open)
            {
                string Query;
                Query = ("select t1.StackID as ID, t1.Stack, t1.Type from " + tbl1 + " t1 "); // "inner join " + tbl2 + " t2 on t1.StackID = t2.StackID group by ID, Stack, Type"); //, Count(t1.StackID) as Status
                MySqlCommand COMM;
                COMM = new MySqlCommand(Query, mySqlQAStacksDB);
                SDA.SelectCommand = COMM;
                SDA.Fill(dbDataSet);
                SDA.Dispose();
                CanConnectStacks = true;
                DisconnectDB(mySqlQAStacksDB);
                return dbDataSet;
            }
            else
            {
                return null;
            }          
        }

        public DataTable RefreshStackQuestions(string StackID)
        {
            MySqlDataAdapter SDA = new MySqlDataAdapter();
            System.Data.DataTable dbDataSet = new System.Data.DataTable();
            BindingSource bSource = new BindingSource();
            string QuestionTable = DatabasesSettings.Default.QuestionDB_setTable1;
            //if (StackType == "2")
            //{
            //    QuestionTable = DatabasesSettings.Default.QuestionFFFDB_setTable1;
            //}
            ConnectQAStacksDB();
            if (mySqlQAStacksDB.State == ConnectionState.Open)
            {
                string Query;
                Query = ("SELECT qs.StackLevel AS `Level`, gq.CategoryID, gq.SubcategoryID, gq.Difficulty, gq.Type, " +
                        " qs.QuestionID AS `ID`, gq.Question AS `Question`, gq.TimesAnswered AS `Used` " +
                        "FROM " + QuestionTable + " gq right join " +
                        DatabasesSettings.Default.QuestionDB_setTable5 +
                        " qs on gq.QuestionID=qs.QuestionID " +
                        "WHERE qs.StackID=" + StackID + " order by Level desc ");
                MySqlCommand COMM;
                COMM = new MySqlCommand(Query, mySqlQAStacksDB);
                SDA.SelectCommand = COMM;
                SDA.Fill(dbDataSet);
                SDA.Dispose();
                DisconnectDB(mySqlQAStacksDB);
                return dbDataSet;
            }
            else
            {
                return null;
            }
 
        }
       public Dictionary<String, String> CategoryFromDB()
        {
            Dictionary<string, string> CategoryDict = new Dictionary<string, string>();
            MySqlDataAdapter SDA = new MySqlDataAdapter();
            DataTable dbDataSet = new DataTable();
            ConnectQAStacksDB();
            if (mySqlQAStacksDB.State == ConnectionState.Open)
            {
                string Query;
                Query = ("SELECT CategoryID, Category FROM " + DatabasesSettings.Default.QuestionDB_setTable2);
                MySqlCommand COMM = new MySqlCommand(Query, mySqlQAStacksDB);
                SDA.SelectCommand = COMM;
                SDA.Fill(dbDataSet);
                foreach (DataRow row in dbDataSet.Rows)
                {
                    CategoryDict.Add(row.ItemArray[0].ToString(), row.ItemArray[1].ToString());
                }
                SDA.Dispose();
                DisconnectDB(mySqlQAStacksDB);
                return CategoryDict;
            }
            else
            {
                return null;
            }

        }

        public Dictionary<String, String> SubcategoryFromDB(string CategoryID)
        {
            Dictionary<string, string> SubcategoryDict = new Dictionary<string, string>();
            MySqlDataAdapter SDA = new MySqlDataAdapter();
            DataTable dbDataSet = new DataTable();
            ConnectQAStacksDB();
            if (mySqlQAStacksDB.State == ConnectionState.Open)
            {
                string Query;
                Query = ("SELECT SubcategoryID, Subcategory FROM " + DatabasesSettings.Default.QuestionDB_setTable3 + " where CategoryID=" + CategoryID);
                MySqlCommand COMM = new MySqlCommand(Query, mySqlQAStacksDB);
                SDA.SelectCommand = COMM;
                SDA.Fill(dbDataSet);
                foreach (DataRow row in dbDataSet.Rows)
                {
                    SubcategoryDict.Add(row.ItemArray[0].ToString(), row.ItemArray[1].ToString());
                }
                SDA.Dispose();
                DisconnectDB(mySqlQAStacksDB);
                return SubcategoryDict;
            }
            else
            {
                return null;
            }
        }

        public void InsertQuestionsInLIVEStack(string StackID, string StackType)
        {
            ConnectQAStacksDB();
            if (mySqlQAStacksDB.State == ConnectionState.Open)
            {
                string showTimeItemTable = DatabasesSettings.Default.ShowtimeDB_setTable1;
                string stackItemsTable = DatabasesSettings.Default.QuestionDB_setTable5;
                string gameqTable = "";
                gameqTable = DatabasesSettings.Default.QuestionDB_setTable1;
                cmd = mySqlQAStacksDB.CreateCommand();
                cmd.CommandText = "insert into " + showTimeItemTable +
                "(Level,QuestionID,Type,Question,Answer1,Answer2,Answer3,Answer4,CorrectAnswer,MoreInformation,Pronunciation,Answered) " +
                " select sq.StackLevel, gq.QuestionID, gq.Type, gq.Question, gq.Answer1, gq.Answer2, gq.Answer3, gq.Answer4, gq.CorrectAnswer, gq.MoreInformation, gq.Pronunciation, gq.TimesAnswered from " +
                stackItemsTable + " sq, " + gameqTable + " gq " + " where gq.QuestionID=sq.QuestionID and sq.StackID=" + StackID +
                " on duplicate key update QuestionID=gq.QuestionID, Level=sq.StackLevel, Type=gq.Type, Question=gq.Question, Answer1=gq.Answer1, Answer2=gq.Answer2, Answer3=gq.Answer3, Answer4=gq.Answer4, CorrectAnswer=gq.CorrectAnswer, MoreInformation=gq.MoreInformation, Pronunciation=gq.Pronunciation, Answered=gq.TimesAnswered";
                //cmd.Parameters.AddWithValue("@StackID", StackID);
                cmd.ExecuteNonQuery();
                DisconnectDB(mySqlQAStacksDB);
                //MessageBox.Show("Selected Question Stack is now LIVE!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void RenameStack(string StackID, string NewStackName)
        {
            ConnectQAStacksDB();
            if (mySqlQAStacksDB.State == ConnectionState.Open)
            {
                string stackNamesTable = DatabasesSettings.Default.QuestionDB_setTable4;
                cmd = mySqlQAStacksDB.CreateCommand();
                cmd.CommandText = "update " + stackNamesTable +
                " set Stack='" + NewStackName + "' where StackID="+StackID;
                cmd.ExecuteNonQuery();
                DisconnectDB(mySqlQAStacksDB);
                //MessageBox.Show("Stack renamed!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
        }
    }
}
