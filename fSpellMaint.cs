using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;

namespace Ecalpon
{
	/// <summary>
	/// Summary description for fSpellMaint.
	/// </summary>
	public class fSpellMaint : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox lstAvailable;
		private System.Windows.Forms.ListBox lstMemorized;
		private System.Windows.Forms.Button cmdAdd;
		private System.Windows.Forms.Button cmdDrop;
		private System.Windows.Forms.Button cmdOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label lblOne;
		private System.Windows.Forms.Label lblTwo;
		private System.Windows.Forms.Label lblThree;
		private System.Windows.Forms.Label lblFour;
		private System.Windows.Forms.Label lblFive;
		private System.Windows.Forms.Label lblSix;
		private System.Windows.Forms.Label lblSeven;
		private System.Windows.Forms.Label lblEight;
		private System.Windows.Forms.Label lblNine;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label lblLeft1;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblClass;
		private System.Windows.Forms.Label lblLevel;
		private System.Windows.Forms.Label lblLeft2;
		private System.Windows.Forms.Label lblLeft3;
		private System.Windows.Forms.Label lblLeft4;
		private System.Windows.Forms.Label lblLeft5;
		private System.Windows.Forms.Label lblLeft6;
		private System.Windows.Forms.Label lblLeft7;
		private System.Windows.Forms.Label lblLeft8;
		private System.Windows.Forms.Label lblLeft9;

		private CCharacter oCharacter = null;
		private bool bReadOnly = true;

		//Write-only property ReadOnly (oh, the irony!)
		public bool ReadOnly 
		{
			set
			{
				bReadOnly = value;
				cmdAdd.Enabled = false;
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
				LoadSpells();
				SetSpellCap();
				SetSpellsLeft();
				lblName.Text = oCharacter.Name;
				lblClass.Text = oCharacter.ClassName;
				lblLevel.Text = oCharacter.Level.ToString();
			}
		}

		private void LoadSpells() 
		{
			lstMemorized.Items.Clear();
			
			foreach(CSpell oSpell in oCharacter.Spells) 
			{
				lstMemorized.Items.Add(oSpell);
			}
			
			lstMemorized.DisplayMember = "NameDisplay";
			lstMemorized.Refresh();

			lstAvailable.Items.Clear();

			CSpells oAvailableSpells = new CSpells();
			oAvailableSpells.LoadAvailableSpells(oCharacter.ClassID, oCharacter.Level);

			foreach(CSpell oSpell in oAvailableSpells) 
			{
				lstAvailable.Items.Add(oSpell);
			}

			lstAvailable.DisplayMember = "NameAvailable";							
			lstAvailable.Refresh();
		}

		private void SetSpellsLeft() 
		{
			int[] left = new int[9];
			
			try 
			{
				left[0]=System.Convert.ToInt32(lblOne.Text);
				left[1]=System.Convert.ToInt32(lblTwo.Text);
				left[2]=System.Convert.ToInt32(lblThree.Text);
				left[3]=System.Convert.ToInt32(lblFour.Text);
				left[4]=System.Convert.ToInt32(lblFive.Text);
				left[5]=System.Convert.ToInt32(lblSix.Text);
				left[6]=System.Convert.ToInt32(lblSeven.Text);
				left[7]=System.Convert.ToInt32(lblEight.Text);
				left[8]=System.Convert.ToInt32(lblNine.Text);

				foreach(CSpell oSpell in oCharacter.Spells) 
					left[oSpell.Level-1] -=  oSpell.Count;
			
				lblLeft1.Text = left[0].ToString();
				lblLeft2.Text = left[1].ToString();
				lblLeft3.Text = left[2].ToString();
				lblLeft4.Text = left[3].ToString();
				lblLeft5.Text = left[4].ToString();
				lblLeft6.Text = left[5].ToString();
				lblLeft7.Text = left[6].ToString();
				lblLeft8.Text = left[7].ToString();
				lblLeft9.Text = left[8].ToString();
			}
			catch (Exception e) 
			{
				MessageBox.Show(e.Message);
			}
		}



		private void SetSpellCap() 
		{
			string sSQL;
			OleDbDataReader drReader;
			CDataAccess oDataAccess = new CDataAccess();

			sSQL = "SELECT * FROM ";
			if(oCharacter.ClassID == (int) CCharacter.eClass.Wizard)
				sSQL += "MagicUserSpellCap ";
			else
				sSQL += "ClericSpellCap ";
			sSQL += "WHERE Level = " + oCharacter.Level.ToString();

			try 
			{
				oDataAccess.FillDataReader(out drReader, sSQL);

				drReader.Read();
				
				lblOne.Text = drReader["1"].ToString();
				lblTwo.Text = drReader["2"].ToString();
				lblThree.Text = drReader["3"].ToString();
				lblFour.Text = drReader["4"].ToString();
				lblFive.Text = drReader["5"].ToString();
				lblSix.Text = drReader["6"].ToString();
				lblSeven.Text = drReader["7"].ToString();
				if(oCharacter.ClassID == (int) CCharacter.eClass.Wizard) 
				{
					lblEight.Text = drReader["8"].ToString();
					lblNine.Text = drReader["9"].ToString();
				}
				else 
				{
					lblEight.Enabled = false;
					lblNine.Enabled = false;
					lblLeft8.Enabled = false;
					lblLeft9.Enabled = false;
				}
				

				drReader.Close();
				oDataAccess = null;
			}
			
			catch(Exception e)
			{
				MessageBox.Show(e.ToString());
			}



		}
		public fSpellMaint()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(fSpellMaint));
			this.lstAvailable = new System.Windows.Forms.ListBox();
			this.lstMemorized = new System.Windows.Forms.ListBox();
			this.cmdAdd = new System.Windows.Forms.Button();
			this.cmdDrop = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblLeft9 = new System.Windows.Forms.Label();
			this.lblLeft8 = new System.Windows.Forms.Label();
			this.lblLeft7 = new System.Windows.Forms.Label();
			this.lblLeft6 = new System.Windows.Forms.Label();
			this.lblLeft5 = new System.Windows.Forms.Label();
			this.lblLeft4 = new System.Windows.Forms.Label();
			this.lblLeft3 = new System.Windows.Forms.Label();
			this.lblLeft2 = new System.Windows.Forms.Label();
			this.lblLeft1 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.lblNine = new System.Windows.Forms.Label();
			this.lblEight = new System.Windows.Forms.Label();
			this.lblSeven = new System.Windows.Forms.Label();
			this.lblSix = new System.Windows.Forms.Label();
			this.lblFive = new System.Windows.Forms.Label();
			this.lblFour = new System.Windows.Forms.Label();
			this.lblThree = new System.Windows.Forms.Label();
			this.lblTwo = new System.Windows.Forms.Label();
			this.lblOne = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.lblClass = new System.Windows.Forms.Label();
			this.lblLevel = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lstAvailable
			// 
			this.lstAvailable.Location = new System.Drawing.Point(16, 16);
			this.lstAvailable.Name = "lstAvailable";
			this.lstAvailable.Size = new System.Drawing.Size(208, 199);
			this.lstAvailable.TabIndex = 0;
			// 
			// lstMemorized
			// 
			this.lstMemorized.Location = new System.Drawing.Point(288, 16);
			this.lstMemorized.Name = "lstMemorized";
			this.lstMemorized.Size = new System.Drawing.Size(208, 199);
			this.lstMemorized.TabIndex = 1;
			// 
			// cmdAdd
			// 
			this.cmdAdd.Location = new System.Drawing.Point(240, 96);
			this.cmdAdd.Name = "cmdAdd";
			this.cmdAdd.Size = new System.Drawing.Size(32, 24);
			this.cmdAdd.TabIndex = 2;
			this.cmdAdd.Text = ">";
			this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
			// 
			// cmdDrop
			// 
			this.cmdDrop.Location = new System.Drawing.Point(440, 232);
			this.cmdDrop.Name = "cmdDrop";
			this.cmdDrop.Size = new System.Drawing.Size(56, 24);
			this.cmdDrop.TabIndex = 3;
			this.cmdDrop.Text = "Drop";
			this.cmdDrop.Click += new System.EventHandler(this.cmdDrop_Click);
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point(440, 352);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(56, 24);
			this.cmdOK.TabIndex = 4;
			this.cmdOK.Text = "&OK";
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.lblLeft9,
																					this.lblLeft8,
																					this.lblLeft7,
																					this.lblLeft6,
																					this.lblLeft5,
																					this.lblLeft4,
																					this.lblLeft3,
																					this.lblLeft2,
																					this.lblLeft1,
																					this.label12,
																					this.lblNine,
																					this.lblEight,
																					this.lblSeven,
																					this.lblSix,
																					this.lblFive,
																					this.lblFour,
																					this.lblThree,
																					this.lblTwo,
																					this.lblOne,
																					this.label11,
																					this.label10,
																					this.label9,
																					this.label8,
																					this.label7,
																					this.label6,
																					this.label5,
																					this.label4,
																					this.label3,
																					this.label2,
																					this.label1});
			this.groupBox1.Location = new System.Drawing.Point(16, 280);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(352, 96);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Capacity";
			// 
			// lblLeft9
			// 
			this.lblLeft9.Location = new System.Drawing.Point(320, 48);
			this.lblLeft9.Name = "lblLeft9";
			this.lblLeft9.Size = new System.Drawing.Size(24, 16);
			this.lblLeft9.TabIndex = 29;
			this.lblLeft9.Text = "0";
			// 
			// lblLeft8
			// 
			this.lblLeft8.Location = new System.Drawing.Point(288, 48);
			this.lblLeft8.Name = "lblLeft8";
			this.lblLeft8.Size = new System.Drawing.Size(24, 16);
			this.lblLeft8.TabIndex = 28;
			this.lblLeft8.Text = "0";
			// 
			// lblLeft7
			// 
			this.lblLeft7.Location = new System.Drawing.Point(256, 48);
			this.lblLeft7.Name = "lblLeft7";
			this.lblLeft7.Size = new System.Drawing.Size(24, 16);
			this.lblLeft7.TabIndex = 27;
			this.lblLeft7.Text = "0";
			// 
			// lblLeft6
			// 
			this.lblLeft6.Location = new System.Drawing.Point(224, 48);
			this.lblLeft6.Name = "lblLeft6";
			this.lblLeft6.Size = new System.Drawing.Size(24, 16);
			this.lblLeft6.TabIndex = 26;
			this.lblLeft6.Text = "0";
			// 
			// lblLeft5
			// 
			this.lblLeft5.Location = new System.Drawing.Point(192, 48);
			this.lblLeft5.Name = "lblLeft5";
			this.lblLeft5.Size = new System.Drawing.Size(24, 16);
			this.lblLeft5.TabIndex = 25;
			this.lblLeft5.Text = "0";
			// 
			// lblLeft4
			// 
			this.lblLeft4.Location = new System.Drawing.Point(160, 48);
			this.lblLeft4.Name = "lblLeft4";
			this.lblLeft4.Size = new System.Drawing.Size(24, 16);
			this.lblLeft4.TabIndex = 24;
			this.lblLeft4.Text = "0";
			// 
			// lblLeft3
			// 
			this.lblLeft3.Location = new System.Drawing.Point(128, 48);
			this.lblLeft3.Name = "lblLeft3";
			this.lblLeft3.Size = new System.Drawing.Size(24, 16);
			this.lblLeft3.TabIndex = 23;
			this.lblLeft3.Text = "0";
			// 
			// lblLeft2
			// 
			this.lblLeft2.Location = new System.Drawing.Point(96, 48);
			this.lblLeft2.Name = "lblLeft2";
			this.lblLeft2.Size = new System.Drawing.Size(24, 16);
			this.lblLeft2.TabIndex = 22;
			this.lblLeft2.Text = "0";
			// 
			// lblLeft1
			// 
			this.lblLeft1.Location = new System.Drawing.Point(64, 48);
			this.lblLeft1.Name = "lblLeft1";
			this.lblLeft1.Size = new System.Drawing.Size(24, 16);
			this.lblLeft1.TabIndex = 21;
			this.lblLeft1.Text = "0";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(8, 48);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(56, 16);
			this.label12.TabIndex = 20;
			this.label12.Text = "Available";
			// 
			// lblNine
			// 
			this.lblNine.Location = new System.Drawing.Point(320, 24);
			this.lblNine.Name = "lblNine";
			this.lblNine.Size = new System.Drawing.Size(24, 16);
			this.lblNine.TabIndex = 19;
			this.lblNine.Text = "0";
			// 
			// lblEight
			// 
			this.lblEight.Location = new System.Drawing.Point(288, 24);
			this.lblEight.Name = "lblEight";
			this.lblEight.Size = new System.Drawing.Size(24, 16);
			this.lblEight.TabIndex = 18;
			this.lblEight.Text = "0";
			// 
			// lblSeven
			// 
			this.lblSeven.Location = new System.Drawing.Point(256, 24);
			this.lblSeven.Name = "lblSeven";
			this.lblSeven.Size = new System.Drawing.Size(24, 16);
			this.lblSeven.TabIndex = 17;
			this.lblSeven.Text = "0";
			// 
			// lblSix
			// 
			this.lblSix.Location = new System.Drawing.Point(224, 24);
			this.lblSix.Name = "lblSix";
			this.lblSix.Size = new System.Drawing.Size(24, 16);
			this.lblSix.TabIndex = 16;
			this.lblSix.Text = "0";
			// 
			// lblFive
			// 
			this.lblFive.Location = new System.Drawing.Point(192, 24);
			this.lblFive.Name = "lblFive";
			this.lblFive.Size = new System.Drawing.Size(24, 16);
			this.lblFive.TabIndex = 15;
			this.lblFive.Text = "0";
			// 
			// lblFour
			// 
			this.lblFour.Location = new System.Drawing.Point(160, 24);
			this.lblFour.Name = "lblFour";
			this.lblFour.Size = new System.Drawing.Size(24, 16);
			this.lblFour.TabIndex = 14;
			this.lblFour.Text = "0";
			// 
			// lblThree
			// 
			this.lblThree.Location = new System.Drawing.Point(128, 24);
			this.lblThree.Name = "lblThree";
			this.lblThree.Size = new System.Drawing.Size(24, 16);
			this.lblThree.TabIndex = 13;
			this.lblThree.Text = "0";
			// 
			// lblTwo
			// 
			this.lblTwo.Location = new System.Drawing.Point(96, 24);
			this.lblTwo.Name = "lblTwo";
			this.lblTwo.Size = new System.Drawing.Size(24, 16);
			this.lblTwo.TabIndex = 12;
			this.lblTwo.Text = "0";
			// 
			// lblOne
			// 
			this.lblOne.Location = new System.Drawing.Point(64, 24);
			this.lblOne.Name = "lblOne";
			this.lblOne.Size = new System.Drawing.Size(24, 16);
			this.lblOne.TabIndex = 11;
			this.lblOne.Text = "0";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(8, 72);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(48, 16);
			this.label11.TabIndex = 10;
			this.label11.Text = "Level";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 24);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(56, 16);
			this.label10.TabIndex = 9;
			this.label10.Text = "Maximum";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(320, 72);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(16, 16);
			this.label9.TabIndex = 8;
			this.label9.Text = "9";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(288, 72);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(16, 16);
			this.label8.TabIndex = 7;
			this.label8.Text = "8";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(256, 72);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(16, 16);
			this.label7.TabIndex = 6;
			this.label7.Text = "7";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(224, 72);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(16, 16);
			this.label6.TabIndex = 5;
			this.label6.Text = "6";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(192, 72);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(16, 16);
			this.label5.TabIndex = 4;
			this.label5.Text = "5";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(160, 72);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(16, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "4";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(128, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(16, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "3";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(96, 72);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(16, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "2";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(64, 72);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(16, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "1";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(16, 224);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(40, 16);
			this.label13.TabIndex = 6;
			this.label13.Text = "Name";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(16, 248);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(40, 16);
			this.label14.TabIndex = 7;
			this.label14.Text = "Class";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(144, 248);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(40, 16);
			this.label15.TabIndex = 8;
			this.label15.Text = "Level";
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(64, 224);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(160, 16);
			this.lblName.TabIndex = 9;
			// 
			// lblClass
			// 
			this.lblClass.Location = new System.Drawing.Point(64, 248);
			this.lblClass.Name = "lblClass";
			this.lblClass.Size = new System.Drawing.Size(80, 16);
			this.lblClass.TabIndex = 10;
			// 
			// lblLevel
			// 
			this.lblLevel.Location = new System.Drawing.Point(192, 248);
			this.lblLevel.Name = "lblLevel";
			this.lblLevel.Size = new System.Drawing.Size(72, 16);
			this.lblLevel.TabIndex = 11;
			// 
			// fSpellMaint
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(512, 389);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lblLevel,
																		  this.lblClass,
																		  this.lblName,
																		  this.label15,
																		  this.label14,
																		  this.label13,
																		  this.groupBox1,
																		  this.cmdOK,
																		  this.cmdDrop,
																		  this.cmdAdd,
																		  this.lstMemorized,
																		  this.lstAvailable});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "fSpellMaint";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Spell Maintenance";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			oCharacter = null;
			this.Close();
		}

		private void cmdDrop_Click(object sender, System.EventArgs e)
		{
			if(lstMemorized.SelectedIndex < 0) 
			{
				MessageBox.Show("You must select a spell to drop.");
			}
			else 
			{
				CSpell oSpell = (CSpell)lstMemorized.Items[lstMemorized.SelectedIndex];
			
				oSpell.Count -= 1;
				if(oSpell.Count==0) 
					oCharacter.Spells.Delete(oSpell.InternalIndex);
				LoadSpells();
				SetSpellsLeft();
			}
		}

		private void cmdAdd_Click(object sender, System.EventArgs e)
		{
			if(lstAvailable.SelectedIndex < 0) 
			{
				MessageBox.Show("You must select a spell to add.");
			}
			else 
			{
				CSpell oSpell = (CSpell)lstAvailable.Items[lstAvailable.SelectedIndex];
				bool bAtCap = false;
				switch(oSpell.Level) 
				{
					case 1:
						if(System.Convert.ToInt32(lblLeft1.Text)<=0)
							bAtCap = true;
						break;
					case 2:
						if(System.Convert.ToInt32(lblLeft2.Text)<=0)
							bAtCap = true;
						break;
					case 3:
						if(System.Convert.ToInt32(lblLeft3.Text)<=0)
							bAtCap = true;
						break;
					case 4:
						if(System.Convert.ToInt32(lblLeft4.Text)<=0)
							bAtCap = true;
						break;
					case 5:
						if(System.Convert.ToInt32(lblLeft5.Text)<=0)
							bAtCap = true;
						break;
					case 6:
						if(System.Convert.ToInt32(lblLeft6.Text)<=0)
							bAtCap = true;
						break;
					case 7:
						if(System.Convert.ToInt32(lblLeft7.Text)<=0)
							bAtCap = true;
						break;
					case 8:
						if(System.Convert.ToInt32(lblLeft8.Text)<=0)
							bAtCap = true;
						break;
					case 9:
						if(System.Convert.ToInt32(lblLeft9.Text)<=0)
							bAtCap = true;
						break;
				}

				if(bAtCap)
					MessageBox.Show("You cannot memorize any more spells at this level.");
				else
				{
					bool bFound = false;
					foreach(CSpell oSpellCursor in oCharacter.Spells)
						if(oSpellCursor.ID==oSpell.ID) 
						{
							bFound = true;
							oSpellCursor.Count += 1;
							break;
						}
					if(!bFound) 
					{
						oSpell.Count = 1;
						oCharacter.Spells.Add(oSpell);
					}
					SetSpellsLeft();
					LoadSpells();
				}
			}
		}

		

	}
}
