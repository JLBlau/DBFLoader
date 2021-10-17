namespace LoadFoxProDBToSQL
{
    partial class Form1
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
            this.DBFLabel = new System.Windows.Forms.Label();
            this.dbfPath = new System.Windows.Forms.TextBox();
            this.selectFolder = new System.Windows.Forms.Button();
            this.sqlServerLabel = new System.Windows.Forms.Label();
            this.sqlServerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.sqlUserName = new System.Windows.Forms.TextBox();
            this.sqlPasswordLabel = new System.Windows.Forms.Label();
            this.sqlPassword = new System.Windows.Forms.TextBox();
            this.sqlDBLabel = new System.Windows.Forms.Label();
            this.newSQLDBName = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.lbMessages = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // DBFLabel
            // 
            this.DBFLabel.AutoSize = true;
            this.DBFLabel.Location = new System.Drawing.Point(7, 15);
            this.DBFLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DBFLabel.Name = "DBFLabel";
            this.DBFLabel.Size = new System.Drawing.Size(72, 13);
            this.DBFLabel.TabIndex = 0;
            this.DBFLabel.Text = "DBF Location";
            // 
            // dbfPath
            // 
            this.dbfPath.Location = new System.Drawing.Point(118, 15);
            this.dbfPath.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.dbfPath.Name = "dbfPath";
            this.dbfPath.Size = new System.Drawing.Size(114, 20);
            this.dbfPath.TabIndex = 1;
            this.dbfPath.Text = "C:\\Temp\\DBFs";
            // 
            // selectFolder
            // 
            this.selectFolder.Location = new System.Drawing.Point(260, 10);
            this.selectFolder.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.selectFolder.Name = "selectFolder";
            this.selectFolder.Size = new System.Drawing.Size(82, 27);
            this.selectFolder.TabIndex = 2;
            this.selectFolder.Text = "Select Folder";
            this.selectFolder.UseVisualStyleBackColor = true;
            this.selectFolder.Click += new System.EventHandler(this.selectFolder_Click);
            // 
            // sqlServerLabel
            // 
            this.sqlServerLabel.AutoSize = true;
            this.sqlServerLabel.Location = new System.Drawing.Point(7, 47);
            this.sqlServerLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.sqlServerLabel.Name = "sqlServerLabel";
            this.sqlServerLabel.Size = new System.Drawing.Size(62, 13);
            this.sqlServerLabel.TabIndex = 3;
            this.sqlServerLabel.Text = "SQL Server";
            // 
            // sqlServerName
            // 
            this.sqlServerName.Location = new System.Drawing.Point(118, 46);
            this.sqlServerName.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.sqlServerName.Name = "sqlServerName";
            this.sqlServerName.Size = new System.Drawing.Size(114, 20);
            this.sqlServerName.TabIndex = 4;
            this.sqlServerName.Text = "SQLServer";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 73);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "SQL User";
            // 
            // sqlUserName
            // 
            this.sqlUserName.Location = new System.Drawing.Point(118, 73);
            this.sqlUserName.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.sqlUserName.Name = "sqlUserName";
            this.sqlUserName.Size = new System.Drawing.Size(114, 20);
            this.sqlUserName.TabIndex = 6;
            this.sqlUserName.Text = "SA";
            // 
            // sqlPasswordLabel
            // 
            this.sqlPasswordLabel.AutoSize = true;
            this.sqlPasswordLabel.Location = new System.Drawing.Point(7, 100);
            this.sqlPasswordLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.sqlPasswordLabel.Name = "sqlPasswordLabel";
            this.sqlPasswordLabel.Size = new System.Drawing.Size(77, 13);
            this.sqlPasswordLabel.TabIndex = 7;
            this.sqlPasswordLabel.Text = "SQL Password";
            // 
            // sqlPassword
            // 
            this.sqlPassword.Location = new System.Drawing.Point(118, 99);
            this.sqlPassword.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.sqlPassword.Name = "sqlPassword";
            this.sqlPassword.Size = new System.Drawing.Size(114, 20);
            this.sqlPassword.TabIndex = 8;
            this.sqlPassword.Text = "password";
            this.sqlPassword.UseSystemPasswordChar = true;
            // 
            // sqlDBLabel
            // 
            this.sqlDBLabel.AutoSize = true;
            this.sqlDBLabel.Location = new System.Drawing.Point(6, 127);
            this.sqlDBLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.sqlDBLabel.Name = "sqlDBLabel";
            this.sqlDBLabel.Size = new System.Drawing.Size(102, 13);
            this.sqlDBLabel.TabIndex = 9;
            this.sqlDBLabel.Text = "New SQL DB Name";
            // 
            // newSQLDBName
            // 
            this.newSQLDBName.Location = new System.Drawing.Point(118, 127);
            this.newSQLDBName.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.newSQLDBName.Name = "newSQLDBName";
            this.newSQLDBName.Size = new System.Drawing.Size(114, 20);
            this.newSQLDBName.TabIndex = 10;
            this.newSQLDBName.Text = "Staged_DBFs";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(260, 118);
            this.startButton.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(82, 25);
            this.startButton.TabIndex = 11;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // lbMessages
            // 
            this.lbMessages.FormattingEnabled = true;
            this.lbMessages.Location = new System.Drawing.Point(9, 177);
            this.lbMessages.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.lbMessages.Name = "lbMessages";
            this.lbMessages.Size = new System.Drawing.Size(339, 121);
            this.lbMessages.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 320);
            this.Controls.Add(this.lbMessages);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.newSQLDBName);
            this.Controls.Add(this.sqlDBLabel);
            this.Controls.Add(this.sqlPassword);
            this.Controls.Add(this.sqlPasswordLabel);
            this.Controls.Add(this.sqlUserName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sqlServerName);
            this.Controls.Add(this.sqlServerLabel);
            this.Controls.Add(this.selectFolder);
            this.Controls.Add(this.dbfPath);
            this.Controls.Add(this.DBFLabel);
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.Name = "Form1";
            this.Text = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DBFLabel;
        private System.Windows.Forms.TextBox dbfPath;
        private System.Windows.Forms.Button selectFolder;
        private System.Windows.Forms.Label sqlServerLabel;
        private System.Windows.Forms.TextBox sqlServerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox sqlUserName;
        private System.Windows.Forms.Label sqlPasswordLabel;
        private System.Windows.Forms.TextBox sqlPassword;
        private System.Windows.Forms.Label sqlDBLabel;
        private System.Windows.Forms.TextBox newSQLDBName;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ListBox lbMessages;
    }
}

