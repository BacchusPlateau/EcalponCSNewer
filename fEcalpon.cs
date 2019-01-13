using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

/////////////////////////////////////////////////////////////////
///Ecalpon, a D&D based adventure.
///Made possible by Patrice Scribe's groundbreaking work with DirectDraw for
///Visual Basic.  Thanks Patrice.
///Also made possible by PageNet (Original VB5 version), Magenic (Ported to VB6,
///and DirectX for VB, then to VB.NET Beta), Match.com (ported to C# release)  ;-)
///
///Ultima III RULES FOREVER!!!!!!!!!
///'*******************************************************************************
////////////////////////////////////////////////////////////////

namespace Ecalpon
{
	/// <summary>
	/// Summary description for fEcalpon.
	/// </summary>
	public class fEcalpon : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox picSurf;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.Timer TimerMonster;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// Main form used to house the Direct Draw Primary Surface
		/// </summary>
		public fEcalpon()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			CEcalpon.gDX.dixuInit(0, this, 640, 480, 16, picSurf);
			
			CEcalpon.InitGameTiles(picSurf);
			
			timer.Enabled = true;
			TimerMonster.Enabled = true;
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(fEcalpon));
			this.picSurf = new System.Windows.Forms.PictureBox();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.TimerMonster = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// picSurf
			// 
			this.picSurf.Name = "picSurf";
			this.picSurf.Size = new System.Drawing.Size(640, 480);
			this.picSurf.TabIndex = 0;
			this.picSurf.TabStop = false;
			this.picSurf.Paint += new System.Windows.Forms.PaintEventHandler(this.picSurf_Paint);
			// 
			// timer
			// 
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// TimerMonster
			// 
			this.TimerMonster.Interval = 1000;
			this.TimerMonster.Tick += new System.EventHandler(this.TimerMonster_Tick);
			// 
			// fEcalpon
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(632, 453);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.picSurf});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "fEcalpon";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Ecalpon";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fEcalpon_KeyDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.fEcalpon_Closing);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fEcalpon_KeyPress);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Gracefully destroy objects
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void fEcalpon_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			CEcalpon.DestroyObjects();
			CEcalpon.CleanUp();
		}

		/// <summary>
		/// Handles the direction keys.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void fEcalpon_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(CEcalpon.SuspendPlay)
				return;

			int Row = CEcalpon.gCurRow;
			int Col = CEcalpon.gCurCol;

			//When a key is pressed, clear the messages
			CEcalpon.gMoveMessage = "";
			CEcalpon.gAttackMessage = "";

			//Fight movement
			if(CEcalpon.FightMode && !CEcalpon.FightCharAttack && CEcalpon.FightCharMove) 
			{
				switch(e.KeyCode)
				{
					case Keys.Up:
						CEcalpon.oCombat.ValidateFightMove(-1, 0);
						break;
					case Keys.Down:
						CEcalpon.oCombat.ValidateFightMove(1, 0);
						break;
					case Keys.Left:
						CEcalpon.oCombat.ValidateFightMove(0, -1);
						break;
					case Keys.Right:
						CEcalpon.oCombat.ValidateFightMove(0, 1);
						break;
				}
			}

			//Fight attack
			if(CEcalpon.FightCharAttack) 
			{
				switch(e.KeyCode)
				{
					case Keys.Up:
						CEcalpon.gAttackMessage = "North";
						CEcalpon.oCombat.ValidateAttack(-1, 0);
						break;
					case Keys.Down:
						CEcalpon.gAttackMessage = "South";
						CEcalpon.oCombat.ValidateAttack(1, 0);
						break;
					case Keys.Left:
						CEcalpon.gAttackMessage = "West";
						CEcalpon.oCombat.ValidateAttack(0, -1);
						break;
					case Keys.Right:
						CEcalpon.gAttackMessage = "East";
						CEcalpon.oCombat.ValidateAttack(0, 1);
						break;
				}
			}

			//Spell casting
			if(CEcalpon.SpellPending) 
			{
				switch(e.KeyCode)
				{
					case Keys.Up:
						CEcalpon.gAttackMessage = "North";
						CEcalpon.SpellParm = "N";
						break;
					case Keys.Down:
						CEcalpon.gAttackMessage = "South";
						CEcalpon.SpellParm = "S";
						break;
					case Keys.Left:
						CEcalpon.gAttackMessage = "West";
						CEcalpon.SpellParm = "W";
						break;
					case Keys.Right:
						CEcalpon.gAttackMessage = "East";
						CEcalpon.SpellParm = "E";
						break;
				}
			}

			//Surface Movement Processing
			if(!CEcalpon.FightMode && !CEcalpon.InEncounter) 
			{	
				switch(e.KeyCode) 
				{
					case Keys.Up:
						if((Row - 1) >= 1) 
							if(CEcalpon.ValidateMove(Row - 1, Col))
								CEcalpon.gCurRow = Row - 1;
						break;
					case Keys.Down:
						if((Row + 1) <= CEcalpon.gintMatrix.GetUpperBound(0)) 
							if(CEcalpon.ValidateMove(Row + 1, Col))
								CEcalpon.gCurRow = Row + 1;
						break;
					case Keys.Left:
						if((Col - 1) >= 1) 
							if(CEcalpon.ValidateMove(Row, Col - 1)) 
								CEcalpon.gCurCol = Col - 1;
						break;
					case Keys.Right:
						if((Col + 1) <= CEcalpon.gintMatrix.GetUpperBound(1)) 
							if(CEcalpon.ValidateMove(Row, Col + 1)) 
								CEcalpon.gCurCol = Col + 1;
						break;
				}
			}

			//Encounter movement
			if(!CEcalpon.FightMode && CEcalpon.InEncounter) 
			{
				CEncounter oE = CEcalpon.oEncounter;
				switch(e.KeyCode) 
				{
					case Keys.Up:
						if(oE.CurRow-1 >= 0)
						{
							if(oE.ValidateMove(oE.CurRow-1, oE.CurCol))
								oE.CurRow--;
						}
						else
							CEcalpon.QueryLeaveEncounter();							
						break;
					case Keys.Down:
						if(oE.CurRow+1 <= oE.Rows-1)
						{
							if(oE.ValidateMove(oE.CurRow+1, oE.CurCol))
								oE.CurRow++;
						}
						else
							CEcalpon.QueryLeaveEncounter();	
						break;
					case Keys.Left:
						if(oE.CurCol-1 >= 0)
						{
							if(oE.ValidateMove(oE.CurRow, oE.CurCol-1))
								oE.CurCol--;
						}
						else
							CEcalpon.QueryLeaveEncounter();	
						break;
					case Keys.Right:
						if(oE.CurCol+1 <= oE.Cols-1)
						{
							if(oE.ValidateMove(oE.CurRow, oE.CurCol+1))
								oE.CurCol++;
						}
						else
							CEcalpon.QueryLeaveEncounter();	
						break;
				}
			}
		}

		/// <summary>
		/// Handles the game commands sent via a single key press
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void fEcalpon_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			
			string KeyAscii = e.KeyChar.ToString();

			switch(KeyAscii) 
			{
				case "a" : //attack
					if(CEcalpon.FightMode)
						CEcalpon.FightCharAttack = true;
					break;
				case "c" : //cast spell
					if(CEcalpon.FightMode)
						CEcalpon.CastSpell(CEcalpon.FightCharMoveOrder);
					else
						CEcalpon.PromptForWho = true;
					break;
				case "p" : //pass
					if(CEcalpon.FightMode) 
					{
						CEcalpon.ShowMessage("Pass", 2);
						CEcalpon.oCombat.NextChar();
					}
					break;
				case "q" : //quit
					CEcalpon.g_dixuAppEnd = true;
					break;
				case "s" : //save game, only outside!
					if(!CEcalpon.FightMode && !CEcalpon.InEncounter && !CEcalpon.EncounterFightMode) 
					{
						DialogResult iResult = MessageBox.Show("Save game?", "Save Game", MessageBoxButtons.YesNo);
						if(iResult == DialogResult.Yes) 
							CEcalpon.SaveCharacters();
					}
                    break;
				case "r" : //restore game, only outside
					if(!CEcalpon.FightMode && !CEcalpon.InEncounter && !CEcalpon.EncounterFightMode) 
					{
						DialogResult iResult = MessageBox.Show("Restore game?", "Restore Game", MessageBoxButtons.YesNo);
						if(iResult == DialogResult.Yes) 
							CEcalpon.RestoreCharacters();
					}
					break;
				case "1" : //1-5 displays character sheet
					if(CEcalpon.PromptForWho)
						CEcalpon.CastSpell(1);
					if(CEcalpon.SpellPending)
						CEcalpon.SpellParm = "1";
					if(!CEcalpon.PromptForWho && !CEcalpon.SpellPending)
						ShowCharacter(1);
					break;
				case "2" :
					if(CEcalpon.PromptForWho)
						CEcalpon.CastSpell(2);
					if(CEcalpon.SpellPending)
						CEcalpon.SpellParm = "2";
					if(!CEcalpon.PromptForWho && !CEcalpon.SpellPending)
						ShowCharacter(2);
					break;
				case "3" :
					if(CEcalpon.PromptForWho)
						CEcalpon.CastSpell(3);
					if(CEcalpon.SpellPending)
						CEcalpon.SpellParm = "3";
					if(!CEcalpon.PromptForWho && !CEcalpon.SpellPending)
						ShowCharacter(3);
					break;
				case "4" :
					if(CEcalpon.PromptForWho)
						CEcalpon.CastSpell(4);
					if(CEcalpon.SpellPending)
						CEcalpon.SpellParm = "4";
					if(!CEcalpon.PromptForWho && !CEcalpon.SpellPending)
						ShowCharacter(4);
					break;
				case "5" :
					if(CEcalpon.PromptForWho)
						CEcalpon.CastSpell(5);
					if(CEcalpon.SpellPending)
						CEcalpon.SpellParm = "5";
					if(!CEcalpon.PromptForWho && !CEcalpon.SpellPending)
						ShowCharacter(5);
					break;
			}
		}

		/// <summary>
		/// Display a character sheet for the indicated character order
		/// </summary>
		/// <param name="CharOrder"></param>
		private void ShowCharacter(int CharOrder) 
		{
			if(CharOrder<=CEcalpon.gobjParty.Characters.Count) 
			{
				fCharacterView f = new fCharacterView();
				f.Character = CEcalpon.gobjParty.GetCharacterByOrder(CharOrder);
				f.ShowDialog();
			}

		}

		/// <summary>
		/// Redraws the screen because the form detects a change
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void picSurf_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			CEcalpon.gDX.dixuDDraw.RestoreAllSurfaces();
			CEcalpon.DrawScreen(picSurf);
		}

		/// <summary>
		/// Performs many time / round based operations relating to gameplay
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer_Tick(object sender, System.EventArgs e)
		{
			if(CEcalpon.SuspendPlay)
				return;

			//We must continually redraw the backbuffer and flip the surfaces
			CEcalpon.DrawScreen(picSurf);

			//Allow keypresses to be processed
			Application.DoEvents();

			//Check for fight mode processing
			if(CEcalpon.FightMode)
			{
				if(CEcalpon.FightMonstMove)
				{
					CEcalpon.oCombat.FightMoveMonsters();
				}
			}

			//Clean up if the game is over
			if(CEcalpon.g_dixuAppEnd) 
			{
				timer.Enabled = false;
				TimerMonster.Enabled = false;
				CEcalpon.DestroyObjects();
				this.Close();
			}
		}

		/// <summary>
		/// moves monsters independently of the other timer
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TimerMonster_Tick(object sender, System.EventArgs e)
		{
			

			//For displaying "read slow" messages
			if(CEcalpon.MessageShowing) 
				if(CEcalpon.MessageSecCount>0) 
					CEcalpon.MessageSecCount--;
				else
				{
					CEcalpon.MessageShowing = false;
					CEcalpon.gMessage = "";
				}

			if(CEcalpon.UsingGem) 
				if(CEcalpon.GemUseCount>0)
					CEcalpon.GemUseCount--;
				else 
				{
					CEcalpon.UsingGem = false;
					CEcalpon.ShowMessage("The magic gem turns to dust!",2);
				}

			if(CEcalpon.TimeSlip)
				if(CEcalpon.SlipCt>0)
					CEcalpon.SlipCt--;
				else
				{
					CEcalpon.TimeSlip=false;
					CEcalpon.ShowMessage("The spell fades...",2);
				}
			else 
			{
				if(!CEcalpon.FightMode && !CEcalpon.SuspendPlay && !CEcalpon.InEncounter) 
				{
					CEcalpon.SuspendPlay = true;
					CEcalpon.CheckForMonsters();
					CEcalpon.MoveSurfaceMonsters();
					CEcalpon.SuspendPlay = false;
				}
				
				//Only perform this if we're not fighting and we're in an encounter
				if(!CEcalpon.FightMode && !CEcalpon.SuspendPlay && CEcalpon.InEncounter)
				{
					CEcalpon.SuspendPlay = true;
					CEcalpon.oEncounter.MoveEncounterMonsters();
					CEcalpon.SuspendPlay = false;
				}

			}
		
		}


	}
}
