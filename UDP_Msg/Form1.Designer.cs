namespace UDP_Msg
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
			this.userNameTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.loginButton = new System.Windows.Forms.Button();
			this.logoutButton = new System.Windows.Forms.Button();
			this.chatTextBox = new System.Windows.Forms.TextBox();
			this.messageTextBox = new System.Windows.Forms.TextBox();
			this.sendButton = new System.Windows.Forms.Button();
			this.privateKeyTextBox = new System.Windows.Forms.TextBox();
			this.publicKeyTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// userNameTextBox
			// 
			this.userNameTextBox.Location = new System.Drawing.Point(172, 24);
			this.userNameTextBox.Name = "userNameTextBox";
			this.userNameTextBox.Size = new System.Drawing.Size(295, 20);
			this.userNameTextBox.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(73, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Введите имя";
			this.label1.Click += new System.EventHandler(this.label1_Click);
			// 
			// loginButton
			// 
			this.loginButton.Location = new System.Drawing.Point(495, 19);
			this.loginButton.Name = "loginButton";
			this.loginButton.Size = new System.Drawing.Size(75, 23);
			this.loginButton.TabIndex = 2;
			this.loginButton.Text = "Вход";
			this.loginButton.UseVisualStyleBackColor = true;
			this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
			// 
			// logoutButton
			// 
			this.logoutButton.Location = new System.Drawing.Point(495, 48);
			this.logoutButton.Name = "logoutButton";
			this.logoutButton.Size = new System.Drawing.Size(75, 23);
			this.logoutButton.TabIndex = 3;
			this.logoutButton.Text = "Выход";
			this.logoutButton.UseVisualStyleBackColor = true;
			this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click_1);
			// 
			// chatTextBox
			// 
			this.chatTextBox.Location = new System.Drawing.Point(76, 191);
			this.chatTextBox.Multiline = true;
			this.chatTextBox.Name = "chatTextBox";
			this.chatTextBox.ReadOnly = true;
			this.chatTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.chatTextBox.Size = new System.Drawing.Size(494, 253);
			this.chatTextBox.TabIndex = 4;
			// 
			// messageTextBox
			// 
			this.messageTextBox.Location = new System.Drawing.Point(76, 450);
			this.messageTextBox.Multiline = true;
			this.messageTextBox.Name = "messageTextBox";
			this.messageTextBox.Size = new System.Drawing.Size(391, 51);
			this.messageTextBox.TabIndex = 5;
			// 
			// sendButton
			// 
			this.sendButton.Location = new System.Drawing.Point(495, 450);
			this.sendButton.Name = "sendButton";
			this.sendButton.Size = new System.Drawing.Size(75, 51);
			this.sendButton.TabIndex = 6;
			this.sendButton.Text = "Отправить";
			this.sendButton.UseVisualStyleBackColor = true;
			this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
			// 
			// privateKeyTextBox
			// 
			this.privateKeyTextBox.Location = new System.Drawing.Point(172, 85);
			this.privateKeyTextBox.Multiline = true;
			this.privateKeyTextBox.Name = "privateKeyTextBox";
			this.privateKeyTextBox.ReadOnly = true;
			this.privateKeyTextBox.Size = new System.Drawing.Size(398, 40);
			this.privateKeyTextBox.TabIndex = 7;
			// 
			// publicKeyTextBox
			// 
			this.publicKeyTextBox.Location = new System.Drawing.Point(172, 143);
			this.publicKeyTextBox.Multiline = true;
			this.publicKeyTextBox.Name = "publicKeyTextBox";
			this.publicKeyTextBox.ReadOnly = true;
			this.publicKeyTextBox.Size = new System.Drawing.Size(398, 40);
			this.publicKeyTextBox.TabIndex = 8;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(73, 85);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(87, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "Закрытый ключ";
			this.label2.Click += new System.EventHandler(this.label2_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(73, 143);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(87, 13);
			this.label3.TabIndex = 10;
			this.label3.Text = "Открытый ключ";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(645, 541);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.publicKeyTextBox);
			this.Controls.Add(this.privateKeyTextBox);
			this.Controls.Add(this.sendButton);
			this.Controls.Add(this.messageTextBox);
			this.Controls.Add(this.chatTextBox);
			this.Controls.Add(this.logoutButton);
			this.Controls.Add(this.loginButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.userNameTextBox);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox userNameTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button loginButton;
		private System.Windows.Forms.Button logoutButton;
		private System.Windows.Forms.TextBox chatTextBox;
		private System.Windows.Forms.TextBox messageTextBox;
		private System.Windows.Forms.Button sendButton;
		private System.Windows.Forms.TextBox privateKeyTextBox;
		private System.Windows.Forms.TextBox publicKeyTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
	}
}

