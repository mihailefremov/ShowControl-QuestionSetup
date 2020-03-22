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
    public partial class FORM_StackAdd : Form
    {
        public int qtype=1;
        public string stackName = "";
        DataGridView datagrid;
        
        public FORM_StackAdd(DataGridView StacksOfQuestionPreview_dataGridView)
        {
            datagrid = StacksOfQuestionPreview_dataGridView;
            InitializeComponent();
        }

        public void AddStackIFOK_Button_Click(object sender, EventArgs e)
        {
            if (stackName == "")
            {
                MessageBox.Show("Enter stack name!", "Important Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.Close();
            }
        }

        private void Qtype1_Button_Click(object sender, EventArgs e)
        {
            qtype = 1;
            Qtype1_Button.BackColor = Color.LightYellow;
            Qtype2_Button.BackColor = Color.WhiteSmoke;
        }

        private void Qtype2_Button_Click(object sender, EventArgs e)
        {
            qtype = 2;
            Qtype2_Button.BackColor = Color.LightYellow;
            Qtype1_Button.BackColor = Color.WhiteSmoke;
        }

        private void StackName_textBox_TextChanged(object sender, EventArgs e)
        {
            stackName = StackName_textBox.Text;
        }
    }
}
