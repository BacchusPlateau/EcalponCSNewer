using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Ecalpon
{
	
	public class fMenu : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbChars;
		private System.Windows.Forms.ListBox lbChars;
		private System.Windows.Forms.Button cmdGo;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public fMenu()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			//Display Characters
			lbChars.DisplayMember = "CCharacter.Name";
			foreach(CCharacter oCharacter in CEcalpon.gobjParty.Characters) 
			{
				lbChars.Items.Add(oCharacter);
			}
			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(fMenu));
			this.gbChars = new System.Windows.Forms.GroupBox();
			this.lbChars = new System.Windows.Forms.ListBox();
			this.cmdGo = new System.Windows.Forms.Button();
			this.gbChars.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbChars
			// 
			this.gbChars.Controls.AddRange(new System.Windows.Forms.Control[] {
																				  this.lbChars});
			this.gbChars.Location = new System.Drawing.Point(64, 32);
			this.gbChars.Name = "gbChars";
			this.gbChars.Size = new System.Drawing.Size(336, 232);
			this.gbChars.TabIndex = 0;
			this.gbChars.TabStop = false;
			// 
			// lbChars
			// 
			this.lbChars.Location = new System.Drawing.Point(16, 24);
			this.lbChars.Name = "lbChars";
			this.lbChars.Size = new System.Drawing.Size(304, 199);
			this.lbChars.TabIndex = 0;
			// 
			// cmdGo
			// 
			this.cmdGo.Location = new System.Drawing.Point(64, 288);
			this.cmdGo.Name = "cmdGo";
			this.cmdGo.Size = new System.Drawing.Size(48, 32);
			this.cmdGo.TabIndex = 1;
			this.cmdGo.Text = "Party On";
			this.cmdGo.Click += new System.EventHandler(this.cmdGo_Click);
			// 
			// fMenu
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(456, 341);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.cmdGo,
																		  this.gbChars});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "fMenu";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Ecalpon";
			this.gbChars.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void cmdGo_Click(object sender, System.EventArgs e)
		{
			this.Close();
			
		}


	}
}
