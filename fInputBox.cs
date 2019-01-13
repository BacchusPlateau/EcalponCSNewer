using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Ecalpon
{
	/// <summary>
	/// Summary description for fInputBox.
	/// </summary>
	public class fInputBox : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtInput;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Label lblQuestion;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private string sResponse = "";

		public string Response 
		{
			get 
			{
				return sResponse;
			}
			set 
			{
				sResponse = value;
			}
		}

		
		public fInputBox(string Caption, string Question)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.Text = Caption;
			lblQuestion.Text = Question;

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(fInputBox));
			this.txtInput = new System.Windows.Forms.TextBox();
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.lblQuestion = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtInput
			// 
			this.txtInput.Location = new System.Drawing.Point(40, 80);
			this.txtInput.Name = "txtInput";
			this.txtInput.Size = new System.Drawing.Size(296, 20);
			this.txtInput.TabIndex = 0;
			this.txtInput.Text = "";
			this.txtInput.Leave += new System.EventHandler(this.txtInput_Leave);
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point(120, 136);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(48, 24);
			this.cmdOK.TabIndex = 1;
			this.cmdOK.Text = "&OK";
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.Location = new System.Drawing.Point(200, 136);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(48, 24);
			this.cmdCancel.TabIndex = 2;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// lblQuestion
			// 
			this.lblQuestion.Location = new System.Drawing.Point(40, 24);
			this.lblQuestion.Name = "lblQuestion";
			this.lblQuestion.Size = new System.Drawing.Size(296, 40);
			this.lblQuestion.TabIndex = 3;
			// 
			// fInputBox
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(360, 173);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lblQuestion,
																		  this.cmdCancel,
																		  this.cmdOK,
																		  this.txtInput});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "fInputBox";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.ResumeLayout(false);

		}
		#endregion

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void txtInput_Leave(object sender, System.EventArgs e)
		{
			Response = txtInput.Text;
		}
	}
}
