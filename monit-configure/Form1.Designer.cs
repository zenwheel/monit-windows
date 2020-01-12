namespace monit
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.saveButton = new System.Windows.Forms.Button();
			this.serverLabel = new System.Windows.Forms.Label();
			this.userLabel = new System.Windows.Forms.Label();
			this.passwordLabel = new System.Windows.Forms.Label();
			this.serverText = new System.Windows.Forms.TextBox();
			this.userText = new System.Windows.Forms.TextBox();
			this.passwordText = new System.Windows.Forms.TextBox();
			this.sslCheckBox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Location = new System.Drawing.Point(446, 98);
			this.saveButton.Margin = new System.Windows.Forms.Padding(2);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(76, 27);
			this.saveButton.TabIndex = 0;
			this.saveButton.Text = "Save";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// serverLabel
			// 
			this.serverLabel.AutoSize = true;
			this.serverLabel.Location = new System.Drawing.Point(11, 14);
			this.serverLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.serverLabel.Name = "serverLabel";
			this.serverLabel.Size = new System.Drawing.Size(81, 13);
			this.serverLabel.TabIndex = 1;
			this.serverLabel.Text = "M/Monit Server";
			// 
			// userLabel
			// 
			this.userLabel.AutoSize = true;
			this.userLabel.Location = new System.Drawing.Point(36, 39);
			this.userLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.userLabel.Name = "userLabel";
			this.userLabel.Size = new System.Drawing.Size(55, 13);
			this.userLabel.TabIndex = 2;
			this.userLabel.Text = "Username";
			// 
			// passwordLabel
			// 
			this.passwordLabel.AutoSize = true;
			this.passwordLabel.Location = new System.Drawing.Point(39, 65);
			this.passwordLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.passwordLabel.Name = "passwordLabel";
			this.passwordLabel.Size = new System.Drawing.Size(53, 13);
			this.passwordLabel.TabIndex = 3;
			this.passwordLabel.Text = "Password";
			// 
			// serverText
			// 
			this.serverText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.serverText.Location = new System.Drawing.Point(97, 11);
			this.serverText.Margin = new System.Windows.Forms.Padding(2);
			this.serverText.Name = "serverText";
			this.serverText.Size = new System.Drawing.Size(425, 20);
			this.serverText.TabIndex = 4;
			// 
			// userText
			// 
			this.userText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.userText.Location = new System.Drawing.Point(97, 36);
			this.userText.Name = "userText";
			this.userText.Size = new System.Drawing.Size(424, 20);
			this.userText.TabIndex = 5;
			// 
			// passwordText
			// 
			this.passwordText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.passwordText.Location = new System.Drawing.Point(97, 62);
			this.passwordText.Name = "passwordText";
			this.passwordText.Size = new System.Drawing.Size(424, 20);
			this.passwordText.TabIndex = 6;
			this.passwordText.UseSystemPasswordChar = true;
			// 
			// sslCheckBox
			// 
			this.sslCheckBox.AutoSize = true;
			this.sslCheckBox.Location = new System.Drawing.Point(97, 89);
			this.sslCheckBox.Name = "sslCheckBox";
			this.sslCheckBox.Size = new System.Drawing.Size(109, 17);
			this.sslCheckBox.TabIndex = 7;
			this.sslCheckBox.Text = "Ignore SSL Errors";
			this.sslCheckBox.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(533, 136);
			this.Controls.Add(this.sslCheckBox);
			this.Controls.Add(this.passwordText);
			this.Controls.Add(this.userText);
			this.Controls.Add(this.serverText);
			this.Controls.Add(this.passwordLabel);
			this.Controls.Add(this.userLabel);
			this.Controls.Add(this.serverLabel);
			this.Controls.Add(this.saveButton);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "Form1";
			this.Text = "Monit Configuration";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Label serverLabel;
		private System.Windows.Forms.Label userLabel;
		private System.Windows.Forms.Label passwordLabel;
		private System.Windows.Forms.TextBox serverText;
		private System.Windows.Forms.TextBox userText;
		private System.Windows.Forms.TextBox passwordText;
		private System.Windows.Forms.CheckBox sslCheckBox;
	}
}

