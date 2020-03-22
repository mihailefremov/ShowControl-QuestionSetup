using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuestionEditAndPreset
{
    public partial class FORM_DatabasesConfiguration : Form
    {

        public FORM_DatabasesConfiguration()
        {
            InitializeComponent();
        }

        private void DatabasesConfiguration_Load(object sender, EventArgs e)
        {
            QuestionDBhost_textBox.Text = DatabasesSettings.Default.QuestionDB_setHost;
            QuestionDBuser_textBox.Text = DatabasesSettings.Default.QuestionDB_setUser;
            QuestionDBdatabaseName_textBox.Text = DatabasesSettings.Default.QuestionDB_setDatabase;
            QuestionDBpassword_textBox.Text = DatabasesSettings.Default.QuestionDB_setPassword;

            QuestionDBtable1_textBox.Text = DatabasesSettings.Default.QuestionDB_setTable1;
            QuestionDBtable2_textBox.Text = DatabasesSettings.Default.QuestionDB_setTable2;
            QuestionDBtable3_textBox.Text = DatabasesSettings.Default.QuestionDB_setTable3;
            QuestionDBtable4_textBox.Text = DatabasesSettings.Default.QuestionDB_setTable4;
            QuestionDBtable5_textBox.Text = DatabasesSettings.Default.QuestionDB_setTable5;

            ShowtimeDBhost_textBox.Text = DatabasesSettings.Default.ShowtimeDB_setHost;
            ShowtimeDBuser_textBox.Text = DatabasesSettings.Default.ShowtimeDB_setUser;
            ShowtimeDBdatabaseName_textBox.Text = DatabasesSettings.Default.ShowtimeDB_setDatabase;
            ShowtimeDBpassword_textBox.Text = DatabasesSettings.Default.ShowtimeDB_setPassword;

            ShowtimeDBtable1_textBox.Text = DatabasesSettings.Default.ShowtimeDB_setTable1;

            questionDBconnectionStringForming();
            showTimeDBconnectionStringforming();
        }

        private void QuestionDBconnect_Button_Click(object sender, EventArgs e)
        {
            setFromTextboxQuestionDB();
            questionDBconnectionStringForming();
        }

        private void StackDBconnect_Button_Click(object sender, EventArgs e)
        {
 
        }

        private void ShowtimeDBconnect_Button_Click(object sender, EventArgs e)
        {
            setFromTextboxShowTimeDB();
            showTimeDBconnectionStringforming();
        }

        public void questionDBconnectionStringForming()
        {
            DatabasesSettings.Default.Reset();
            DatabasesSettings.Default.questionDBconnectionSQL = "Server=" + DatabasesSettings.Default.QuestionDB_setHost +
            ";Uid=" + DatabasesSettings.Default.QuestionDB_setUser +
            ";Password=" + DatabasesSettings.Default.QuestionDB_setPassword +
            ";Database=" + DatabasesSettings.Default.QuestionDB_setDatabase +
            ";Character Set=utf8;";
            //MessageBox.Show(DatabasesSettings.Default.questionDBconnectionSQL);
        }
 

        public void showTimeDBconnectionStringforming()
        {
            DatabasesSettings.Default.showtimeDBconnectionSQL = "Server=" + DatabasesSettings.Default.ShowtimeDB_setHost +
            ";Uid=" + DatabasesSettings.Default.ShowtimeDB_setUser +
            ";Password=" + DatabasesSettings.Default.ShowtimeDB_setPassword +
            ";Database=" + DatabasesSettings.Default.ShowtimeDB_setDatabase +
            ";Character Set=utf8;";
            //MessageBox.Show(DatabasesSettings.Default.showtimeDBconnectionSQL);
        }

        public void setFromTextboxQuestionDB()
        {
            DatabasesSettings.Default.QuestionDB_setHost = QuestionDBhost_textBox.Text;
            //
            DatabasesSettings.Default.QuestionDB_setDatabase = QuestionDBdatabaseName_textBox.Text;
            DatabasesSettings.Default.QuestionDB_setPassword = QuestionDBpassword_textBox.Text;

            DatabasesSettings.Default.QuestionDB_setTable1 = QuestionDBtable1_textBox.Text;
            DatabasesSettings.Default.QuestionDB_setTable2 = QuestionDBtable2_textBox.Text;
            DatabasesSettings.Default.QuestionDB_setTable3 = QuestionDBtable3_textBox.Text;
            DatabasesSettings.Default.QuestionDB_setTable4 = QuestionDBtable4_textBox.Text;
            DatabasesSettings.Default.QuestionDB_setTable5 = QuestionDBtable5_textBox.Text;

            DatabasesSettings.Default.Save();
        }

        
        public void setFromTextboxShowTimeDB()
        {
            DatabasesSettings.Default.ShowtimeDB_setHost = ShowtimeDBhost_textBox.Text;
            DatabasesSettings.Default.ShowtimeDB_setDatabase = ShowtimeDBdatabaseName_textBox.Text;
            DatabasesSettings.Default.ShowtimeDB_setPassword = ShowtimeDBpassword_textBox.Text;

            DatabasesSettings.Default.ShowtimeDB_setTable1 = ShowtimeDBtable1_textBox.Text;

            DatabasesSettings.Default.Save();
        }

        public void setDefaultQuestionDB()
        {
            QuestionDBhost_textBox.Text = DatabasesSettings.Default.QuestionDB_defaultHost;
            QuestionDBdatabaseName_textBox.Text = DatabasesSettings.Default.QuestionDB_defaultDatabase;
            QuestionDBpassword_textBox.Text = DatabasesSettings.Default.QuestionDB_defaultPassword;

            QuestionDBtable1_textBox.Text = DatabasesSettings.Default.QuestionDB_defaultTable1;
            QuestionDBtable2_textBox.Text = DatabasesSettings.Default.QuestionDB_defaultTable2;
            QuestionDBtable3_textBox.Text = DatabasesSettings.Default.QuestionDB_defaultTable3;
            QuestionDBtable4_textBox.Text = DatabasesSettings.Default.QuestionDB_defaultTable4;
            QuestionDBtable5_textBox.Text = DatabasesSettings.Default.QuestionDB_defaultTable5;
        }
 
        public void setDefaultShowTimeDB()
        {
            ShowtimeDBhost_textBox.Text = DatabasesSettings.Default.ShowtimeDB_defaultHost;
            ShowtimeDBdatabaseName_textBox.Text = DatabasesSettings.Default.ShowtimeDB_defaultDatabase;
            ShowtimeDBpassword_textBox.Text = DatabasesSettings.Default.ShowtimeDB_defaultPassword;

            ShowtimeDBtable1_textBox.Text = DatabasesSettings.Default.ShowtimeDB_defaultTable1;
        }

        private void QuestionDBdefault_Label_Click(object sender, EventArgs e)
        {
            setDefaultQuestionDB();
            setFromTextboxQuestionDB();
        }

        private void StackDBdefault_Label_Click(object sender, EventArgs e)
        {
 
        }

        private void ShowtimeDBdefault_Label_Click(object sender, EventArgs e)
        {
            setDefaultShowTimeDB();
            setFromTextboxShowTimeDB();
        }
    }
}
