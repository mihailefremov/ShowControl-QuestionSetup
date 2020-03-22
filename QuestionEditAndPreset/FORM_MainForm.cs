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
    public partial class MainControlForm : Form
    {
        public string QuestionIDfromGameQSelected;
        public string StackTypeChosen = "-1";
        public string StackNameChosen;
        public string StackIDselected = "-1";
        public string StackNameselected = "";

        public string LevelSelectedFromStack = "-1";
        public string QuestionIDfromStackSelected="-1";
        public string CategorySelected = "-1";
        public string SubcategorySelected = "-1";

        public string LiveStackNameselected = "";
        public string LevelSelectedFromLIVEStack = "-1";
        public string QuestionIDfromLIVEStackSelected = "-1";
        public string CategorySelectedFromLIVEStack = "-1";
        public string SubcategorySelectedFromLIVEStack = "-1";

        QuestionStackManipulation QAStack = new QuestionStackManipulation();

        public MainControlForm()
        {
            InitializeComponent();
        }

        public void MainControlForm_Load(object sender, EventArgs e)
        {
            //textBox2.Text=DatabasesSettings.Default.questionDBconnectionSQL;
            //MessageBox.Show(AdvancedGameConfigSettings.Default.CategorySelectionQuery);
        }

        private void Homepage_Button_Click(object sender, EventArgs e)
        {
            MainTabControl.SelectedTab = Homepage_tabPage;
        }

        private void ImportExport_Button_Click(object sender, EventArgs e)
        {
            MainTabControl.SelectedTab = ImportExport_tabPage;
        }

        private void QuestionsManip_Button_Click(object sender, EventArgs e)
        {
            MainTabControl.SelectedTab = Questions_tabPage;
        }

        private void StacksManip_Button_Click(object sender, EventArgs e)
        {
            MainTabControl.SelectedTab = Stacks_tabPage;
        }

        private void ShowTime_Button_Click(object sender, EventArgs e)
        {
            MainTabControl.SelectedTab = Showtime_tabPage;
        }

        private void GameAdvancedConfiguration_Button_Click(object sender, EventArgs e)
        {
            //String p = MapLevelDifficultyGameSettings.Default[""].ToString();
            FORM_AdvancedConfigurations fmAdvConfing = new FORM_AdvancedConfigurations();
            fmAdvConfing.ShowDialog();
        }

        private void DatabasesConnectionConfiguration_Button_Click(object sender, EventArgs e)
        {
            FORM_DatabasesConfiguration dbConfig = new FORM_DatabasesConfiguration();
            dbConfig.ShowDialog();
        }

        private void AddQuestionToDatabase_toolStripButton_Click(object sender, EventArgs e)
        {
            FORM_AddEditQuestions addQtoDB = new FORM_AddEditQuestions(StackIDselected, QuestionIDfromGameQSelected, 0);
            addQtoDB.ShowDialog();
        }

        private void DeleteQuestionFromDatabase_toolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void EditSelectedQuestionFromDatabase_toolStripButton_Click(object sender, EventArgs e)
        {
            EditQuestion("", QuestionIDfromGameQSelected, 1);
        }

        private void SearchQuestions_toolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void PrintSelectedQuestionsFromDatabase_toolStripButton_Click(object sender, EventArgs e)
        {

        }

        public void RefreshAllQuestions_toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshAllQuestionsTable();
            EnableDisableOperationQuestionsToolstripButtons();
        }

        public void RefreshAllQuestionsTable()
        {
            EnableDisableOperationQuestionsToolstripButtons();
            ALLquestionsPreview_dataGridView.DataSource = QAStack.RefreshAllQuestionsTable("", "", "", "", "", "", "");
            if (ALLquestionsPreview_dataGridView.Rows.Count > 0)
            {
                ALLquestionsPreview_dataGridView.Columns[0].FillWeight = 15;
                ALLquestionsPreview_dataGridView.Columns[1].FillWeight = 15;
                ALLquestionsPreview_dataGridView.Columns[2].FillWeight = 120;
                ALLquestionsPreview_dataGridView.Columns[3].FillWeight = 65;
                ALLquestionsPreview_dataGridView.Columns[4].FillWeight = 65;
                ALLquestionsPreview_dataGridView.Columns[5].FillWeight = 65;
                ALLquestionsPreview_dataGridView.Columns[6].FillWeight = 65;
                ALLquestionsPreview_dataGridView.Columns[7].FillWeight = 22;
                ALLquestionsPreview_dataGridView.Columns[8].FillWeight = 22;
                //MarkRedOrangeNotSetUsedQA(ALLquestionsPreview_dataGridView);
            }
         }

        private void ImportQuestionsToDatabase_Button_Click(object sender, EventArgs e)
        {
            ImportExportData.SetFile();
        }

        private void SearchOptions_Label_Click(object sender, EventArgs e)
        {

        }


        private void RefreshQuery_Button_Click(object sender, EventArgs e)
        {

        }

        public void EnableDisableOperationQuestionsToolstripButtons()
        {
            AddQuestionToDatabase_toolStripButton.Enabled = false;
            DeleteQuestionFromDatabase_toolStripButton.Enabled = false;
            EditSelectedQuestionFromDatabase_toolStripButton.Enabled = false;
            PrintSelectedQuestionsFromDatabase_toolStripButton.Enabled = false;
            SearchQuestions_toolStripButton.Enabled = false;
            QuestionUsage_groupBox.Enabled = false;
            QuestionStacked_groupBox.Enabled = false;
            QuestionCategory_groupBox.Enabled = false;
            InQuestionSearch_checkBox.Enabled = false;
            InAnswerSearch_checkBox.Enabled = false;
            QuestionReferenceID_Textbox.Enabled = false;
            QuestionLevel_comboBox.Enabled = false;
            ContainingTextInQA_textBox.Enabled = false;
            LastFirstAdded_textBox.Enabled = false;
            if (QAStack.CanConnectStacks)
            {
                AddQuestionToDatabase_toolStripButton.Enabled = true;
                DeleteQuestionFromDatabase_toolStripButton.Enabled = true;
                EditSelectedQuestionFromDatabase_toolStripButton.Enabled = true;
                PrintSelectedQuestionsFromDatabase_toolStripButton.Enabled = true;
                SearchQuestions_toolStripButton.Enabled = true;
                QuestionUsage_groupBox.Enabled = true;
                QuestionStacked_groupBox.Enabled = true;
                QuestionCategory_groupBox.Enabled = true;
                InQuestionSearch_checkBox.Enabled = true;
                InAnswerSearch_checkBox.Enabled = true;
                QuestionReferenceID_Textbox.Enabled = true;
                QuestionLevel_comboBox.Enabled = true;
                ContainingTextInQA_textBox.Enabled = true;
                LastFirstAdded_textBox.Enabled = true;
            }
        }

        public void RefreshStacks()
        {         
            StacksOfQuestionPreview_dataGridView.ClearSelection();
            StacksOfQuestionPreview_dataGridView.DataSource = QAStack.RefreshStackList();
            if (StacksOfQuestionPreview_dataGridView.DataSource != null)
            {
                StacksOfQuestionPreview_dataGridView.Columns[0].Visible = false;
                StacksOfQuestionPreview_dataGridView.Columns[1].FillWeight = 40;
                //StacksOfQuestionPreview_dataGridView.Columns[1].Width = 110;
                StacksOfQuestionPreview_dataGridView.Columns[2].FillWeight = 30;
                //StacksOfQuestionPreview_dataGridView.Columns[2].Width = 45;
            }
            QuestionLIVEstack_Textbox.Text = StackSettings.Default.LiveStackNameselected;
        }

        public void RefreshQAinStack(DataGridView QuestionsFromStack_dataGridView, string StackIDselected)
        {
            QuestionsFromStack_dataGridView.DataSource = QAStack.RefreshStackQuestions(StackIDselected);
            if (QuestionsFromStack_dataGridView.Rows.Count>0)
            {
                QuestionsFromStack_dataGridView.Columns[1].Visible = false;
                QuestionsFromStack_dataGridView.Columns[2].Visible = false;
                QuestionsFromStack_dataGridView.Columns[3].Visible = false;
                QuestionsFromStack_dataGridView.Columns[4].Visible = false;

                QuestionsFromStack_dataGridView.Columns[0].FillWeight = 10;
                QuestionsFromStack_dataGridView.Columns[5].FillWeight = 15;
                QuestionsFromStack_dataGridView.Columns[6].FillWeight = 100;
                QuestionsFromStack_dataGridView.Columns[7].FillWeight = 14;

                //QuestionsFromStack_dataGridView.Columns[0].Width = 40;
                //QuestionsFromStack_dataGridView.Columns[5].Width = 54;
                //QuestionsFromStack_dataGridView.Columns[6].Width = 281;
                //QuestionsFromStack_dataGridView.Columns[7].Width = 50;
                //System.Threading.Thread.Sleep(2000);
                //MessageBox.Show("");
                MarkRedOrangeNotSetUsedQA(QuestionsFromStack_dataGridView);
            }
            
        }

        public void MarkRedOrangeNotSetUsedQA(DataGridView QuestionsFromStack_dataGridView)
        {
            //https://stackoverflow.com/questions/2189376/how-to-change-row-color-in-datagridview
            foreach (DataGridViewRow row in QuestionsFromStack_dataGridView.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
                try
                {
                    if (Convert.ToInt32(row.Cells["ID"].Value) == -1)
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;
                        //MessageBox.Show("Not Set");
                    }
                    else
                    {
                        if (Convert.ToInt32(row.Cells["Used"].Value) > 0)
                        {
                            row.DefaultCellStyle.BackColor = Color.Orange;
                            //MessageBox.Show(row.Cells["Used"].Value.ToString());
                        }
                    }
                }
                catch (Exception)
                {

                }
                //More code here
            }
        }

        private void AddStackQ_toolStripButton_Click(object sender, EventArgs e)
        {
            FORM_StackAdd fStackAdd = new FORM_StackAdd(StacksOfQuestionPreview_dataGridView);
            fStackAdd.ShowDialog();
            if (fStackAdd.stackName != "")
            {
                QAStack.AddStackWithQuestions(fStackAdd.qtype, fStackAdd.stackName);
                RefreshStacksWithQuestions();
            }
        }

        private void DeleteStack_toolStripButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(StackIDselected) || !String.IsNullOrWhiteSpace(StackIDselected))
            {
                QAStack.DeleteStackWithQuestions(StackIDselected);
                RefreshStacksWithQuestions();
            }
        }

        private void ImportStack_toolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void RefreshQuestionStack_toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshStacksWithQuestions();
        }

        public void RefreshStacksWithQuestions()
        {
            RefreshStacks();
            EnableDisableOperationStackToolstripButtons();
            RefreshQAinStack(QuestionsLIVEPreview_dataGridView, StackSettings.Default.LiveStackID);
        }

        public void EnableDisableOperationStackToolstripButtons()
        {
            AddStackQ_toolStripButton.Enabled = false;
            DeleteStack_toolStripButton.Enabled = false;
            RenameStack_toolStripButton.Enabled = false;
            ImportStack_toolStripButton.Enabled = false;
            SetLiveStackQ_toolStripButton.Enabled = false;
            BuildAStack_toolStripButton.Enabled = false;
            EditQuestion_toolStripButton.Enabled = false;
            ReplaceQuestion_toolStripButton.Enabled = false;
            ClearAllQuestionsFromStack_toolStripButton.Enabled = false;
            ClearSelectedQuestion_toolStripButton.Enabled = false;
            SearchQuestion_toolStripButton.Enabled = false;
            PrintCurrentlyOpenedStack_toolStripButton.Enabled = false;
            MoveQuestionUp_toolStripButton.Enabled = false;
            MoveQuestionDown_toolStripButton.Enabled = false;
            if (QAStack.CanConnectStacks)
            {
                AddStackQ_toolStripButton.Enabled = true;
                DeleteStack_toolStripButton.Enabled = true;
                RenameStack_toolStripButton.Enabled = true;
                ImportStack_toolStripButton.Enabled = true;
                SetLiveStackQ_toolStripButton.Enabled = true;
                BuildAStack_toolStripButton.Enabled = true;
                EditQuestion_toolStripButton.Enabled = true;
                ReplaceQuestion_toolStripButton.Enabled = true;
                ClearAllQuestionsFromStack_toolStripButton.Enabled = true;
                ClearSelectedQuestion_toolStripButton.Enabled = true;
                SearchQuestion_toolStripButton.Enabled = true;
                PrintCurrentlyOpenedStack_toolStripButton.Enabled = true;
                MoveQuestionUp_toolStripButton.Enabled = true;
                MoveQuestionDown_toolStripButton.Enabled = true;
            }
        }

        private void QAquerySearch(object sender, EventArgs e)
        {

        }

        public void EditQuestion_toolStripButton_Click(object sender, EventArgs e)
        {
            EditQuestion(StackIDselected, QuestionIDfromStackSelected, 2);
        }     

        public void EditQuestion(string StackID, string QuestionID, int Privileges)
        {
            FORM_AddEditQuestions editQtoDB = new FORM_AddEditQuestions(StackID, QuestionID, Privileges);
            editQtoDB.ShowDialog();
        }

        private void QuestionsStack_DoubleClick(object sender, MouseEventArgs e)
        {
            EditQuestion(StackIDselected, QuestionIDfromStackSelected, 2);
        }

        private void QuestionsLIVEStack_DoubleClick(object sender, MouseEventArgs e)
        {
            EditQuestion(StackSettings.Default.LiveStackID, QuestionIDfromLIVEStackSelected, 2);
        }

        private void ClearSelectedQuestion_toolStripButton_Click(object sender, EventArgs e)
        {
            QAStack.ClearOneQuestionInStack(StackIDselected, LevelSelectedFromStack);
            int LevelTemp = QuestionsFromStackPreview_dataGridView.CurrentCell.RowIndex;
            RefreshQAinStack(QuestionsFromStackPreview_dataGridView, StackIDselected);
            if (LevelTemp > 0)
            {
                QuestionsFromStackPreview_dataGridView.ClearSelection();
                QuestionsFromStackPreview_dataGridView.CurrentCell = QuestionsFromStackPreview_dataGridView.Rows[LevelTemp].Cells["Question"];
                //QuestionsFromStackPreview_dataGridView.Rows[LevelTemp].Selected = true;
                QuestionSelectionChanged();
            }           
        }

        private void ClearAllQuestionsFromStack_toolStripButton_Click(object sender, EventArgs e)
        {
            ClearAllQuestionFromStack(QuestionsFromStackPreview_dataGridView,StackIDselected);
        }

        public void ClearAllQuestionFromStack(DataGridView QFromStackPreview_dataGridView, string StackIDselected)
        {
            QAStack.ClearAllQuestionsInStack(StackIDselected);
            RefreshQAinStack(QFromStackPreview_dataGridView, StackIDselected);
        }
        
        private void BuildAStack_toolStripButton_Click(object sender, EventArgs e)
        {
            BuildAStack(QuestionsFromStackPreview_dataGridView,StackIDselected,StackTypeChosen);
        }

        public void BuildAStack(DataGridView QuestionsFromStackPreview_dataGridView, string StackIDselected, string StackTypeChosen)
        {
            QAStack.BuildStackWithQuestions(StackIDselected, StackTypeChosen);
            RefreshQAinStack(QuestionsFromStackPreview_dataGridView, StackIDselected);
        }

        private void SetLiveStackQ_toolStripButton_Click(object sender, EventArgs e)
        {
            QAStack.InsertQuestionsInLIVEStack(StackIDselected, StackTypeChosen);
            StackSettings.Default.LiveStackID = StackIDselected;
            StackSettings.Default.LiveStackTypeChosen = StackTypeChosen;
            StackSettings.Default.LiveStackNameselected = StackNameselected;
            RefreshQAinStack(QuestionsLIVEPreview_dataGridView, StackSettings.Default.LiveStackID);
           
            StackSettings.Default.Save();
            MainTabControl.SelectedTab = Showtime_tabPage;
        }

        public void StacksOfQuestions_SelectionChanged(object sender, EventArgs e)
        {
            StackOfQuestionsSelectionChanged();
        }

        public void StackOfQuestionsSelectionChanged()
        {
            if (StacksOfQuestionPreview_dataGridView.CurrentCell != null)
            {
                StackIDselected = Convert.ToString(StacksOfQuestionPreview_dataGridView.CurrentRow.Cells["ID"].Value);
                StackTypeChosen = Convert.ToString(StacksOfQuestionPreview_dataGridView.CurrentRow.Cells["Type"].Value);
                StackNameselected = Convert.ToString(StacksOfQuestionPreview_dataGridView.CurrentRow.Cells["Stack"].Value);
                CurrentStack_textBox.Text = StackNameselected;
            }

            if (!String.IsNullOrEmpty(StackIDselected) || !String.IsNullOrWhiteSpace(StackIDselected))
            {
                RefreshQAinStack(QuestionsFromStackPreview_dataGridView, StackIDselected);
            }
        }

        private void QuestionInStack_SelectionChanged(object sender, EventArgs e)
        {
            QuestionSelectionChanged();
        }

        public void QuestionSelectionChanged()
        {
            LevelSelectedFromStack = Convert.ToString(QuestionsFromStackPreview_dataGridView.CurrentRow.Cells["Level"].Value);
            QuestionIDfromStackSelected = Convert.ToString(QuestionsFromStackPreview_dataGridView.CurrentRow.Cells["ID"].Value);
            CategorySelected = Convert.ToString(QuestionsFromStackPreview_dataGridView.CurrentRow.Cells[1].Value);
            SubcategorySelected = Convert.ToString(QuestionsFromStackPreview_dataGridView.CurrentRow.Cells[2].Value);
        }

        private void MainTabControl_SelectedEvent(object sender, TabControlEventArgs e)
        {
            if (MainTabControl.SelectedTab == Stacks_tabPage || MainTabControl.SelectedTab == Showtime_tabPage)
            {
                //Timer_RecolorOrangeRedQA.Start();
            }
        }

        private void ReplaceQuestion_toolStripButton_Click(object sender, EventArgs e)
        {
            FORM_ReplacementQuestion replaceQtoDB = new FORM_ReplacementQuestion(StackIDselected, StackTypeChosen, LevelSelectedFromStack,CategorySelected,SubcategorySelected);
            replaceQtoDB.ShowDialog();
            int LevelTemp = QuestionsFromStackPreview_dataGridView.CurrentCell.RowIndex;
            RefreshQAinStack(QuestionsFromStackPreview_dataGridView, StackIDselected);
            if (LevelTemp > 0)
            {
                QuestionsFromStackPreview_dataGridView.ClearSelection();
                //StacksOfQuestions_SelectionChanged(QuestionsFromStackPreview_dataGridView, null);
                //QuestionsFromStackPreview_dataGridView.FirstDisplayedScrollingRowIndex = QuestionsFromStackPreview_dataGridView.RowCount-1;
                QuestionsFromStackPreview_dataGridView.CurrentCell = QuestionsFromStackPreview_dataGridView.Rows[LevelTemp].Cells["Question"];
                QuestionsFromStackPreview_dataGridView.Rows[LevelTemp].Selected = true;
                QuestionSelectionChanged();
                //QuestionInStack_SelectionChanged(QuestionsFromStackPreview_dataGridView, new EventArgs());
                //MessageBox.Show("");
            }
        }

        private void SearchQuestion_toolStripButton_Click(object sender, EventArgs e)
        {
            FORM_SearchQuestions searchQtoDB = new FORM_SearchQuestions(StackIDselected, StackTypeChosen, LevelSelectedFromStack);
            searchQtoDB.ShowDialog();
            int LevelTemp = QuestionsFromStackPreview_dataGridView.CurrentCell.RowIndex;
            RemainQStackLevelIndex(StackIDselected, LevelTemp, QuestionsFromStackPreview_dataGridView);
        }

        private void MoveQuestionUp_toolStripButton_Click(object sender, EventArgs e)
        {
            MoveQuestionUpOrDown(StackIDselected, LevelSelectedFromStack, 1);
        }

        private void MoveQuestionDown_toolStripButton_Click(object sender, EventArgs e)
        {
            MoveQuestionUpOrDown(StackIDselected, LevelSelectedFromStack, -1 );
        }

        public void MoveQuestionUpOrDown(string StackID, string LevelSelected, int UpOrDown, bool isLiveStack = false)
        {
            int NewLevel = Convert.ToInt16(LevelSelected) + UpOrDown;
            int LevelTemp;

            LevelTemp = QuestionsFromStackPreview_dataGridView.CurrentCell.RowIndex - UpOrDown;
            QAStack.MoveQuestionInStack(StackID, LevelSelected, NewLevel.ToString());
            RemainQStackLevelIndex(StackID, LevelTemp, QuestionsFromStackPreview_dataGridView);

        }

        public void RemainQStackLevelIndex(string StackID, int LevelTemp, DataGridView QStackDataGridView)
        {
            RefreshQAinStack(QStackDataGridView, StackID);
            if (LevelTemp > 0)
            {
                QStackDataGridView.ClearSelection();
                QStackDataGridView.CurrentCell = QStackDataGridView.Rows[LevelTemp].Cells["Question"];
                QStackDataGridView.Rows[LevelTemp].Selected = true;
                QuestionSelectionChanged();
            }
        }

        private void RenameStack_toolStripButton_Click(object sender, EventArgs e)
        {
            QAStack.RenameStack(StackIDselected, CurrentStack_textBox.Text);
            int StackIDTemp = StacksOfQuestionPreview_dataGridView.CurrentCell.RowIndex;
            RefreshStacksWithQuestions();
            if (StackIDTemp > 0)
            {
                StacksOfQuestionPreview_dataGridView.ClearSelection();
                StacksOfQuestionPreview_dataGridView.Rows[StackIDTemp].Cells[0].Selected = true;
            }
        }

        private void Timer_RecolorOrangeRedQA_Tick(object sender, EventArgs e)
        {
            RefreshStacksWithQuestions();
            Timer_RecolorOrangeRedQA.Stop();
        }

        private void ReplaceLIVEQuestion_toolStripButton_Click(object sender, EventArgs e)
        {
            FORM_ReplacementQuestion replaceQtoDB = new FORM_ReplacementQuestion(StackSettings.Default.LiveStackID, StackSettings.Default.LiveStackTypeChosen, LevelSelectedFromLIVEStack, CategorySelectedFromLIVEStack, SubcategorySelectedFromLIVEStack);
            replaceQtoDB.ShowDialog();
            QAStack.InsertQuestionsInLIVEStack(StackSettings.Default.LiveStackID,StackTypeChosen); //Because MK WWTBAM QuizOperator is made like that...
            int LevelTemp = QuestionsLIVEPreview_dataGridView.CurrentCell.RowIndex;
            RemainQStackLevelIndex(StackSettings.Default.LiveStackID, LevelTemp, QuestionsLIVEPreview_dataGridView);
            if (LevelTemp > 0)
            {
                QuestionsLIVEPreview_dataGridView.ClearSelection();
                QuestionsLIVEPreview_dataGridView.CurrentCell = QuestionsFromStackPreview_dataGridView.Rows[LevelTemp].Cells["Question"];
                QuestionsLIVEPreview_dataGridView.Rows[LevelTemp].Selected = true;
                QuestionLIVESelectionChanged();
            }
        }

        private void QuestionLIVE_SelectionChanged(object sender, EventArgs e)
        {
            QuestionLIVESelectionChanged();
        }

        public void QuestionLIVESelectionChanged()
        {
            //IF NOT SELECTED LIVE
            LevelSelectedFromLIVEStack = Convert.ToString(QuestionsLIVEPreview_dataGridView.CurrentRow.Cells["Level"].Value);
            QuestionIDfromLIVEStackSelected = Convert.ToString(QuestionsLIVEPreview_dataGridView.CurrentRow.Cells["ID"].Value);
            CategorySelectedFromLIVEStack = Convert.ToString(QuestionsLIVEPreview_dataGridView.CurrentRow.Cells[1].Value);
            SubcategorySelectedFromLIVEStack = Convert.ToString(QuestionsLIVEPreview_dataGridView.CurrentRow.Cells[2].Value);
        }

        private void RefreshQuestionLIVEStack_toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshQAinStack(QuestionsLIVEPreview_dataGridView, StackSettings.Default.LiveStackID);
        }

        private void EditLIVEQuestion_toolStripButton_Click(object sender, EventArgs e)
        {
            EditQuestion(StackSettings.Default.LiveStackID, QuestionIDfromLIVEStackSelected, 2);
            QAStack.InsertQuestionsInLIVEStack(StackSettings.Default.LiveStackID,StackTypeChosen); //Because MK WWTBAM QuizOperator is made like that...
        }

        private void ClearSelectedLIVEQuestion_toolStripButton_Click(object sender, EventArgs e)
        {
            QAStack.ClearOneQuestionInStack(StackSettings.Default.LiveStackID, LevelSelectedFromLIVEStack);
            QAStack.InsertQuestionsInLIVEStack(StackSettings.Default.LiveStackID,StackTypeChosen); //Because MK WWTBAM QuizOperator is made like that...
            int LevelTemp = QuestionsLIVEPreview_dataGridView.CurrentCell.RowIndex;
            RemainQStackLevelIndex(StackSettings.Default.LiveStackID, LevelTemp, QuestionsLIVEPreview_dataGridView);
        }

        private void ClearAllQuestionsFromLIVEStack_toolStripButton_Click(object sender, EventArgs e)
        {
            ClearAllQuestionFromStack(QuestionsLIVEPreview_dataGridView, StackSettings.Default.LiveStackID);
        }

        private void SearchLIVEQuestion_toolStripButton_Click(object sender, EventArgs e)
        {
            FORM_SearchQuestions searchQtoDB = new FORM_SearchQuestions(StackSettings.Default.LiveStackID, StackSettings.Default.LiveStackTypeChosen, LevelSelectedFromLIVEStack);
            searchQtoDB.ShowDialog();
            QAStack.InsertQuestionsInLIVEStack(StackSettings.Default.LiveStackID,StackTypeChosen); //Because MK WWTBAM QuizOperator is made like that...
            int LevelTemp = QuestionsLIVEPreview_dataGridView.CurrentCell.RowIndex;
            RemainQStackLevelIndex(StackSettings.Default.LiveStackID, LevelTemp, QuestionsLIVEPreview_dataGridView);
        }

        private void PrintCurrentLIVEStack_toolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void MoveLIVEQuestionUp_toolStripButton_Click(object sender, EventArgs e)
        {
            MoveLIVEQuestionUpOrDown(LevelSelectedFromStack, 1);
        }

        private void MoveLIVEQuestionDown_toolStripButton_Click(object sender, EventArgs e)
        {
            MoveLIVEQuestionUpOrDown(LevelSelectedFromStack, -1);
        }

        public void MoveLIVEQuestionUpOrDown(string LevelSelected, int UpOrDown, bool isLiveStack = false)
        {
            int NewLevel = Convert.ToInt16(LevelSelected) + UpOrDown;
            int LevelTemp = QuestionsLIVEPreview_dataGridView.CurrentCell.RowIndex - UpOrDown;
            QAStack.MoveQuestionInLIVEStack(StackSettings.Default.LiveStackTypeChosen, LevelSelected, NewLevel.ToString());
            RemainQStackLevelIndex(StackSettings.Default.LiveStackID, LevelTemp, QuestionsLIVEPreview_dataGridView);
        }

        private void BuildALIVEStack_toolStripButton_Click(object sender, EventArgs e)
        {

        }


    }
}
