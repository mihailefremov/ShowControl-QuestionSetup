namespace QuestionEditAndPreset
{
    partial class FORM_StackAdd
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Qtype1_Button = new System.Windows.Forms.Button();
            this.Qtype2_Button = new System.Windows.Forms.Button();
            this.StackName_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AddStackIFOK_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Qtype1_Button
            // 
            this.Qtype1_Button.BackColor = System.Drawing.Color.LightYellow;
            this.Qtype1_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Qtype1_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Qtype1_Button.Location = new System.Drawing.Point(35, 38);
            this.Qtype1_Button.Name = "Qtype1_Button";
            this.Qtype1_Button.Size = new System.Drawing.Size(84, 48);
            this.Qtype1_Button.TabIndex = 0;
            this.Qtype1_Button.Text = "Game Play";
            this.Qtype1_Button.UseVisualStyleBackColor = false;
            this.Qtype1_Button.Click += new System.EventHandler(this.Qtype1_Button_Click);
            // 
            // Qtype2_Button
            // 
            this.Qtype2_Button.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Qtype2_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Qtype2_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Qtype2_Button.Location = new System.Drawing.Point(150, 38);
            this.Qtype2_Button.Name = "Qtype2_Button";
            this.Qtype2_Button.Size = new System.Drawing.Size(83, 48);
            this.Qtype2_Button.TabIndex = 1;
            this.Qtype2_Button.Text = "Fastest Finger";
            this.Qtype2_Button.UseVisualStyleBackColor = false;
            this.Qtype2_Button.Click += new System.EventHandler(this.Qtype2_Button_Click);
            // 
            // StackName_textBox
            // 
            this.StackName_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StackName_textBox.Location = new System.Drawing.Point(35, 152);
            this.StackName_textBox.Name = "StackName_textBox";
            this.StackName_textBox.Size = new System.Drawing.Size(198, 26);
            this.StackName_textBox.TabIndex = 2;
            this.StackName_textBox.TextChanged += new System.EventHandler(this.StackName_textBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label1.Location = new System.Drawing.Point(32, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Stack name:";
            // 
            // AddStackIFOK_Button
            // 
            this.AddStackIFOK_Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.AddStackIFOK_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddStackIFOK_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddStackIFOK_Button.Location = new System.Drawing.Point(83, 206);
            this.AddStackIFOK_Button.Name = "AddStackIFOK_Button";
            this.AddStackIFOK_Button.Size = new System.Drawing.Size(103, 38);
            this.AddStackIFOK_Button.TabIndex = 4;
            this.AddStackIFOK_Button.Text = "OK";
            this.AddStackIFOK_Button.UseVisualStyleBackColor = false;
            this.AddStackIFOK_Button.Click += new System.EventHandler(this.AddStackIFOK_Button_Click);
            // 
            // FORM_StackAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 280);
            this.Controls.Add(this.AddStackIFOK_Button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StackName_textBox);
            this.Controls.Add(this.Qtype2_Button);
            this.Controls.Add(this.Qtype1_Button);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FORM_StackAdd";
            this.Text = "FORM_StackAdd";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Qtype1_Button;
        private System.Windows.Forms.Button Qtype2_Button;
        private System.Windows.Forms.TextBox StackName_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AddStackIFOK_Button;
    }
}