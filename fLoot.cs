using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Ecalpon
{
	/// <summary>
	/// Distribute gold and items to users
	/// </summary>
	public class fLoot : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblGold;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.GroupBox fraTreasure;
		private System.Windows.Forms.Button cmdCharOne;
		private System.Windows.Forms.Button cmdCharTwo;
		private System.Windows.Forms.Button cmdCharThree;
		private System.Windows.Forms.Button cmdCharFour;
		private System.Windows.Forms.Button cmdCharFive;
		private System.Windows.Forms.ListBox lstLoot;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private CLoot oLoot=null;

		public fLoot(CLoot Loot)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			//Save a local copy of the loot object
			oLoot = Loot;
			
			//display amount of gold
			lblGold.Text = oLoot.Gold.ToString();
			
			//set names of buttons
			foreach(CCharacter oCharacter in CEcalpon.gobjParty.Characters) 
			{
				switch(oCharacter.Order)
				{
					case 1:
						cmdCharOne.Text = oCharacter.Name;
						break;
					case 2:
						cmdCharTwo.Text = oCharacter.Name;
						break;
					case 3:
						cmdCharThree.Text = oCharacter.Name;
						break;
					case 4:
						cmdCharFour.Text = oCharacter.Name;
						break;
					case 5:
						cmdCharFive.Text = oCharacter.Name;
						break;
				}
			}

			//load up items
			foreach(CItem oItem in oLoot.Items)
				lstLoot.Items.Add(oItem);

			//set the display member
			lstLoot.DisplayMember = "Name";

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(fLoot));
			this.label1 = new System.Windows.Forms.Label();
			this.lblGold = new System.Windows.Forms.Label();
			this.cmdOK = new System.Windows.Forms.Button();
			this.fraTreasure = new System.Windows.Forms.GroupBox();
			this.lstLoot = new System.Windows.Forms.ListBox();
			this.cmdCharFive = new System.Windows.Forms.Button();
			this.cmdCharFour = new System.Windows.Forms.Button();
			this.cmdCharThree = new System.Windows.Forms.Button();
			this.cmdCharTwo = new System.Windows.Forms.Button();
			this.cmdCharOne = new System.Windows.Forms.Button();
			this.fraTreasure.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Gold:";
			// 
			// lblGold
			// 
			this.lblGold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblGold.Location = new System.Drawing.Point(64, 40);
			this.lblGold.Name = "lblGold";
			this.lblGold.Size = new System.Drawing.Size(88, 16);
			this.lblGold.TabIndex = 1;
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point(304, 344);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(64, 24);
			this.cmdOK.TabIndex = 2;
			this.cmdOK.Text = "&OK";
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// fraTreasure
			// 
			this.fraTreasure.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.lstLoot,
																					  this.cmdCharFive,
																					  this.cmdCharFour,
																					  this.cmdCharThree,
																					  this.cmdCharTwo,
																					  this.cmdCharOne});
			this.fraTreasure.Location = new System.Drawing.Point(16, 64);
			this.fraTreasure.Name = "fraTreasure";
			this.fraTreasure.Size = new System.Drawing.Size(352, 264);
			this.fraTreasure.TabIndex = 3;
			this.fraTreasure.TabStop = false;
			this.fraTreasure.Text = "Loot";
			// 
			// lstLoot
			// 
			this.lstLoot.Location = new System.Drawing.Point(16, 96);
			this.lstLoot.Name = "lstLoot";
			this.lstLoot.Size = new System.Drawing.Size(320, 147);
			this.lstLoot.TabIndex = 5;
			// 
			// cmdCharFive
			// 
			this.cmdCharFive.Location = new System.Drawing.Point(192, 56);
			this.cmdCharFive.Name = "cmdCharFive";
			this.cmdCharFive.Size = new System.Drawing.Size(88, 24);
			this.cmdCharFive.TabIndex = 4;
			this.cmdCharFive.Click += new System.EventHandler(this.cmdCharFive_Click);
			// 
			// cmdCharFour
			// 
			this.cmdCharFour.Location = new System.Drawing.Point(64, 56);
			this.cmdCharFour.Name = "cmdCharFour";
			this.cmdCharFour.Size = new System.Drawing.Size(88, 24);
			this.cmdCharFour.TabIndex = 3;
			this.cmdCharFour.Click += new System.EventHandler(this.cmdCharFour_Click);
			// 
			// cmdCharThree
			// 
			this.cmdCharThree.Location = new System.Drawing.Point(248, 24);
			this.cmdCharThree.Name = "cmdCharThree";
			this.cmdCharThree.Size = new System.Drawing.Size(88, 24);
			this.cmdCharThree.TabIndex = 2;
			this.cmdCharThree.Click += new System.EventHandler(this.cmdCharThree_Click);
			// 
			// cmdCharTwo
			// 
			this.cmdCharTwo.Location = new System.Drawing.Point(136, 24);
			this.cmdCharTwo.Name = "cmdCharTwo";
			this.cmdCharTwo.Size = new System.Drawing.Size(88, 24);
			this.cmdCharTwo.TabIndex = 1;
			this.cmdCharTwo.Click += new System.EventHandler(this.cmdCharTwo_Click);
			// 
			// cmdCharOne
			// 
			this.cmdCharOne.Location = new System.Drawing.Point(24, 24);
			this.cmdCharOne.Name = "cmdCharOne";
			this.cmdCharOne.Size = new System.Drawing.Size(88, 24);
			this.cmdCharOne.TabIndex = 0;
			this.cmdCharOne.Click += new System.EventHandler(this.cmdCharOne_Click);
			// 
			// fLoot
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(383, 376);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.fraTreasure,
																		  this.cmdOK,
																		  this.lblGold,
																		  this.label1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "fLoot";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Treasure!";
			this.fraTreasure.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void DistributeItem(int Order)
		{
			if(lstLoot.SelectedIndex<0)
			{
				MessageBox.Show("You must select an item to distribute.");
				return;
			}
			
			//Give item and associated experience points to character
			CItem oItem = (CItem)(lstLoot.Items[lstLoot.SelectedIndex]);
			CCharacter oDestChar = CEcalpon.gobjParty.GetCharacterByOrder(Order);
			oDestChar.Inventory.Add(oItem);
			oDestChar.Experience += oItem.ExperienceValue;

			//remove from list 
			lstLoot.Items.RemoveAt(lstLoot.SelectedIndex);
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			//award gold and associated gold experience (1 exp per 1 gp)
			foreach(CCharacter oCharacter in CEcalpon.gobjParty.Characters)
			{
				oCharacter.Gold += (int)(oLoot.Gold / (CEcalpon.gobjParty.Characters.Count));
				oCharacter.Experience += (int)(oLoot.Gold / (CEcalpon.gobjParty.Characters.Count));
			}

			this.Close();
		}

		//great opportunity for a control array
		private void cmdCharOne_Click(object sender, System.EventArgs e)
		{
			DistributeItem(1);
		}

		private void cmdCharTwo_Click(object sender, System.EventArgs e)
		{
			DistributeItem(2);
		}

		private void cmdCharThree_Click(object sender, System.EventArgs e)
		{
			DistributeItem(3);
		}

		private void cmdCharFour_Click(object sender, System.EventArgs e)
		{
			DistributeItem(4);
		}

		private void cmdCharFive_Click(object sender, System.EventArgs e)
		{
			DistributeItem(5);
		}
	}
}
