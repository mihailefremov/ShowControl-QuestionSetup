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
    public partial class FORM_AdvancedConfigurations : Form
    {

        public FORM_AdvancedConfigurations()
        {
            InitializeComponent();
        }

        private void AdvancedConfigurations_Load(object sender, EventArgs e)
        {
            Gameplay1stackQuestions_textBox.Text = AdvancedGameConfigSettings.Default.GamePlay1_TotalStackQ.ToString();
            QualificationGame1_textBox.Text = AdvancedGameConfigSettings.Default.QualificationGame1_TotalStackQ.ToString();

            if (AdvancedGameConfigSettings.Default.StackBuild_UnusedUsedBothQ == 0)
            {
                UnusedQA_checkBox.Checked = true;
                UsedQA_checkBox.Checked = false;
            }
            else if (AdvancedGameConfigSettings.Default.StackBuild_UnusedUsedBothQ == 1)
            {
                UnusedQA_checkBox.Checked = false;
                UsedQA_checkBox.Checked = true;
            }
            else if (AdvancedGameConfigSettings.Default.StackBuild_UnusedUsedBothQ == 2)
            {
                UnusedQA_checkBox.Checked = true;
                UsedQA_checkBox.Checked = true;
            }

        }

        public void StackBuildUpSettingsChange(object sender, EventArgs e)
        {
            AdvancedGameConfigSettings.Default.UsageSelectionQuery = "0";

            if (UnusedQA_checkBox.Checked == true && UsedQA_checkBox.Checked == false)
            {
                AdvancedGameConfigSettings.Default.StackBuild_UnusedUsedBothQ = 0;
                AdvancedGameConfigSettings.Default.UsageSelectionQuery = "0";
            }
            else if (UnusedQA_checkBox.Checked == false && UsedQA_checkBox.Checked == true)
            {
                AdvancedGameConfigSettings.Default.StackBuild_UnusedUsedBothQ = 1;
                AdvancedGameConfigSettings.Default.UsageSelectionQuery = "1";
            }
            else if (UnusedQA_checkBox.Checked == true && UsedQA_checkBox.Checked == true)
            {
                AdvancedGameConfigSettings.Default.StackBuild_UnusedUsedBothQ = 2;
                AdvancedGameConfigSettings.Default.UsageSelectionQuery = "0";
            }

            if (UseOnlyQ_checkBox.Checked == true)
            {
                if (TopQuestionSelect_radioButton.Checked == true)
                {
                    AdvancedGameConfigSettings.Default.TopBottomSelectionQuery = " order by QuestionID asc Limit " + NumberOfQselect_textBox.Text;
                }
                else if (BottomQuestionSelect_radioButton.Checked == true)
                {
                    AdvancedGameConfigSettings.Default.TopBottomSelectionQuery = " order by QuestionID desc Limit " + NumberOfQselect_textBox.Text;
                }
            }
            else if (UseOnlyQ_checkBox.Checked == false)
            {
                AdvancedGameConfigSettings.Default.TopBottomSelectionQuery = "";
            }

            AdvancedGameConfigSettings.Default.Save();

        }

        private void Gameplay1stackQuestions_textBox_TextChanged(object sender, EventArgs e)
        {
            AdvancedGameConfigSettings.Default.GamePlay1_TotalStackQ = Convert.ToInt16(Gameplay1stackQuestions_textBox.Text);
            AdvancedGameConfigSettings.Default.Save();
        }

        private void Gameplay2stackQuestions_textBox_TextChanged(object sender, EventArgs e)
        {
            AdvancedGameConfigSettings.Default.QualificationGame1_Name = QualificationGame1_textBox.Text;
            AdvancedGameConfigSettings.Default.Save();
        }
    }
}
