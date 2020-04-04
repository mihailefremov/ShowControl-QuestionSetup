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
    public partial class FORM_AddEditQuestions : Form
    {
        public int Priviligies = 0;
        public String Level = "0";
        public String Type = "0";
        public String Difficulty = "0";
        public String QuestionID = "-1";
        public String CategoryID = "0";
        public String SubcategoryID = "0";
        public String Stack = "-1";
        public String CorrectAnswer = "0";
        public String Used = "0";

        public MySqlConnection MySqlConnection = new MySqlConnection();
        public MySqlCommand cmd = new MySqlCommand();

        private void FORM_AddEditQuestions_Load(object sender, EventArgs e)
        {
 
        }

        public FORM_AddEditQuestions()
        {
            InitializeComponent();
            ShowAddQuestionTemplateButtons();
        }

        public FORM_AddEditQuestions(string StackID, string QuestionID, int Priviligies)
        {
            InitializeComponent();
            ShowEditQuestionTemplateButtons();
            Stack = StackID;
            this.QuestionID = QuestionID;
            this.Priviligies = Priviligies;

            FillQuestion(GetTableCurrentQuestion(Stack, QuestionID, Priviligies));
            ShowAddQuestionTemplateButtons();
        }
        
        private void ShowEditQuestionTemplateButtons()
        {
            //throw new NotImplementedException();
            SetCategoriesInCombo();

        }

        private void ShowAddQuestionTemplateButtons()
        {
            //throw new NotImplementedException();
            if (Priviligies == 1)
            {
                PreviousQ_button.Visible = false;
                NextQ_button.Visible = false;
            }
            else
            {
                PreviousQ_button.Visible = true;
                NextQ_button.Visible = true;
            }
        }

        public DataTable GetTableCurrentQuestion(string StackID, string QuestionID, int Priviligies)
        {
            {
                MySqlConnection.ConnectionString = DatabasesSettings.Default.questionDBconnectionSQL;
                MySqlDataAdapter SDA = new MySqlDataAdapter();
                DataTable dbDataSet = new DataTable();

                String presetQTable = DatabasesSettings.Default.QuestionDB_setTable1;

                String StackSelection = " ";
                if (Priviligies != 1)
                {
                    StackSelection = " qs.StackID=" + StackID + " and ";
                }
                if (Priviligies == 2)
                {
                    presetQTable = DatabasesSettings.Default.QuestionDB_setTable5;
                }
                if (Priviligies == 3) //Suggest replacement
                {
                    presetQTable = DatabasesSettings.Default.QuestionDB_setTable5;
                }
                
                try
                {
                    MySqlConnection.Open();
                    string Query;
                    Query = ("SELECT * FROM " + DatabasesSettings.Default.QuestionDB_setTable1 + " gq " +
                        "inner join " + presetQTable + " qs " +
                        "on gq.QuestionID=qs.QuestionID " + " where " + StackSelection + " qs.QuestionID="+ QuestionID);
                    MySqlCommand COMM;
                    COMM = new MySqlCommand(Query, MySqlConnection);
                    SDA.SelectCommand = COMM;
                    SDA.Fill(dbDataSet);
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
                    SDA.Dispose();
                    MySqlConnection.Dispose();
                }
            }
        }


        public DataTable GetTableNextQuestion(string StackID, string CurrentLevel, int Priviligies)
        {
            {
                MySqlConnection.ConnectionString = DatabasesSettings.Default.questionDBconnectionSQL;
                MySqlDataAdapter SDA = new MySqlDataAdapter();
                DataTable dbDataSet = new DataTable();

                String presetQTable = DatabasesSettings.Default.QuestionDB_setTable1;
                if (Priviligies == 2)
                {
                    presetQTable = DatabasesSettings.Default.QuestionDB_setTable5;
                }
                if (Priviligies == 3) //Suggest replacement
                {
                    presetQTable = DatabasesSettings.Default.QuestionDB_setTable5;
                }

                try
                {
                    MySqlConnection.Open();
                    string Query;
                    Query = ("SELECT * FROM " + DatabasesSettings.Default.QuestionDB_setTable1 + " gq " +
                        "inner join " + presetQTable + " qs " +
                        "on gq.QuestionID=qs.QuestionID " + "where qs.StackID=" + StackID + " and qs.StackLevel=" +
                        "(select min(StackLevel) from " + DatabasesSettings.Default.QuestionDB_setTable5 + " where StackLevel>" + CurrentLevel + ")");
                    MySqlCommand COMM;
                    COMM = new MySqlCommand(Query, MySqlConnection);
                    SDA.SelectCommand = COMM;
                    SDA.Fill(dbDataSet);
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
                    SDA.Dispose();
                    MySqlConnection.Dispose();
                }
            }
        }

        public DataTable GetTablePreviousQuestion(string StackID, string CurrentLevel, int Priviligies)
        {
            {
                MySqlConnection.ConnectionString = DatabasesSettings.Default.questionDBconnectionSQL;
                MySqlDataAdapter SDA = new MySqlDataAdapter();
                DataTable dbDataSet = new DataTable();

                String presetQTable = DatabasesSettings.Default.QuestionDB_setTable1;
                if (Priviligies == 2)
                {
                    presetQTable = DatabasesSettings.Default.QuestionDB_setTable5;
                }
                if (Priviligies == 3) //Suggest replacement
                {
                    presetQTable = DatabasesSettings.Default.QuestionDB_setTable5;
                }

                try
                {
                    MySqlConnection.Open();
                    string Query;
                    Query = ("SELECT * FROM " + DatabasesSettings.Default.QuestionDB_setTable1 + " gq " +
                        "inner join " + presetQTable + " qs " +
                        "on gq.QuestionID=qs.QuestionID " + "and qs.StackID=" + StackID + " where qs.StackLevel=" +
                        "(select max(StackLevel) from " + DatabasesSettings.Default.QuestionDB_setTable5 + " where StackLevel<" + CurrentLevel + ")");
                    MySqlCommand COMM;
                    COMM = new MySqlCommand(Query, MySqlConnection);
                    SDA.SelectCommand = COMM;
                    SDA.Fill(dbDataSet);
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
                    SDA.Dispose();
                    MySqlConnection.Dispose();
                }
            }
        }

        public void FillQuestion(DataTable dbDataSet)
        {
            if (dbDataSet.Rows.Count > 0) {
                ResetCheckboxes(); ResetTextboxes(); ResetQuestion();
                QuestionID = Convert.ToString(dbDataSet.Rows[0]["QuestionID"]);
                if (Priviligies != 1)
                {
                    Level = Convert.ToString(dbDataSet.Rows[0]["StackLevel"]);
                }
                Difficulty = Convert.ToString(dbDataSet.Rows[0]["Difficulty"]);
                Type = Convert.ToString(dbDataSet.Rows[0]["Type"]);
                CategoryID = Convert.ToString(dbDataSet.Rows[0]["CategoryID"]);
                SubcategoryID = Convert.ToString(dbDataSet.Rows[0]["SubcategoryID"]);
                QuestionText_textbox.Text = Convert.ToString(dbDataSet.Rows[0]["Question"]);
                Answer1_textbox.Text = Convert.ToString(dbDataSet.Rows[0]["Answer1"]);
                Answer2_textbox.Text = Convert.ToString(dbDataSet.Rows[0]["Answer2"]);
                Answer3_textbox.Text = Convert.ToString(dbDataSet.Rows[0]["Answer3"]);
                Answer4_textbox.Text = Convert.ToString(dbDataSet.Rows[0]["Answer4"]);
                CorrectAnswer = Convert.ToString(dbDataSet.Rows[0]["CorrectAnswer"]);
                Pronunciation_textBox.Text = Convert.ToString(dbDataSet.Rows[0]["Pronunciation"]);
                ExplanationQ_textBox.Text = Convert.ToString(dbDataSet.Rows[0]["MoreInformation"]);
                Used = Convert.ToString(dbDataSet.Rows[0]["TimesAnswered"]);
                QuestionReferenceID_Textbox.Text = QuestionID;
                Difficulty_comboBox.Text = Difficulty;
                Level_comboBox.Text = Level;

                Category_comboBox.SelectedValue = CategoryID;

                if (Type == "1")
                {
                    if (CorrectAnswer == "1") { Answer1correct_checkBox.CheckState = CheckState.Checked; }
                    if (CorrectAnswer == "2") { Answer2correct_checkBox.CheckState = CheckState.Checked; }
                    if (CorrectAnswer == "3") { Answer3correct_checkBox.CheckState = CheckState.Checked; }
                    if (CorrectAnswer == "4") { Answer4correct_checkBox.CheckState = CheckState.Checked; }
                    CorrectOrder_textBox.Enabled = false;
                } else if (Type == "2")
                {
                    CorrectOrder_textBox.Text = CorrectAnswer;
                    CorrectOrder_textBox.Enabled = true;
                }

                if (Used == "0")
                {
                    QuestionUsage_checkBox.CheckState = CheckState.Unchecked;
                }
                else
                {
                    QuestionUsage_checkBox.CheckState = CheckState.Checked;
                }


            }
        }

        private void PreviousQ_button_Click(object sender, EventArgs e)
        {
            FillQuestion(GetTablePreviousQuestion(Stack, Level, Priviligies));
        }

        private void NextQ_button_Click(object sender, EventArgs e)
        {
            FillQuestion(GetTableNextQuestion(Stack, Level, Priviligies));
        }

        private void AnswerStateChange(object sender, EventArgs e)
        {
            ResetTextboxes();

            if (Type == "1")
            {
                CorrectAnswer = "";
                if (Answer1correct_checkBox.CheckState == CheckState.Checked)
                {
                    Answer1_textbox.BackColor = Color.PaleGreen; Answer1_textbox.ForeColor = Color.DarkGreen;
                    CorrectAnswer = "1";
                }
                else if (Answer2correct_checkBox.CheckState == CheckState.Checked)
                {
                    Answer2_textbox.BackColor = Color.PaleGreen; Answer2_textbox.ForeColor = Color.DarkGreen;
                    CorrectAnswer = "2";
                }
                else if (Answer3correct_checkBox.CheckState == CheckState.Checked)
                {
                    Answer3_textbox.BackColor = Color.PaleGreen; Answer3_textbox.ForeColor = Color.DarkGreen;
                    CorrectAnswer = "3";
                }
                else if (Answer4correct_checkBox.CheckState == CheckState.Checked)
                {
                    Answer4_textbox.BackColor = Color.PaleGreen; Answer4_textbox.ForeColor = Color.DarkGreen;
                    CorrectAnswer = "4";
                }
                CorrectOrder_textBox.Enabled = false;
            }
            else if (Type == "2")
            {
                CorrectOrder_textBox.Enabled = true;
            }

        }

        public void ResetCheckboxes()
        {
            Answer1correct_checkBox.CheckState = CheckState.Unchecked;  Answer2correct_checkBox.CheckState = CheckState.Unchecked;
            Answer3correct_checkBox.CheckState = CheckState.Unchecked;  Answer4correct_checkBox.CheckState = CheckState.Unchecked;
            Type = "";
        }

        public void ResetQuestion()
        {

        }

        public void ResetTextboxes()
        {
            Answer1_textbox.ForeColor = Color.Black;
            Answer2_textbox.ForeColor = Color.Black;
            Answer3_textbox.ForeColor = Color.Black;
            Answer4_textbox.ForeColor = Color.Black;
            Answer1_textbox.BackColor = Color.White;
            Answer2_textbox.BackColor = Color.White;
            Answer3_textbox.BackColor = Color.White;
            Answer4_textbox.BackColor = Color.White;
        }

        private void QuestionAnswerEdit_TextChanged(object sender, EventArgs e)
        {
            QuestionText_button.Text = QuestionText_textbox.Text.Replace("|", "\r\n");
            Answer1_button.Text = Answer1_textbox.Text;
            Answer2_button.Text = Answer2_textbox.Text;
            Answer3_button.Text = Answer3_textbox.Text;
            Answer4_button.Text = Answer4_textbox.Text;
        }

        public void SetCategoriesInCombo()
        {
            QuestionStackManipulation sc = new QuestionStackManipulation();
            Category_comboBox.DataSource = new BindingSource(sc.CategoryFromDB(), null);
            Category_comboBox.DisplayMember = "Value";
            Category_comboBox.ValueMember = "Key";
        }

        private void CategoryChange_Selection(object sender, EventArgs e)
        {
            QuestionStackManipulation sc = new QuestionStackManipulation();

            CategoryID = ((KeyValuePair<string, string>)Category_comboBox.SelectedItem).Key;

            Subcategory_Fill();
        }
        
        private void OKedit_Button_Click(object sender, EventArgs e)
        {
            UpdateQuestionInDB();
        }

        public void UpdateQuestionInDB()
        {
            MySqlConnection.ConnectionString = DatabasesSettings.Default.showtimeDBconnectionSQL;
            try
            {
                MySqlConnection.Open();
                String gameqTable = DatabasesSettings.Default.QuestionDB_setTable1;
                cmd = MySqlConnection.CreateCommand();
                cmd.CommandText = "insert into " + gameqTable
                + "(QuestionID,Difficulty,Type,Question,Answer1,Answer2,Answer3,Answer4,CorrectAnswer,CategoryID,SubcategoryID,MoreInformation,Pronunciation,Comments,TimesAnswered) "
                + " values(@QuestionID,@Difficulty,@Type,@Question,@Answer1,@Answer2,@Answer3,@Answer4,@CorrectAnswer,@CategoryID,@SubcategoryID,@MoreInformation,@Pronunciation,@Comments,@TimesAnswered) "
                + " on duplicate key update QuestionID=@QuestionID,Difficulty=@Difficulty,Type=@Type,Question=@Question,Answer1=@Answer1,Answer2=@Answer2,Answer3=@Answer3,Answer4=@Answer4,CorrectAnswer=@CorrectAnswer,CategoryID=@CategoryID,SubcategoryID=@SubcategoryID,MoreInformation=@MoreInformation,Pronunciation=@Pronunciation,Comments=@Comments,TimesAnswered=@TimesAnswered";
                cmd.Parameters.AddWithValue("@QuestionID", QuestionID);
                cmd.Parameters.AddWithValue("@Difficulty", Difficulty);
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@Question", QuestionText_textbox.Text);
                cmd.Parameters.AddWithValue("@Answer1", Answer1_textbox.Text);
                cmd.Parameters.AddWithValue("@Answer2", Answer2_textbox.Text);
                cmd.Parameters.AddWithValue("@Answer3", Answer3_textbox.Text);
                cmd.Parameters.AddWithValue("@Answer4", Answer4_textbox.Text);
                cmd.Parameters.AddWithValue("@CorrectAnswer", CorrectAnswer);
                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                cmd.Parameters.AddWithValue("@SubcategoryID", Subcategory_comboBox.SelectedIndex);
                cmd.Parameters.AddWithValue("@MoreInformation", ExplanationQ_textBox.Text);
                cmd.Parameters.AddWithValue("@Pronunciation", Pronunciation_textBox.Text);
                cmd.Parameters.AddWithValue("@Comments", References_textBox.Text);
                cmd.Parameters.AddWithValue("@TimesAnswered", Used);
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

        private void Difficulty_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Difficulty = Difficulty_comboBox.Text;
        }

        private void QuestionUsage_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (QuestionUsage_checkBox.Checked == true)
            {
                Used = "1";
            }
            else { Used = "0"; }

        }

        private void Category_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CategoryID = Category_comboBox.SelectedIndex.ToString();
            Subcategory_Fill();

            Subcategory_comboBox.SelectedValue = SubcategoryID;
        }

        public void Subcategory_Fill()
        {
            QuestionStackManipulation sc = new QuestionStackManipulation();
            Subcategory_comboBox.DataSource = new BindingSource(sc.SubcategoryFromDB(CategoryID), null);
            Subcategory_comboBox.DisplayMember = "Value";
            Subcategory_comboBox.ValueMember = "Key";
        }

        private void Subcategory_DropDown(object sender, EventArgs e)
        {
            Subcategory_Fill();
            SubcategoryID = Subcategory_comboBox.SelectedIndex.ToString();
        }

        private void CorrectOrder_textBox_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
