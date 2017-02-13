namespace CrappyListenMoe
{
	partial class FormLogin
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
			this.button1 = new System.Windows.Forms.Button();
			this.textBox2 = new CrappyListenMoe.GhostTextbox();
			this.textBox1 = new CrappyListenMoe.GhostTextbox();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(26)))), ((int)(((byte)(85)))));
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(26)))), ((int)(((byte)(85)))));
			this.button1.Location = new System.Drawing.Point(404, 11);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(44, 24);
			this.button1.TabIndex = 2;
			this.button1.Text = "Login";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox2
			// 
			this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(46)))), ((int)(((byte)(59)))));
			this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox2.ForeColor = System.Drawing.Color.White;
			this.textBox2.GhostText = "Password";
			this.textBox2.Location = new System.Drawing.Point(208, 11);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(190, 20);
			this.textBox2.TabIndex = 4;
			this.textBox2.UseSystemPasswordChar = true;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(46)))), ((int)(((byte)(59)))));
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox1.ForeColor = System.Drawing.Color.White;
			this.textBox1.GhostText = "Username";
			this.textBox1.Location = new System.Drawing.Point(12, 11);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(190, 20);
			this.textBox1.TabIndex = 3;
			// 
			// FormLogin
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(46)))), ((int)(((byte)(59)))));
			this.ClientSize = new System.Drawing.Size(460, 46);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormLogin";
			this.Text = "Login";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button button1;
		private GhostTextbox textBox1;
		private GhostTextbox textBox2;
	}
}