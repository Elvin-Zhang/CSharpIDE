namespace CSharpIDE.Views
{
    partial class NewProjectWindow
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
            this.ProjectNameTxtBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ProjectPathTxtBox = new System.Windows.Forms.TextBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.ChooseFolderButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ProjectNameTxtBox
            // 
            this.ProjectNameTxtBox.Location = new System.Drawing.Point(15, 25);
            this.ProjectNameTxtBox.Name = "ProjectNameTxtBox";
            this.ProjectNameTxtBox.Size = new System.Drawing.Size(178, 20);
            this.ProjectNameTxtBox.TabIndex = 0;
            this.ProjectNameTxtBox.TextChanged += new System.EventHandler(this.ProjectNameTxtBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Folder:";
            // 
            // ProjectPathTxtBox
            // 
            this.ProjectPathTxtBox.Enabled = false;
            this.ProjectPathTxtBox.Location = new System.Drawing.Point(15, 64);
            this.ProjectPathTxtBox.Name = "ProjectPathTxtBox";
            this.ProjectPathTxtBox.Size = new System.Drawing.Size(178, 20);
            this.ProjectPathTxtBox.TabIndex = 3;
            this.ProjectPathTxtBox.TextChanged += new System.EventHandler(this.ProjectPathTxtBox_TextChanged);
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(15, 127);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 5;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(118, 127);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 6;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // ChooseFolderButton
            // 
            this.ChooseFolderButton.Location = new System.Drawing.Point(15, 91);
            this.ChooseFolderButton.Name = "ChooseFolderButton";
            this.ChooseFolderButton.Size = new System.Drawing.Size(178, 23);
            this.ChooseFolderButton.TabIndex = 7;
            this.ChooseFolderButton.Text = "Choose Folder";
            this.ChooseFolderButton.UseVisualStyleBackColor = true;
            this.ChooseFolderButton.Click += new System.EventHandler(this.ChooseFolderButton_Click);
            // 
            // NewProjectWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(205, 162);
            this.Controls.Add(this.ChooseFolderButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.ProjectPathTxtBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ProjectNameTxtBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "NewProjectWindow";
            this.Text = "Create New Project";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ProjectNameTxtBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ProjectPathTxtBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button ChooseFolderButton;
    }
}