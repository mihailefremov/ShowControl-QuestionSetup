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
    public partial class FORM_SearchQuestions : Form
    {
        string Stack = "-1";
        string Type = "0";
        string LevelGetOfQuestion = "0";
        string LevelGetOfCombobox = "0";
        string CategoryID = "0";
        string SubcategoryID = "0";

        //bool SameCategory = true;
        //bool SameSubcategory = false;
        //bool AnyCategory = false;

        string QuestionIDselection = "-1";

        string LevelSelect = "";
        string ReferenceSelect = "";
        string CategSelect = "";
        string SearchQTextSelect = "";
        string SearchAnsTextSelect = "";

        public MySqlConnection MySqlConnection = new MySqlConnection();

        public MySqlConnection _PropertyQaDBConnection;
        public MySqlConnection PropQuestionDbMySqlConnection
        {
            get
            {
                if (_PropertyQaDBConnection == null)
                {
                    _PropertyQaDBConnection = new MySqlConnection();
                    _PropertyQaDBConnection.ConnectionString = DatabasesSettings.Default.questionDBconnectionSQL;
                    _PropertyQaDBConnection.Open();
                    return _PropertyQaDBConnection;
                }
                else if (_PropertyQaDBConnection.State == ConnectionState.Open)
                {
                    return _PropertyQaDBConnection;
                } else if (_PropertyQaDBConnection.State == ConnectionState.Closed || _PropertyQaDBConnection.State == ConnectionState.Broken)
                {
                    _PropertyQaDBConnection = new MySqlConnection();
                    _PropertyQaDBConnection.ConnectionString = DatabasesSettings.Default.questionDBconnectionSQL;
                    _PropertyQaDBConnection.Open();
                    return _PropertyQaDBConnection;
                }
                else
                {
                    return _PropertyQaDBConnection;
                }
            }
        }

        public MySqlCommand cmd = new MySqlCommand();

        public FORM_SearchQuestions()
        {
            InitializeComponent();
        }

        public FORM_SearchQuestions(string StackID, string Type, string Level)
        {
            Stack = StackID;
            this.Type = Type;
            this.LevelGetOfQuestion = Level;
            //Category = CategoryID;
            //Subcategory = SubcategoryID;

            InitializeComponent();
        }

        private void FORM_SearchQuestions_Load(object sender, EventArgs e)
        {
            QuestionLevel_comboBox.Text = this.LevelGetOfQuestion;
            SetCategoriesInCombo();
            //FillSearchedQuestions(GetSearchedQuestions(Type));
        }

        public void SetCategoriesInCombo()
        {
            QuestionStackManipulation sc = new QuestionStackManipulation();
            QCategory_comboBox.DataSource = new BindingSource(sc.CategoryFromDB(), null);
            QCategory_comboBox.DisplayMember = "Value";
            QCategory_comboBox.ValueMember = "Key";
        }

        private void CategoryChange_Selection(object sender, EventArgs e)
        {
            QuestionStackManipulation sc = new QuestionStackManipulation();
            CategoryID = ((KeyValuePair<string, string>)QCategory_comboBox.SelectedItem).Key;
            Subcategory_Fill();

            if (CategoryID == "0" || CategoryID == "")
            {
                CategSelect = "";
            }
            else
            {
                CategSelect = " and (CategoryID=" + CategoryID + " or AdditionalCategoryID=" + CategoryID + ") ";
            }
        }

        private void SubcategoryChange_Selection(object sender, EventArgs e)
        {
            SubcategoryID = QSubcategory_comboBox.SelectedIndex.ToString();
            if (SubcategoryID == "0" || SubcategoryID == "")
            {
                CategSelect = " and (CategoryID=" + CategoryID + " or AdditionalCategoryID=" + CategoryID + ") ";
            }
            else
            {
                CategSelect = " and (CategoryID=" + CategoryID + " or AdditionalCategoryID=" + CategoryID + ") " + " and (SubcategoryID=" + SubcategoryID + " or AdditionalSubcategoryID=" + SubcategoryID + ") ";
            }
        }

        private void Category_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CategoryID = QCategory_comboBox.SelectedIndex.ToString();
            Subcategory_Fill();
            QSubcategory_comboBox.SelectedValue = SubcategoryID;
        }

        public void Subcategory_Fill()
        {
            QuestionStackManipulation sc = new QuestionStackManipulation();
            QSubcategory_comboBox.DataSource = new BindingSource(sc.SubcategoryFromDB(CategoryID), null);
            QSubcategory_comboBox.DisplayMember = "Value";
            QSubcategory_comboBox.ValueMember = "Key";
        }

        private void Subcategory_DropDown(object sender, EventArgs e)
        {
            Subcategory_Fill();
            SubcategoryID = QSubcategory_comboBox.SelectedIndex.ToString();
        }

        public void FillSearchedQuestions(DataTable dbDataSet)
        {
            SearchedQuestionsPreview_dataGridView.DataSource = null;
            if (dbDataSet.Rows.Count > 0)
            {
                SearchedQuestionsPreview_dataGridView.DataSource = dbDataSet;

                SearchedQuestionsPreview_dataGridView.Columns[0].FillWeight = 10;
                SearchedQuestionsPreview_dataGridView.Columns[1].FillWeight = 100;
                SearchedQuestionsPreview_dataGridView.Columns[2].FillWeight = 14;

                SearchedQuestionsPreview_dataGridView.Columns[0].Width = 40;
                SearchedQuestionsPreview_dataGridView.Columns[1].Width = 281;
                SearchedQuestionsPreview_dataGridView.Columns[2].Width = 50;
            }
        }

        //public DataTable GetSearchedQuestions(string Type)
        //{
        //    MySqlDataAdapter rda = new MySqlDataAdapter();
        //    DataTable dbDataSet = new DataTable();
           
        //    String GameQdatabase = "";
        //    if (Type == "1" || Type == "2")
        //    {
        //        GameQdatabase = DatabasesSettings.Default.QuestionDB_setTable1;
        //    }

        //    MySqlConnection.ConnectionString = DatabasesSettings.Default.questionDBconnectionSQL;

        //    try
        //    {
        //        MySqlConnection.Open();
        //        cmd = MySqlConnection.CreateCommand();
        //        cmd.CommandText = "Select QuestionID as ID, Question, TimesAnswered as Used from " + GameQdatabase
        //            + " where 1 " + LevelSelect
        //            + " " + CategSelect + " " + ReferenceSelect + SearchQTextSelect + SearchAnsTextSelect
        //            + " and TimesAnswered=" + AdvancedGameConfigSettings.Default.UsageSelectionQuery + " and Type=" + Type
        //            + " and QuestionID not in (Select QuestionID from questionstackitems) "
        //            + " ";
        //        label3.Text = cmd.CommandText;
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

        public DataTable GetSearchedQuestions(string Type)
        {
            MySqlDataAdapter rda = new MySqlDataAdapter();
            DataTable dbDataSet = new DataTable();

            String GameQdatabase = "";
            if (Type == "1" || Type == "2")
            {
                GameQdatabase = DatabasesSettings.Default.QuestionDB_setTable1;
            }

            try
            {
    
                cmd = PropQuestionDbMySqlConnection.CreateCommand();
                cmd.CommandText = "Select QuestionID as ID, Question, TimesAnswered as Used from " + GameQdatabase
                    + " where 1 " + LevelSelect
                    + " " + CategSelect + " " + ReferenceSelect + SearchQTextSelect + SearchAnsTextSelect
                    + " and TimesAnswered=" + AdvancedGameConfigSettings.Default.UsageSelectionQuery + " and Type=" + Type
                    + " and QuestionID not in (Select QuestionID from questionstackitems) "
                    + " ";
                label3.Text = cmd.CommandText;
                rda.SelectCommand = cmd;
                rda.Fill(dbDataSet);
                //PropertyQuestionDbSqlConnection.Close();
                return dbDataSet;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
            //    PropertyQuestionDbSqlConnection.Close();
            //    PropertyQuestionDbSqlConnection.Dispose();
            }

        }

        private void QLevel_ComboChangeEvent(object sender, EventArgs e)
        {
            LevelSelect = "";
            LevelGetOfCombobox = QuestionLevel_comboBox.Text;
            if (!String.IsNullOrEmpty(LevelGetOfCombobox) || !String.IsNullOrWhiteSpace(LevelGetOfCombobox))
            {
                LevelSelect = " and Difficulty=" + MapLevelDifficultyGameSettings.Default["Level" + LevelGetOfCombobox].ToString();
                ReferenceSelect = "";
            }
        }

        private void SearchQuestions_Button_Click(object sender, EventArgs e)
        {
            FillSearchedQuestions(GetSearchedQuestions(Type));
        }

        private void ContainingText_textBox_TextChanged(object sender, EventArgs e)
        {
            ContainingTextEvent();
        }

        private void QASearch_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            ContainingTextEvent();
        }

        public void ContainingTextEvent()
        {
            string matchTextbox = ContainingText_textBox.Text;
            matchTextbox.ToLower();
            QuestionReferenceID_Textbox.Text = "";

            if (InQuestionSearch_checkBox.Checked == true)
            {
                SearchQTextSelect = "";
                if (!String.IsNullOrEmpty(matchTextbox))
                {
                    SearchQTextSelect = " and LOWER(Question) like '%" + matchTextbox + "%' ";
                }
            }

            if (InAnswerSearch_checkBox.Checked == true)
            {
                SearchAnsTextSelect = "";
                if (!String.IsNullOrEmpty(matchTextbox))
                {
                    SearchAnsTextSelect = " and (LOWER(Answer1) like '%" + matchTextbox + "%' " +
                        " or LOWER(Answer2) like '%" + matchTextbox + "%' " +
                        " or LOWER(Answer3) like '%" + matchTextbox + "%' " +
                        " or LOWER(Answer4) like '%" + matchTextbox + "%') ";
                }
            }
        }

        private void QuestionReferenceID_Textbox_TextChanged(object sender, EventArgs e)
        {
            ReferenceSelect = "";
            if (!String.IsNullOrEmpty(QuestionReferenceID_Textbox.Text))
            {
                ReferenceSelect = " and QuestionID=" + QuestionReferenceID_Textbox.Text + " ";
                LevelSelect = "";
            }
            ContainingText_textBox.Text = "";
        }

        public void SearchedQuestionsPreview_SelectionChanged(object sender, EventArgs e)
        {
            //Level = Convert.ToString(SugestedQuestionsPreview_dataGridView.CurrentRow.Cells["Level"].Value);
            QuestionIDselection = Convert.ToString(SearchedQuestionsPreview_dataGridView.CurrentRow.Cells["ID"].Value);

        }

        //public void InsertSearchedQuestionInStack()
        //{
        //    MySqlConnection.ConnectionString = DatabasesSettings.Default.showtimeDBconnectionSQL;
        //    string gameqTable = "";
        //    if (Type == "1" || Type == "2")
        //    {
        //        gameqTable = DatabasesSettings.Default.QuestionDB_setTable1;
        //    }
        //    try
        //    {
        //        MySqlConnection.Open();
        //        String stackItemsTable = DatabasesSettings.Default.QuestionDB_setTable5;

        //        cmd = MySqlConnection.CreateCommand();
        //        cmd.CommandText = "insert into " + stackItemsTable +
        //        "(StackID,StackLevel,QuestionID) " + " values(@StackID,@StackLevel,@QuestionID) " +
        //        " on duplicate key update QuestionID = @QuestionID, StackLevel=@StackLevel, QuestionID=@QuestionID";
        //        cmd.Parameters.AddWithValue("@StackID", Stack);
        //        cmd.Parameters.AddWithValue("@StackLevel", LevelGetOfQuestion);
        //        cmd.Parameters.AddWithValue("@QuestionID", QuestionIDselection);
        //        cmd.ExecuteNonQuery();
        //        MySqlConnection.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        MySqlConnection.Dispose();
        //    }
        //}

        public void InsertSearchedQuestionInStack()
        {
            string gameqTable = "";
            if (Type == "1" || Type == "2")
            {
                gameqTable = DatabasesSettings.Default.QuestionDB_setTable1;
            }
            try
            {
                String stackItemsTable = DatabasesSettings.Default.QuestionDB_setTable5;

                cmd = PropQuestionDbMySqlConnection.CreateCommand();
                cmd.CommandText = "insert into " + stackItemsTable +
                "(StackID,StackLevel,QuestionID) " + " values(@StackID,@StackLevel,@QuestionID) " +
                " on duplicate key update QuestionID = @QuestionID, StackLevel=@StackLevel, QuestionID=@QuestionID";
                cmd.Parameters.AddWithValue("@StackID", Stack);
                cmd.Parameters.AddWithValue("@StackLevel", LevelGetOfQuestion);
                cmd.Parameters.AddWithValue("@QuestionID", QuestionIDselection);
                cmd.ExecuteNonQuery();
                //PropertyQuestionDbSqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //PropertyQuestionDbSqlConnection.Dispose();
            }
        }

        private void MakeReplacement_Button_Click(object sender, EventArgs e)
        {
            InsertSearchedQuestionInStack();
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
