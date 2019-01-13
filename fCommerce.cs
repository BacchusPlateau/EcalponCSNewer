using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Ecalpon
{
	/// <summary>
	/// Summary description for fCommerce.
	/// </summary>
	public class fCommerce : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox fraItem;
		private System.Windows.Forms.GroupBox fraWho;
		private System.Windows.Forms.ListBox lstItems;
		private System.Windows.Forms.ListBox lstWho;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblGold;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Button cmdBuySell;
		private System.Windows.Forms.Button cmdQuit;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private string sCategory;
		private int iTreasureType;
		private string sBuySell;
		private CLoot oLoot=null;
		private int iGoldPool=0;

		public fCommerce(string Category, int TreasureType, string BuySell)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			sCategory = Category;
			iTreasureType = TreasureType;
			sBuySell = BuySell;

			//Display Characters
			lstWho.DisplayMember = "Name";
			foreach(CCharacter oCharacter in CEcalpon.gobjParty.Characters) 
			{
				lstWho.Items.Add(oCharacter);
				iGoldPool += oCharacter.Gold;
			}
			
			//default the box to the first one inserted
			lstWho.SelectedIndex = 0;
			lstWho.Select();

			//display how much gold the characters have
			lblGold.Text = iGoldPool.ToString();

			this.Text = sCategory + " Store";
			if(sBuySell=="Buy") 
			{
				cmdBuySell.Text = "Buy";
				fraItem.Text = "Item / Price";
				LoadItems();
			}
			else
			{
				cmdBuySell.Text = "Sell";
				fraItem.Text = "Inventory / Value";
				LoadCurrentCharItems();
			}
			 
		}


		private void LoadItems()
		{

			oLoot = new CLoot(iTreasureType);
			lstItems.DisplayMember = "NamePrice";
			foreach(CItem oItem in oLoot.Items)
				lstItems.Items.Add(oItem);

			if(lstItems.Items.Count > 0)
			{
				lstItems.SelectedIndex = 0;
				lstItems.Select();
			}

		}

		private void LoadCurrentCharItems() 
		{
			CCharacter oTargetChar = (CCharacter)(lstWho.Items[lstWho.SelectedIndex]);

			lstItems.DisplayMember = "NamePrice";
			foreach(CItem oItem in oTargetChar.Inventory)
				lstItems.Items.Add(oItem);

			if(lstItems.Items.Count > 0)
			{
				lstItems.SelectedIndex = 0;
				lstItems.Select();
			}

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(fCommerce));
			this.fraItem = new System.Windows.Forms.GroupBox();
			this.lstItems = new System.Windows.Forms.ListBox();
			this.fraWho = new System.Windows.Forms.GroupBox();
			this.lblGold = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lstWho = new System.Windows.Forms.ListBox();
			this.lblStatus = new System.Windows.Forms.Label();
			this.cmdBuySell = new System.Windows.Forms.Button();
			this.cmdQuit = new System.Windows.Forms.Button();
			this.fraItem.SuspendLayout();
			this.fraWho.SuspendLayout();
			this.SuspendLayout();
			// 
			// fraItem
			// 
			this.fraItem.Controls.AddRange(new System.Windows.Forms.Control[] {
																				  this.lstItems});
			this.fraItem.Location = new System.Drawing.Point(16, 16);
			this.fraItem.Name = "fraItem";
			this.fraItem.Size = new System.Drawing.Size(224, 216);
			this.fraItem.TabIndex = 0;
			this.fraItem.TabStop = false;
			this.fraItem.Text = "Items";
			// 
			// lstItems
			// 
			this.lstItems.Location = new System.Drawing.Point(8, 16);
			this.lstItems.Name = "lstItems";
			this.lstItems.Size = new System.Drawing.Size(208, 186);
			this.lstItems.TabIndex = 0;
			// 
			// fraWho
			// 
			this.fraWho.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.lblGold,
																				 this.label1,
																				 this.lstWho});
			this.fraWho.Location = new System.Drawing.Point(272, 16);
			this.fraWho.Name = "fraWho";
			this.fraWho.Size = new System.Drawing.Size(224, 256);
			this.fraWho.TabIndex = 1;
			this.fraWho.TabStop = false;
			this.fraWho.Text = "Characters";
			// 
			// lblGold
			// 
			this.lblGold.Location = new System.Drawing.Point(72, 224);
			this.lblGold.Name = "lblGold";
			this.lblGold.Size = new System.Drawing.Size(104, 16);
			this.lblGold.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 224);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Gold:";
			// 
			// lstWho
			// 
			this.lstWho.Location = new System.Drawing.Point(8, 16);
			this.lstWho.Name = "lstWho";
			this.lstWho.Size = new System.Drawing.Size(208, 186);
			this.lstWho.TabIndex = 0;
			this.lstWho.SelectedIndexChanged += new System.EventHandler(this.lstWho_SelectedIndexChanged);
			// 
			// lblStatus
			// 
			this.lblStatus.Location = new System.Drawing.Point(8, 288);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(272, 40);
			this.lblStatus.TabIndex = 2;
			// 
			// cmdBuySell
			// 
			this.cmdBuySell.Location = new System.Drawing.Point(312, 296);
			this.cmdBuySell.Name = "cmdBuySell";
			this.cmdBuySell.Size = new System.Drawing.Size(56, 32);
			this.cmdBuySell.TabIndex = 3;
			this.cmdBuySell.Click += new System.EventHandler(this.cmdBuySell_Click);
			// 
			// cmdQuit
			// 
			this.cmdQuit.Location = new System.Drawing.Point(424, 296);
			this.cmdQuit.Name = "cmdQuit";
			this.cmdQuit.Size = new System.Drawing.Size(56, 32);
			this.cmdQuit.TabIndex = 4;
			this.cmdQuit.Text = "&OK";
			this.cmdQuit.Click += new System.EventHandler(this.cmdQuit_Click);
			// 
			// fCommerce
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(510, 340);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.cmdQuit,
																		  this.cmdBuySell,
																		  this.lblStatus,
																		  this.fraWho,
																		  this.fraItem});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "fCommerce";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.fCommerce_Closing);
			this.fraItem.ResumeLayout(false);
			this.fraWho.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void fCommerce_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			foreach(CCharacter oCharacter in CEcalpon.gobjParty.Characters)
				oCharacter.Gold = (int)iGoldPool/5;
		}

		private void cmdQuit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void lstWho_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(sBuySell=="Sell")
				LoadCurrentCharItems();
		}

		private void cmdBuySell_Click(object sender, System.EventArgs e)
		{

			CCharacter oTargetChar = (CCharacter)(lstWho.Items[lstWho.SelectedIndex]);
			CItem oItem = (CItem)(lstItems.Items[lstItems.SelectedIndex]);

			if(sBuySell=="Buy")
			{
				iGoldPool -= oItem.Price;
				oTargetChar.Inventory.Add(oItem);
				lblStatus.Text = "Bought " + oItem.Name;
			}
			else
			{
				iGoldPool += oItem.Price;
				oTargetChar.Inventory.Remove(oItem.InternalIndex);
				lblStatus.Text = "Sold " + oItem.Name;
			}

			//remove item from listbox
			lstItems.Items.RemoveAt(lstItems.SelectedIndex);
		}
	}
}
