using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace QuestionEditAndPreset
{
    public partial class FORM_ReplacementQuestion : Form
    {
        string Stack="-1";
        string Type="0";
        string Level="0";
        string Category="0";
        string Subcategory="0";

        int FromVariation=0;
        int ToVariation = 0;

        //bool SameCategory = true;
        //bool SameSubcategory = false;
        //bool AnyCategory = false;

        string QuestionIDselection="-1";
        string CategSelect = "";

        public MySqlConnection MySqlConnection = new MySqlConnection();
        public MySqlCommand cmd = new MySqlCommand();

        public FORM_ReplacementQuestion(string StackID, string Type, string Level, string CategoryID, string SubcategoryID)
        {
            Stack = StackID;
            this.Type = Type;
            this.Level = Level;
            Category = CategoryID;
            Subcategory = SubcategoryID;
            if(Category == "")
            {
                Category = "0";
                Subcategory = "0";
            }

            InitializeComponent();
        }

        private void FORM_ReplacementQuestion_Load(object sender, EventArgs e)
        {
            FillSuggestedQuestions(GetSuggestedQuestions(Level, Type));
        }

        public void FillSuggestedQuestions(DataTable dbDataSet)
        {
            if (dbDataSet.Rows.Count > 0)
            {
                SugestedQuestionsPreview_dataGridView.DataSource = dbDataSet;

                SugestedQuestionsPreview_dataGridView.Columns[0].FillWeight = 10;
                SugestedQuestionsPreview_dataGridView.Columns[1].FillWeight = 100;
                SugestedQuestionsPreview_dataGridView.Columns[2].FillWeight = 14;

                SugestedQuestionsPreview_dataGridView.Columns[0].Width = 40;
                SugestedQuestionsPreview_dataGridView.Columns[1].Width = 281;
                SugestedQuestionsPreview_dataGridView.Columns[2].Width = 50;
            }
        }

        //public DataTable GetSuggestedQuestions(string Level, string Type)
        //{
        //    MySqlDataAdapter rda = new MySqlDataAdapter();
        //    DataTable dbDataSet = new DataTable();

        //    String qaDifficulty = MapLevelDifficultyGameSettings.Default["Level" + Level].ToString();

        //    MySqlConnection.ConnectionString = DatabasesSettings.Default.questionDBconnectionSQL;
        //    String GameQdatabase = "";

        //    if(Type=="1" || Type == "2")
        //    {
        //        GameQdatabase = DatabasesSettings.Default.QuestionDB_setTable1;
        //    }

        //    CategSelect = "";
        //    if (SameCategory_radioButton.Checked==true) {
        //        CategSelect = " and CategoryID=" + Category + " ";
        //        if (SameSubcategory_checkBox.Checked == true) {
        //            CategSelect += " and SubcategoryID=" + Subcategory + " ";
        //        }
        //    }
        //    else {
        //        CategSelect = "";
        //    }

        //    try
        //    {
        //        MySqlConnection.Open();
        //        cmd = MySqlConnection.CreateCommand();
        //        cmd.CommandText = "Select QuestionID as ID, Question, TimesAnswered as Used from " + GameQdatabase 
        //            + " where Difficulty>=" + (Convert.ToInt16(qaDifficulty) + Convert.ToInt16(FromVariation)) 
        //            + " and Difficulty<=" + (Convert.ToInt16(qaDifficulty) + Convert.ToInt16(ToVariation))
        //            + " " + CategSelect + " "
        //            + AdvancedGameConfigSettings.Default.UsageSelectionQuery + " and Type=" + Type
        //            + " and QuestionID not in (Select QuestionID from questionstackitems) "
        //            + " order by RAND() limit " + FetchQ_textBox.Text;
        //        rda.SelectCommand = cmd;
        //        rda.Fill(dbDataSet);
        //        MySqlConnection.Close();
        //        return dbDataSet;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        return null;
        //    }
        //    finally
        //    {
        //        MySqlConnection.Close();
        //        MySqlConnection.Dispose();
        //    }

        //}

        public DataTable GetSuggestedQuestions(string Level, string Type)
        {
            MySqlDataAdapter rda = new MySqlDataAdapter();
            DataTable dbDataSet = new DataTable();

            String qaDifficulty = MapLevelDifficultyGameSettings.Default["Level" + Level].ToString();

            MySqlConnection.ConnectionString = DatabasesSettings.Default.questionDBconnectionSQL;
            String GameQdatabase = "";

            if (Type == "1" || Type == "2")
            {
                GameQdatabase = DatabasesSettings.Default.QuestionDB_setTable1;
            }

            CategSelect = "";
            if (SameCategory_radioButton.Checked == true)
            {
                CategSelect = " and CategoryID=" + Category + " ";
                if (SameSubcategory_checkBox.Checked == true)
                {
                    CategSelect += " and SubcategoryID=" + Subcategory + " ";
                }
            }
            else
            {
                CategSelect = "";
            }

            try
            {
                MySqlConnection.Open();
                cmd = MySqlConnection.CreateCommand();
                cmd.CommandText = "Select QuestionID as ID, Question, TimesAnswered as Used from " + GameQdatabase
                    + " where Difficulty>=" + (Convert.ToInt16(qaDifficulty) + Convert.ToInt16(FromVariation))
                    + " and Difficulty<=" + (Convert.ToInt16(qaDifficulty) + Convert.ToInt16(ToVariation))
                    + " " + CategSelect + " and TimesAnswered="
                    + AdvancedGameConfigSettings.Default.UsageSelectionQuery + " and Type=" + Type
                    + " and QuestionID not in (Select QuestionID from questionstackitems) "
                    + " order by RAND() limit " + FetchQ_textBox.Text;
                rda.SelectCommand = cmd;
                rda.Fill(dbDataSet);
                MySqlConnection.Close();
                return dbDataSet;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                MySqlConnection.Close();
                MySqlConnection.Dispose();
            }

        }

        private void RefreshQuestionList_Button_Click(object sender, EventArgs e)
        {
            FillSuggestedQuestions(GetSuggestedQuestions(Level, Type));
        }

        private void SearchQuestion_Button_Click(object sender, EventArgs e)
        {

        }

        private void FromVariation_trackBar_Scroll(object sender, EventArgs e)
        {
            FromVariation = FromVariation_trackBar.Value - 3;
            FromVariation_label.Text = FromVariation.ToString();
        }

        private void ToVariation_trackBar_Scroll(object sender, EventArgs e)
        {
            ToVariation = ToVariation_trackBar.Value - 3;
            ToVariation_label.Text = ToVariation.ToString();
        }

        private void CategorySelectionChange(object sender, EventArgs e)
        {
            CategSelect = "";
            if (SameCategory_radioButton.Checked == true)
            {
                CategSelect += " and CategoryID=" + Category + " ";

                if (SameSubcategory_checkBox.Checked == true)
                {
                    CategSelect += " and SubcategoryID=" + Subcategory + " ";
                }

            }
            if (AnyCategory_radioButton.Checked == true)
            {

            }

        }

        public void SugestedQuestionsPreview_SelectionChanged(object sender, EventArgs e)
        {
            //Level = Convert.ToString(SugestedQuestionsPreview_dataGridView.CurrentRow.Cells["Level"].Value);
            QuestionIDselection = Convert.ToString(SugestedQuestionsPreview_dataGridView.CurrentRow.Cells["ID"].Value);

        }

        public void InsertReplacementQuestionStack()
        {
            MySqlConnection.ConnectionString = DatabasesSettings.Default.showtimeDBconnectionSQL;
            string gameqTable = "";
            if (Type == "1" || Type == "2")
            {
                gameqTable = DatabasesSettings.Default.QuestionDB_setTable1;
            }

            try
            {
                MySqlConnection.Open();
                String stackItemsTable = DatabasesSettings.Default.QuestionDB_setTable5;
                
                cmd = MySqlConnection.CreateCommand();
                cmd.CommandText = "insert into " + stackItemsTable +
                "(StackID,StackLevel,QuestionID) " + " values(@StackID,@StackLevel,@QuestionID) " +
                " on duplicate key update QuestionID=@QuestionID, StackLevel=@StackLevel, QuestionID=@QuestionID";
                cmd.Parameters.AddWithValue("@StackID", Stack);
                cmd.Parameters.AddWithValue("@StackLevel", Level);
                cmd.Parameters.AddWithValue("@QuestionID", QuestionIDselection);
                cmd.ExecuteNonQuery();
                MySqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                MySqlConnection.Dispose();
            }
        }

        private void MakeReplacement_Button_Click(object sender, EventArgs e)
        {
            InsertReplacementQuestionStack();
            this.Close();
        }

        public void EditQuestion(string StackID, string QuestionID, int Privileges)
        {
            FORM_AddEditQuestions editQtoDB = new FORM_AddEditQuestions(StackID, QuestionID, Privileges);
            editQtoDB.ShowDialog();
        }

        private void SearchedQuestionsStack_DoubleClick(object sender, MouseEventArgs e)
        {
            EditQuestion("-1", QuestionIDselection, 1);
        }
    }
}
