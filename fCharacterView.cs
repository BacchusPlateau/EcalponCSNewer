using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Ecalpon
{
	/// <summary>
	/// Summary description for fCharacterView.
	/// </summary>
	public class fCharacterView : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox grpChar;
		private System.Windows.Forms.GroupBox grpAttributes;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lblStrength;
		private System.Windows.Forms.Label lblIntelligence;
		private System.Windows.Forms.Label lblConstitution;
		private System.Windows.Forms.Label lblDexterity;
		private System.Windows.Forms.Label lblWisdom;
		private System.Windows.Forms.Label lblCharisma;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.ListBox lstInventory;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblLevel;
		private System.Windows.Forms.Label lblClass;
		private System.Windows.Forms.Label lblExperience;
		private System.Windows.Forms.Label lblGold;
		private System.Windows.Forms.Label lblAC;
		private System.Windows.Forms.Label lblHP;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label lblWeapon;
		private System.Windows.Forms.Button cmdRoll;
		private System.Windows.Forms.Button cmdExchange;
		private System.Windows.Forms.Button cmdUse;
		private System.Windows.Forms.Button cmdDrop;
		private System.Windows.Forms.Button cmdSpells;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private CCharacter oCharacter = null;
		private bool bAutoClose = false;

		public bool AutoCloseCharacterView 
		{
			get 
			{
				return bAutoClose;
			}
			set 
			{
				bAutoClose = value;
			}
		}

		public CCharacter Character 
		{
			get 
			{
				return oCharacter;
			}
			set 
			{
				oCharacter = value;
				LoadCharacterData();
			}
		}

		private void LoadCharacterData() 
		{
			this.lblAC.Text = oCharacter.ArmorClass.ToString();
			this.lblCharisma.Text = oCharacter.Charisma.ToString();
			this.lblClass.Text = oCharacter.ClassName;
			this.lblConstitution.Text = oCharacter.Constitution.ToString();
			this.lblDexterity.Text = oCharacter.Dexterity.ToString();
			this.lblExperience.Text = oCharacter.Experience.ToString();
			this.lblGold.Text = oCharacter.Gold.ToString();
			this.lblHP.Text = oCharacter.HitPoints.ToString() + " (" + oCharacter.MaxHitPoints.ToString() + ")";
			this.lblIntelligence.Text = oCharacter.Intelligence.ToString();
			this.lblLevel.Text = oCharacter.Level.ToString();
			this.lblName.Text = oCharacter.Name;
			this.lblStrength.Text = oCharacter.Strength.ToString();
			this.lblWeapon.Text = oCharacter.WeaponName;
			this.lblWisdom.Text = oCharacter.Wisdom.ToString();

			//Load inventory
			this.lstInventory.Items.Clear();

			foreach(CItem oItem in oCharacter.Inventory) 
			{
				this.lstInventory.Items.Add(oItem);
			}
			this.lstInventory.DisplayMember = "Name";

			this.cmdRoll.Visible = false;

			if(oCharacter.ClassID == (int) CCharacter.eClass.Cleric || 
				oCharacter.ClassID == (int) CCharacter.eClass.Wizard)
				this.cmdSpells.Visible = true;
			else
				this.cmdSpells.Visible = false;
		}


		public fCharacterView()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(fCharacterView));
			this.grpChar = new System.Windows.Forms.GroupBox();
			this.cmdSpells = new System.Windows.Forms.Button();
			this.cmdDrop = new System.Windows.Forms.Button();
			this.cmdUse = new System.Windows.Forms.Button();
			this.cmdExchange = new System.Windows.Forms.Button();
			this.lblWeapon = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.lblHP = new System.Windows.Forms.Label();
			this.lblAC = new System.Windows.Forms.Label();
			this.lblGold = new System.Windows.Forms.Label();
			this.lblExperience = new System.Windows.Forms.Label();
			this.lblClass = new System.Windows.Forms.Label();
			this.lblLevel = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.lstInventory = new System.Windows.Forms.ListBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.grpAttributes = new System.Windows.Forms.GroupBox();
			this.lblCharisma = new System.Windows.Forms.Label();
			this.lblWisdom = new System.Windows.Forms.Label();
			this.lblDexterity = new System.Windows.Forms.Label();
			this.lblConstitution = new System.Windows.Forms.Label();
			this.lblIntelligence = new System.Windows.Forms.Label();
			this.lblStrength = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdRoll = new System.Windows.Forms.Button();
			this.grpChar.SuspendLayout();
			this.grpAttributes.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpChar
			// 
			this.grpChar.Controls.AddRange(new System.Windows.Forms.Control[] {
																				  this.cmdSpells,
																				  this.cmdDrop,
																				  this.cmdUse,
																				  this.cmdExchange,
																				  this.lblWeapon,
																				  this.label15,
																				  this.lblHP,
																				  this.lblAC,
																				  this.lblGold,
																				  this.lblExperience,
																				  this.lblClass,
																				  this.lblLevel,
																				  this.lblName,
																				  this.label14,
																				  this.lstInventory,
																				  this.label13,
																				  this.label12,
																				  this.label11,
																				  this.label10,
																				  this.label9,
																				  this.label8,
																				  this.label7,
																				  this.grpAttributes});
			this.grpChar.Location = new System.Drawing.Point(8, 8);
			this.grpChar.Name = "grpChar";
			this.grpChar.Size = new System.Drawing.Size(360, 408);
			this.grpChar.TabIndex = 0;
			this.grpChar.TabStop = false;
			// 
			// cmdSpells
			// 
			this.cmdSpells.Image = ((System.Drawing.Bitmap)(resources.GetObject("cmdSpells.Image")));
			this.cmdSpells.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdSpells.Location = new System.Drawing.Point(280, 224);
			this.cmdSpells.Name = "cmdSpells";
			this.cmdSpells.Size = new System.Drawing.Size(64, 32);
			this.cmdSpells.TabIndex = 22;
			this.cmdSpells.Text = "Spells";
			this.cmdSpells.Click += new System.EventHandler(this.cmdSpells_Click);
			// 
			// cmdDrop
			// 
			this.cmdDrop.Location = new System.Drawing.Point(280, 304);
			this.cmdDrop.Name = "cmdDrop";
			this.cmdDrop.Size = new System.Drawing.Size(64, 32);
			this.cmdDrop.TabIndex = 21;
			this.cmdDrop.Text = "Drop";
			this.cmdDrop.Click += new System.EventHandler(this.cmdDrop_Click);
			// 
			// cmdUse
			// 
			this.cmdUse.Location = new System.Drawing.Point(280, 344);
			this.cmdUse.Name = "cmdUse";
			this.cmdUse.Size = new System.Drawing.Size(64, 32);
			this.cmdUse.TabIndex = 20;
			this.cmdUse.Text = "Use";
			this.cmdUse.Click += new System.EventHandler(this.cmdUse_Click);
			// 
			// cmdExchange
			// 
			this.cmdExchange.Location = new System.Drawing.Point(280, 264);
			this.cmdExchange.Name = "cmdExchange";
			this.cmdExchange.Size = new System.Drawing.Size(64, 32);
			this.cmdExchange.TabIndex = 19;
			this.cmdExchange.Text = "Exchange";
			this.cmdExchange.Click += new System.EventHandler(this.cmdExchange_Click);
			// 
			// lblWeapon
			// 
			this.lblWeapon.Location = new System.Drawing.Point(88, 384);
			this.lblWeapon.Name = "lblWeapon";
			this.lblWeapon.Size = new System.Drawing.Size(120, 16);
			this.lblWeapon.TabIndex = 18;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(8, 384);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(56, 16);
			this.label15.TabIndex = 17;
			this.label15.Text = "Weapon";
			// 
			// lblHP
			// 
			this.lblHP.Location = new System.Drawing.Point(88, 216);
			this.lblHP.Name = "lblHP";
			this.lblHP.Size = new System.Drawing.Size(96, 16);
			this.lblHP.TabIndex = 16;
			// 
			// lblAC
			// 
			this.lblAC.Location = new System.Drawing.Point(88, 176);
			this.lblAC.Name = "lblAC";
			this.lblAC.Size = new System.Drawing.Size(96, 16);
			this.lblAC.TabIndex = 15;
			// 
			// lblGold
			// 
			this.lblGold.Location = new System.Drawing.Point(88, 144);
			this.lblGold.Name = "lblGold";
			this.lblGold.Size = new System.Drawing.Size(96, 16);
			this.lblGold.TabIndex = 14;
			// 
			// lblExperience
			// 
			this.lblExperience.Location = new System.Drawing.Point(88, 120);
			this.lblExperience.Name = "lblExperience";
			this.lblExperience.Size = new System.Drawing.Size(96, 16);
			this.lblExperience.TabIndex = 13;
			// 
			// lblClass
			// 
			this.lblClass.Location = new System.Drawing.Point(88, 88);
			this.lblClass.Name = "lblClass";
			this.lblClass.Size = new System.Drawing.Size(96, 16);
			this.lblClass.TabIndex = 12;
			// 
			// lblLevel
			// 
			this.lblLevel.Location = new System.Drawing.Point(88, 56);
			this.lblLevel.Name = "lblLevel";
			this.lblLevel.Size = new System.Drawing.Size(96, 16);
			this.lblLevel.TabIndex = 11;
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(88, 24);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(96, 16);
			this.lblName.TabIndex = 10;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(8, 248);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(88, 16);
			this.label14.TabIndex = 9;
			this.label14.Text = "Inventory";
			// 
			// lstInventory
			// 
			this.lstInventory.Location = new System.Drawing.Point(16, 264);
			this.lstInventory.Name = "lstInventory";
			this.lstInventory.Size = new System.Drawing.Size(248, 108);
			this.lstInventory.TabIndex = 8;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(8, 216);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(56, 16);
			this.label13.TabIndex = 7;
			this.label13.Text = "HP (Max)";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(8, 184);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(24, 16);
			this.label12.TabIndex = 6;
			this.label12.Text = "AC";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(8, 152);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(32, 16);
			this.label11.TabIndex = 5;
			this.label11.Text = "Gold";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 120);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(64, 16);
			this.label10.TabIndex = 4;
			this.label10.Text = "Experience";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 88);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(40, 16);
			this.label9.TabIndex = 3;
			this.label9.Text = "Class";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 56);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(40, 16);
			this.label8.TabIndex = 2;
			this.label8.Text = "Level";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 24);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(40, 16);
			this.label7.TabIndex = 1;
			this.label7.Text = "Name";
			// 
			// grpAttributes
			// 
			this.grpAttributes.Controls.AddRange(new System.Windows.Forms.Control[] {
																						this.lblCharisma,
																						this.lblWisdom,
																						this.lblDexterity,
																						this.lblConstitution,
																						this.lblIntelligence,
																						this.lblStrength,
																						this.label6,
																						this.label5,
																						this.label4,
																						this.label3,
																						this.label2,
																						this.label1});
			this.grpAttributes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.grpAttributes.Location = new System.Drawing.Point(208, 32);
			this.grpAttributes.Name = "grpAttributes";
			this.grpAttributes.Size = new System.Drawing.Size(128, 176);
			this.grpAttributes.TabIndex = 0;
			this.grpAttributes.TabStop = false;
			this.grpAttributes.Text = "Attributes";
			// 
			// lblCharisma
			// 
			this.lblCharisma.Location = new System.Drawing.Point(72, 144);
			this.lblCharisma.Name = "lblCharisma";
			this.lblCharisma.Size = new System.Drawing.Size(48, 16);
			this.lblCharisma.TabIndex = 11;
			this.lblCharisma.Text = "0";
			// 
			// lblWisdom
			// 
			this.lblWisdom.Location = new System.Drawing.Point(72, 120);
			this.lblWisdom.Name = "lblWisdom";
			this.lblWisdom.Size = new System.Drawing.Size(48, 16);
			this.lblWisdom.TabIndex = 10;
			this.lblWisdom.Text = "0";
			// 
			// lblDexterity
			// 
			this.lblDexterity.Location = new System.Drawing.Point(72, 96);
			this.lblDexterity.Name = "lblDexterity";
			this.lblDexterity.Size = new System.Drawing.Size(48, 16);
			this.lblDexterity.TabIndex = 9;
			this.lblDexterity.Text = "0";
			// 
			// lblConstitution
			// 
			this.lblConstitution.Location = new System.Drawing.Point(72, 72);
			this.lblConstitution.Name = "lblConstitution";
			this.lblConstitution.Size = new System.Drawing.Size(48, 16);
			this.lblConstitution.TabIndex = 8;
			this.lblConstitution.Text = "0";
			// 
			// lblIntelligence
			// 
			this.lblIntelligence.Location = new System.Drawing.Point(72, 48);
			this.lblIntelligence.Name = "lblIntelligence";
			this.lblIntelligence.Size = new System.Drawing.Size(48, 16);
			this.lblIntelligence.TabIndex = 7;
			this.lblIntelligence.Text = "0";
			// 
			// lblStrength
			// 
			this.lblStrength.Location = new System.Drawing.Point(72, 24);
			this.lblStrength.Name = "lblStrength";
			this.lblStrength.Size = new System.Drawing.Size(48, 16);
			this.lblStrength.TabIndex = 6;
			this.lblStrength.Text = "0";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 144);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(56, 16);
			this.label6.TabIndex = 5;
			this.label6.Text = "Charisma";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 120);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 16);
			this.label5.TabIndex = 4;
			this.label5.Text = "Wisdom";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 96);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "Dexterity";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Constitution";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Intelligence";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Strength";
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point(8, 424);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(56, 24);
			this.cmdOK.TabIndex = 1;
			this.cmdOK.Text = "&Ok";
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// cmdRoll
			// 
			this.cmdRoll.Location = new System.Drawing.Point(144, 424);
			this.cmdRoll.Name = "cmdRoll";
			this.cmdRoll.Size = new System.Drawing.Size(80, 24);
			this.cmdRoll.TabIndex = 2;
			this.cmdRoll.Text = "Roll Again";
			this.cmdRoll.Visible = false;
			this.cmdRoll.Click += new System.EventHandler(this.cmdRoll_Click);
			// 
			// fCharacterView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(378, 461);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.cmdRoll,
																		  this.cmdOK,
																		  this.grpChar});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "fCharacterView";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Character Maintenance";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.fCharacterView_Closing);
			this.grpChar.ResumeLayout(false);
			this.grpAttributes.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// releases resources when the form closes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void fCharacterView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			oCharacter = null;
		}

		/// <summary>
		/// Close the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			//Save changed values
			this.Close();
		}


		/// <summary>
		/// Exchange an item from one character to another
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdExchange_Click(object sender, System.EventArgs e)
		{
			if(ItemSelected()) 
			{
				CItem oItem = null;
				int iDestChar = -1;

				oItem = (CItem)(lstInventory.Items[lstInventory.SelectedIndex]);
				if(oItem.InUse) 
				{
					MessageBox.Show("Item is in use. Unuse item before exchanging.");
				}
				else
				{
					fInputBox InputBox = new fInputBox("Move Item", "Give to who? (1-5)");
					InputBox.ShowDialog();

					string Result = InputBox.Response;
					InputBox.Close();
					InputBox = null;

					try 
					{
						iDestChar = System.Convert.ToInt32(Result);
					}
					catch
					{
						MessageBox.Show("Could not convert your entry.");
					} 

					if(iDestChar>0 && iDestChar<6) 
					{
						CCharacter oDestChar = CEcalpon.gobjParty.GetCharacterByOrder(iDestChar);
						oCharacter.Inventory.Remove(oItem.InternalIndex);
						oDestChar.Inventory.Add(oItem);
						LoadCharacterData();
					}
					else
					{
						if(iDestChar!=-1)
							MessageBox.Show("Character must be (1-5)");
					}
					
				}

			}
		}

		/// <summary>
		/// Discards an item from the inventory
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdDrop_Click(object sender, System.EventArgs e)
		{
			CItem oItem = null;

			if(ItemSelected()) 
			{
				oItem = (CItem)(lstInventory.Items[lstInventory.SelectedIndex]);
				if(oItem.InUse) 
				{
					MessageBox.Show("Item is in use. Unuse item before dropping.");
				}
				else 
				{
					oCharacter.Inventory.Remove(oItem.InternalIndex);
					LoadCharacterData();
				}
			}
		}

		/// <summary>
		/// "Uses" an item in the character's inventory
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdUse_Click(object sender, System.EventArgs e)
		{
			CItem oItem = null;
			bool bClassCheck = true;
			bool bUseFail = false;

			if(ItemSelected()) 
			{
				oItem = (CItem)(lstInventory.Items[lstInventory.SelectedIndex]);

				//Use Item
				if(oItem.ItemCharacterClass >= 0) 
				{
					if(oItem.ItemCharacterClass != oCharacter.ClassID) 
					{
						MessageBox.Show("Wrong class.  You cannot use this item.");
						bClassCheck = false;
					}
				}

				if(bClassCheck) 
				{

					switch(oItem.ItemType) 
					{
						case 0: //weapon
							if(oItem.InUse) 
							{
								oCharacter.WeaponID = 99;  //empty
							}
							else 
							{
								if(oCharacter.WeaponID == 99) 
								{
									oCharacter.WeaponID = oItem.ItemTypeID;
								}
								else 
								{
									MessageBox.Show("Unuse current weapon before using new weapon.");
									bUseFail = true;
								}	
							}
							break;
						case 1: //armor
							
							break;
						case 2: //sundry
						switch(oItem.Category) 
						{
							case "NA":
							switch(oItem.ItemTypeID) 
							{
								case 3:  //Gem
									CEcalpon.UsingGem = true;
									if(CEcalpon.InEncounter)
										CEcalpon.GemUseCount = 10;
									else 
										CEcalpon.GemUseCount = 5;
									CEcalpon.ShowMessage("You peer into a gem...",2);
									oCharacter.Inventory.Remove(oItem.InternalIndex);
									AutoCloseCharacterView = true;
									bUseFail = true;
									break;
							}
								break;
							case "ST":
								if(oItem.InUse) 
									oCharacter.Strength -= oItem.Rating;
								else
									oCharacter.Strength += oItem.Rating;
								break;
							case "HP":
								int iNewHP = CEcalpon.MyRand(oItem.Rating, (int) (oItem.Rating / 2));
								if(iNewHP + oCharacter.HitPoints > oCharacter.MaxHitPoints)
									oCharacter.HitPoints = oCharacter.MaxHitPoints;
								else
									oCharacter.HitPoints += iNewHP;
								oCharacter.Inventory.Remove(oItem.InternalIndex);
								break;
						}
							break;
						case 3: //special;
							break;
					}
			
					if(!bUseFail) 
					{
						oItem.InUse = ! oItem.InUse;
						if(oItem.ItemType==1) 
						{
							oCharacter.ComputeAC();
						}
					}
				}

				if(AutoCloseCharacterView)
					this.Close();
				else
					LoadCharacterData();
			}
		}

		/// <summary>
		/// Returns whether or not an item is selected in the listbox
		/// </summary>
		/// <returns></returns>
		private bool ItemSelected() 
		{
			bool bOK = true;

			if(lstInventory.SelectedIndex < 0) 
			{
				MessageBox.Show("You must first select an item.");
				bOK = false;
			}
			
			return bOK;
		}

		/// <summary>
		/// display spells each user owns
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdSpells_Click(object sender, System.EventArgs e)
		{
			//add code to make spells read-only here.  you should only be able
			//to get spells via camping
			fSpellMaint Spells = new fSpellMaint();
			Spells.Character = oCharacter;
			Spells.ShowDialog();
		}

		/// <summary>
		/// Randomly select user attributes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdRoll_Click(object sender, System.EventArgs e)
		{
		
		}

		

		
	}
}
