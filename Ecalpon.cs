using System;
using System.Windows.Forms;
using DxVBLib;
using System.Data.OleDb;
using System.Data;

namespace Ecalpon 
{

	public class CEcalpon 
	{
		//Adjacency
		public struct mPoint 
		{
			public int Row, Col;

			public void Point(int p1, int p2) 
			{
				Row = p1;
				Col = p2;    
			}
		}
		
		

		//Misc Constants
		public const int MAX_ROWS = 150;
		public const int MAX_COLS = 150;
		public const int eOUTSIDE_ENCOUNTER = 0;
		public const int eENCOUNTER = 1;
		public const int eMONSTER = 0;
		public const int eCHARACTER = 1;
		
		//Terrain Constants
		public const int tMOUNTAIN = 1;
		public const int tGRASS = 0;
		public const int tDESERT = 2;
		public const int tTREES1 = 4;
		public const int tTREES2 = 5;
		public const int tTREES3 = 6;
		public const int tTOWN = 8;
		public const int tPASS = 9;
		public const int tCAVE = 10;
		public const int tWALL = 11;

		//Overlay Constants
		public const int oKOBOLDS = 1;
		public const int oORCS = 2;
		public const int oGOBLINS = 3;
		public const int oIMPASSABLE = 99;

		//Holds the game id for save/restore
		public static int GameID = 0;
		
		//Fight Mode Support
		public static bool FightCharMove;
		public static int FightCharMoveOrder;
		public static bool FightCharAttack;
		public static bool FightMonstMove;
		public static int FightRow;
		public static int FightCol;

		//Modes
		public static bool FightMode;
		public static bool EncounterMode;
		public static bool EncounterFightMode;
		public static bool MessageShowing;
		public static int MessageSecCount;
		public static bool SuspendPlay;
		public static bool TimeSlip;

		//Messages to be drawn
		public static string gMoveMessage = "";
		public static string gAttackMessage = "";
		public static string gHitMessage = "";
		public static string gMessage = "";

		//Gameplay Globals
		public static CParty gobjParty;
		public static CCombat oCombat=null;
		public static CEncounter oEncounter=null;
		public static CMonsters gobjSurfaceMonsters;
		public static int[,] gintOverlay;
		public static int[,] gintMatrix;
		public static int mCharID;
		public static int mCharOrder;
		public static bool PromptForSpell;
		public static bool PromptForWho;
		public static int SpellCaster;
		public static int SpellID;
		public static bool SpellPending;
		public static string SpellParm;
		public static string SpellType;
		public static string SpellPrompt;
		public static string SpellName;
		public static int SlipCt;
		public static bool InEncounter;
		public static bool UsingGem;
		public static int GemUseCount;
		public static Random oRandom;
		public static bool AutoCloseCharacterView = false;

		//True if game is ending
		public static bool g_dixuAppEnd;

		//Current coordinates of game play
		public static int gCurRow;
		public static int gCurCol;

		//Game Tiles
		public static DirectDrawSurface7 dxsMountain;
		public static DirectDrawSurface7 dxsGrass;
		public static DirectDrawSurface7 dxsDesert;
		public static DirectDrawSurface7 dxsTrees1;
		public static DirectDrawSurface7 dxsTrees2;
		public static DirectDrawSurface7 dxsTrees3;
		public static DirectDrawSurface7 dxsOrc;
		public static DirectDrawSurface7 dxsTown;
		public static DirectDrawSurface7 dxsFighter;
		public static DirectDrawSurface7 dxsGoblin;
		public static DirectDrawSurface7 dxsCave;
		public static DirectDrawSurface7 dxsPass;

		//Fightmode specific surfaces
		public static DirectDrawSurface7 dxsMonster1;
		public static DirectDrawSurface7 dxsCleric;
		public static DirectDrawSurface7 dxsWizard;
		public static DirectDrawSurface7 dxsOutline;

		//Encounter-mode specific surfaces
		public static DirectDrawSurface7 dxsStoneWall;
		public static DirectDrawSurface7 dxsWoodFloor;
		public static DirectDrawSurface7 dxsProprietor;
		public static DirectDrawSurface7 dxsVBarrier;
		public static DirectDrawSurface7 dxsHBarrier;
		public static DirectDrawSurface7 dxsTeleporter;
		public static DirectDrawSurface7 dxsDoor;

		//Cave wall surfaces
		public static DirectDrawSurface7 dxsCaveFloor;
		public static DirectDrawSurface7 dxsCaveTL;
		public static DirectDrawSurface7 dxsCaveTR;
		public static DirectDrawSurface7 dxsCaveBL;
		public static DirectDrawSurface7 dxsCaveBR;
		public static DirectDrawSurface7 dxsCaveLWall;
		public static DirectDrawSurface7 dxsCaveRWall;
		public static DirectDrawSurface7 dxsCaveCeiling;
		public static DirectDrawSurface7 dxsCaveOpenArea;
		public static DirectDrawSurface7 dxsCaveFloorM;
		public static DirectDrawSurface7 dxsCaveWallM;
		public static DirectDrawSurface7 dxsCaveTRM;
		public static DirectDrawSurface7 dxsCaveTBM;
		public static DirectDrawSurface7 dxsCaveTTM;
		public static DirectDrawSurface7 dxsCaveTLM;
		public static DirectDrawSurface7 dxsCavePlus;
		
		//Sounds
		public static DirectSoundBuffer dsDeadJim;
		public static DirectSoundBuffer dsHit;

		//Sounds enumeration
		public enum Sounds {DEATH, HIT};

		//Public instance classes
		public static CDirectx gDX;

		/// <summary>
		/// Remove a monster from the Overlay matrix
		/// </summary>
		public static void RemoveSurfaceMonster() 
		{
			foreach(CMonster oMonster in gobjSurfaceMonsters) 
			{
				if(oMonster.FightRow==oCombat.FightRow && oMonster.FightCol==oCombat.FightCol) 
				{
					gobjSurfaceMonsters.Delete(oMonster.InternalIndex);
					break;
				}
			}

			gintOverlay[oCombat.FightRow, oCombat.FightCol] = 0;
			oCombat.FightRow = -1;
			oCombat.FightCol = -1;

		}

		/// <summary>
		/// Overloaded function to return a random number with a fixed
		/// lower limit of 1.
		/// </summary>
		/// <param name="ulimit"></param>
		/// <returns></returns>
		public static int MyRand(int ulimit) 
		{
			return oRandom.Next(1,ulimit+1);
		}

		/// <summary>
		/// Overloaded function to return a random number with a specific
		/// lower limit.
		/// </summary>
		/// <param name="llimit"></param>
		/// <param name="ulimit"></param>
		/// <returns></returns>
		public static int MyRand(int llimit, int ulimit) 
		{
			return oRandom.Next(llimit,ulimit+1);
		}

		/// <summary>
		/// Draws an encounter
		/// </summary>
		/// <param name="pic"></param>
		public static void DrawEncounter(PictureBox pic)
		{
			RECT 
				DR,
				SR;
			int 
				sRow, 
				sCol, 
				sData, 
				oData,
				ccount=0;
			bool bFound;				
			int[,] 
				iMatrix,
				iOverlay;

			iMatrix = new int[19,19];
			iOverlay = new int[19,19];
			
			//All tiles are 25x25 pixels
			SR.Top = 0;
			SR.Left = 0;
			SR.Right = 24;
			SR.Bottom = 24;

			try 
			{
				//Copy viewable matrix into local array
				for(int iRow=0; iRow<19; iRow++) 
				{
					for(int iCol=0; iCol<19; iCol++) 
					{
						sRow = iRow + oEncounter.CurRow - 9;
						sCol = iCol + oEncounter.CurCol - 9;
						if((sRow<1) | (sRow>oEncounter.Matrix.GetUpperBound(0)) |
							(sCol<1) | (sCol>oEncounter.Matrix.GetUpperBound(1)))
							iMatrix[iRow, iCol] = -1;
						else 
						{
							iMatrix[iRow, iCol] = oEncounter.Matrix[sRow, sCol];
							iOverlay[iRow, iCol] = oEncounter.Overlay[sRow, sCol];
						}
					}
				}

				//Starting at the center (9,9), go N, S, E and W to black out tiles
				//the user can't see from his position.  Negated if using a gem.
				//Note that there is a flaw in my algorithm because the overlapping
				//of each cardinal direction obscures the diagonal view.
				if(!UsingGem) 
				{
					//Search the north
					bFound = false;
					for(int x=8; x>-1; x--)
					{
						switch(iMatrix[x,9]) 
						{
							case 9: case 15: case 16: case 17: case 18: case 20:
							case 21: case 22: case 24: case 25: case 26: case 27:
							case 28: case 29: case 30: case 32:
								for(int iBarrier=x-1; iBarrier>-1; iBarrier--) 
								{
									for(int y=0; y<19; y++)
									{
										iMatrix[iBarrier,y]=-1;
										iOverlay[iBarrier,y]=-1;
									}//y
								}//iBarrier
								bFound = true;
								break;
						}//switch
						if(bFound)
							break;
					}//x

					//Search the south
					bFound = false;
					for(int x=10; x<19; x++)
					{
						switch(iMatrix[x,9]) 
						{
							case 9: case 15: case 16: case 17: case 18: case 20:
							case 21: case 22: case 24: case 25: case 26: case 27:
							case 28: case 29: case 30: case 32:
								for(int iBarrier=x+1; iBarrier<19; iBarrier++) 
								{
									for(int y=0; y<19; y++) 
									{
										iMatrix[iBarrier,y]=-1;
										iOverlay[iBarrier,y]=-1;
									}//y
								}//iBarrier
								bFound = true;
								break;
						}//switch
						if(bFound)
							break;
					}//x

					//Search the east
					bFound = false;
					for(int y=10; y<19; y++)
					{
						switch(iMatrix[9,y]) 
						{
							case 9: case 15: case 16: case 17: case 18: case 20:
							case 21: case 22: case 24: case 25: case 26: case 27:
							case 28: case 29: case 30: case 32:
								for(int iBarrier=y+1; iBarrier<19; iBarrier++) 
								{
									for(int x=0; x<19; x++)
									{
										iMatrix[x,iBarrier]=-1;
										iOverlay[x,iBarrier]=-1;
									}//x
								}//iBarrier
								bFound = true;
								break;
						}//switch
						if(bFound)
							break;
					}//y

					//Search the west
					bFound = false;
					for(int y=8; y>-1; y--)
					{
						switch(iMatrix[9,y]) 
						{
							case 9: case 15: case 16: case 17: case 18: case 20:
							case 21: case 22: case 24: case 25: case 26: case 27:
							case 28: case 29: case 30: case 32:
								for(int iBarrier=y-1; iBarrier>-1; iBarrier--) 
								{
									for(int x=0; x<19; x++) 
									{
										iMatrix[x,iBarrier]=-1;
										iOverlay[x,iBarrier]=-1;
									}//x
								}//iBarrier
								bFound = true;
								break;
						}//switch
						if(bFound)
							break;
					}//y
				}//check for using gem

				//Draw 19x19 block of tiles
				for(int iRow=0; iRow<19; iRow++) 
				{
					for(int iCol=0; iCol<19; iCol++)
					{
						DR.Bottom = 25 * iRow + 25;
						DR.Top = 25 * iRow;
						DR.Right = 25 * iCol + 25;
						DR.Left = 25 * iCol;

						sData = iMatrix[iRow, iCol];
						oData = iOverlay[iRow, iCol];

						//Draw surface tiles
						switch(sData) 
						{
							case 0:  //Grass
								gDX.dixuBackBuffer.Blt(ref DR, dxsGrass, ref SR,
									CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 1:  //Stone wall
								gDX.dixuBackBuffer.Blt(ref DR, dxsStoneWall, ref SR,
									CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 2:  //Wood Floor
								gDX.dixuBackBuffer.Blt(ref DR, dxsWoodFloor, ref SR,
									CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 3: //Proprietor
								gDX.dixuBackBuffer.Blt(ref DR, dxsWoodFloor, ref SR,
									CONST_DDBLTFLAGS.DDBLT_WAIT);
								gDX.dixuBackBuffer.Blt(ref DR, dxsProprietor, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 4:  //vertical barrier
								gDX.dixuBackBuffer.Blt(ref DR, dxsVBarrier, ref SR,
									CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 5:  //trees
								gDX.dixuBackBuffer.Blt(ref DR, dxsTrees1, ref SR,
									CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 6:  //horizontal barrier
								gDX.dixuBackBuffer.Blt(ref DR, dxsHBarrier, ref SR,
									CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 9:  //cave floor
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveFloor, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 15:  //tlcave
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveTL, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 16:  //blcave
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveBL, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 17: //trcave
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveTR, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 18: //brcave
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveBR, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 20: //cavewalll
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveLWall, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 21: //cavewallr
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveRWall, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 22: //caveceiling
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveCeiling, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 23: //caveopenarea
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveOpenArea, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 24: //casefloormiddle
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveFloorM, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 25: //cavewallmiddle
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveWallM, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 26: //cave t right middle
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveTRM, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 27: //cave t bottom middle
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveTBM, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 28: //cave t top middle
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveTTM, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 29: //cave t left middle
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveTLM, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 30: //cave plus sign
								gDX.dixuBackBuffer.Blt(ref DR, dxsCavePlus, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 31: //teleporter
								gDX.dixuBackBuffer.Blt(ref DR, dxsTeleporter, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 32: //door
								gDX.dixuBackBuffer.Blt(ref DR, dxsDoor, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
						}

						//Draw Overlay Tiles
						switch(oData) 
						{
							case 1: //Kobolds
								gDX.dixuBackBuffer.Blt(ref DR, dxsMonster1, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 2: //Orcs
								gDX.dixuBackBuffer.Blt(ref DR, dxsOrc, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 3: //Goblins
								gDX.dixuBackBuffer.Blt(ref DR, dxsGoblin, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
						}
					}
				}

				//Blit our heroes
				DR.Bottom = 25 * 9 + 25;
				DR.Top = 25 * 9;
				DR.Right = 25 * 9 + 25;
				DR.Left = 25 * 9;

				gDX.dixuBackBuffer.Blt(ref DR, dxsFighter, ref SR, CONST_DDBLTFLAGS.DDBLT_KEYSRC |
					CONST_DDBLTFLAGS.DDBLT_WAIT);

				//Headings
				gDX.dixuDrawText("Who", 480, 0);
				gDX.dixuDrawText("HP", 560, 0);
				gDX.dixuDrawText("AC", 590, 0);
				gDX.dixuDrawText("C", 620, 0);

				//Show names, hitpoints, ac and class
				foreach(CCharacter oCharacter in gobjParty.Characters) 
				{
					ccount++;
					gDX.dixuDrawText(oCharacter.Name, 480, ccount * 20);
					if(oCharacter.HitPoints <= 0) 
						gDX.dixuDrawText("D", 560, ccount * 20);
					else
						gDX.dixuDrawText(oCharacter.HitPoints.ToString(), 560,
							ccount * 20);
					gDX.dixuDrawText(oCharacter.ArmorClass.ToString(), 590,
						ccount * 20);
					gDX.dixuDrawText(oCharacter.ClassName.Substring(0,1), 620, ccount * 20);
				}

				//Draw Move message
				if(gMoveMessage.Length > 0) 
					gDX.dixuDrawText(gMoveMessage, 480, 400);

				//spell related processing
				if(SpellPending) 
				{
					if(SpellParm.Length==0) 
					{
						gDX.dixuDrawText(SpellName, 480, 320);
						gDX.dixuDrawText(SpellPrompt, 480, 350);
					}
					else 
					{
						if(SpellType=="Any")
							ProcessSpellAny();
						if(SpellType=="Attack") 
							ShowMessage("Not fighting.",2);
					}
				}

				//Draw the screen
				gDX.dixuBackBufferDraw(pic);
			}
			catch (Exception e) 
			{
				MessageBox.Show(e.ToString());
			}
		}


		/// <summary>
		/// Draws surface movement
		/// </summary>
		/// <param name="pic"></param>
		public static void DrawScreen(PictureBox pic) 
		{
			RECT 
				DR,
				SR;
			int 
				sRow, 
				sCol, 
				sData, 
				oData,
				ccount=0;
				
			int[,] 
				iMatrix,
				iOverlay;

			if(gDX.dixuBackBuffer==null) return;

			iMatrix = new int[19,19];
			iOverlay = new int[19,19];
			
			//All tiles are 25x25 pixels
			SR.Top = 0;
			SR.Left = 0;
			SR.Right = 24;
			SR.Bottom = 24;

			//Resets back buffer
			gDX.dixuBackBufferClear();

			//Draw any pending messages
			if(MessageShowing) 
				gDX.dixuDrawText(gMessage, 480, 440);

			//Check for fighting
			if(FightMode) 
			{
				DrawFight(pic);
				return;
			}

			//Are we in an encounter?
			if(InEncounter)
			{
				DrawEncounter(pic);
				return;
			}

			try 
			{

				//Copy viewable matrix into local array
				for(int iRow=0; iRow<19; iRow++) 
				{
					for(int iCol=0; iCol<19; iCol++) 
					{
						sRow = iRow + gCurRow - 9;
						sCol = iCol + gCurCol - 9;
						if((sRow<1) | (sRow>gintMatrix.GetUpperBound(0)) |
							(sCol<1) | (sCol>gintMatrix.GetUpperBound(1)))
							iMatrix[iRow, iCol] = -1;
						else 
						{
							iMatrix[iRow, iCol] = gintMatrix[sRow, sCol];
							iOverlay[iRow, iCol] = gintOverlay[sRow, sCol];
						}
					}
				}

				
				//Starting at the center (9,9), go N, S, E and W to black out tiles
				//the user can't see from his position.  Negated if using a gem.
				if(!UsingGem) 
				{
					//Search the north
					for(int x=8; x>-1; x--)
						for(int y=0; y<19; y++)
							switch(iMatrix[x,y]) 
							{
								case 1:
								case 10:
								case 11:
									for(int iBarrier=x-1; iBarrier>-1; iBarrier--) 
									{
										iMatrix[iBarrier,y]=-1;
										iOverlay[iBarrier,y]=-1;
									}
									break;
							}//switch

					//Search the south
					for(int x=10; x<19; x++)
						for(int y=0; y<19; y++)
							switch(iMatrix[x,y]) 
							{
								case 1:
								case 10:
								case 11:
									for(int iBarrier=x+1; iBarrier<19; iBarrier++) 
									{
										iMatrix[iBarrier,y]=-1;
										iOverlay[iBarrier,y]=-1;
									}
									break;
							}//switch

					//Search the east
					for(int x=0; x<19; x++)
						for(int y=10; y<19; y++)
							switch(iMatrix[x,y]) 
							{
								case -1:
								case 1:
								case 10:
								case 11:
									for(int iBarrier=y+1; iBarrier<19; iBarrier++) 
									{
										iMatrix[x,iBarrier]=-1;
										iOverlay[x,iBarrier]=-1;
									}
									break;
							}//switch

					//Search the west
					for(int x=0; x<19; x++)
						for(int y=8; y>-1; y--)
							switch(iMatrix[x,y]) 
							{
								case -1:
								case 1:
								case 10:
								case 11:
									for(int iBarrier=y-1; iBarrier>-1; iBarrier--) 
									{
										iMatrix[x,iBarrier]=-1;
										iOverlay[x,iBarrier]=-1;
									}
									break;
							}//switch


				} //Check for using gem


				//Draw 19x19 block of tiles
				for(int iRow=0; iRow<19; iRow++) 
				{
					for(int iCol=0; iCol<19; iCol++)
					{
						DR.Bottom = 25 * iRow + 25;
						DR.Top = 25 * iRow;
						DR.Right = 25 * iCol + 25;
						DR.Left = 25 * iCol;

						sData = iMatrix[iRow, iCol];
						oData = iOverlay[iRow, iCol];

						//Draw surface tiles
						switch(sData) 
						{
							case 0:  //Grass
								gDX.dixuBackBuffer.Blt(ref DR, dxsGrass, ref SR,
									CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 1:  //Mountain
								gDX.dixuBackBuffer.Blt(ref DR, dxsMountain, ref SR,
									CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 2:  //Desert
								gDX.dixuBackBuffer.Blt(ref DR, dxsDesert, ref SR,
									CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 4:  //Trees
							case 5:  //Trees
							case 6:  //Trees
								gDX.dixuBackBuffer.Blt(ref DR, dxsTrees1, ref SR,
									CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 8:  //Town
								gDX.dixuBackBuffer.Blt(ref DR, dxsTown, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 9:  //Pass
								gDX.dixuBackBuffer.Blt(ref DR, dxsPass, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 10:  //Cave
								gDX.dixuBackBuffer.Blt(ref DR, dxsCave, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 11:  //Stone wall
								gDX.dixuBackBuffer.Blt(ref DR, dxsStoneWall, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;

						}

						//Draw Overlay Tiles
						switch(oData) 
						{
							case 1: //Kobolds
								gDX.dixuBackBuffer.Blt(ref DR, dxsMonster1, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 2: //Orcs
								gDX.dixuBackBuffer.Blt(ref DR, dxsOrc, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 3: //Goblins
								gDX.dixuBackBuffer.Blt(ref DR, dxsGoblin, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
						}

					}
				}

				//Blit our heroes
				DR.Bottom = 25 * 9 + 25;
				DR.Top = 25 * 9;
				DR.Right = 25 * 9 + 25;
				DR.Left = 25 * 9;

				gDX.dixuBackBuffer.Blt(ref DR, dxsFighter, ref SR, CONST_DDBLTFLAGS.DDBLT_KEYSRC |
					CONST_DDBLTFLAGS.DDBLT_WAIT);

				//Headings
				gDX.dixuDrawText("Who", 480, 0);
				gDX.dixuDrawText("HP", 560, 0);
				gDX.dixuDrawText("AC", 590, 0);
				gDX.dixuDrawText("C", 620, 0);

				//Show names, hitpoints, ac and class
				foreach(CCharacter oCharacter in gobjParty.Characters) 
				{
					ccount++;
					gDX.dixuDrawText(oCharacter.Name, 480, ccount * 20);
					if(oCharacter.HitPoints <= 0) 
						gDX.dixuDrawText("D", 560, ccount * 20);
					else
						gDX.dixuDrawText(oCharacter.HitPoints.ToString(), 560,
							ccount * 20);
					gDX.dixuDrawText(oCharacter.ArmorClass.ToString(), 590,
						ccount * 20);
					gDX.dixuDrawText(oCharacter.ClassName.Substring(0,1), 620, ccount * 20);
				}

				//Draw Move message
				if(gMoveMessage.Length > 0) 
					gDX.dixuDrawText(gMoveMessage, 480, 400);

				//spell related processing
				if(SpellPending) 
				{
					if(SpellParm.Length==0) 
					{
						gDX.dixuDrawText(SpellName, 480, 320);
						gDX.dixuDrawText(SpellPrompt, 480, 350);
					}
					else 
					{
						if(SpellType=="Any")
							ProcessSpellAny();
						if(SpellType=="Attack") 
							ShowMessage("Not fighting.",2);
					}
				}

				//Draw the screen
				gDX.dixuBackBufferDraw(pic);
			}
			catch (Exception e) 
			{
				MessageBox.Show(e.ToString());
			}
		}

		/// <summary>
		/// Used to draw the conflicts.
		/// </summary>
		/// <param name="pic"></param>
		public static void DrawFight(PictureBox pic) 
		{
			RECT 
				DR,
				SR;
			int 
				sData, 
				oData,
				ccount=0;
			
			CCharacter oFightChar = null;

			int[,] 
				iMatrix,
				iOverlay;

			if(gDX.dixuBackBuffer==null) return;

			iMatrix = new int[19,19];
			iOverlay = new int[19,19];
			
			//All tiles are 25x25 pixels
			SR.Top = 0;
			SR.Left = 0;
			SR.Right = 24;
			SR.Bottom = 24;

			if(FightCharMove) 
				oFightChar = gobjParty.GetCharacterByOrder(FightCharMoveOrder);

			try 
			{

				//Draw 19x19 block of tiles
				for(int iRow=0; iRow<19; iRow++) 
				{
					for(int iCol=0; iCol<19; iCol++)
					{
						DR.Bottom = 25 * iRow + 25;
						DR.Top = 25 * iRow;
						DR.Right = 25 * iCol + 25;
						DR.Left = 25 * iCol;

						sData = oCombat.FightBase[iRow, iCol];
						oData = oCombat.FightOverlay[iRow, iCol];

						//Draw surface tiles
						switch(sData) 
						{
							case 0:  //Grass
								gDX.dixuBackBuffer.Blt(ref DR, dxsGrass, ref SR,
									CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 1:  //Mountain
								gDX.dixuBackBuffer.Blt(ref DR, dxsMountain, ref SR,
									CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 2:  //Desert
								gDX.dixuBackBuffer.Blt(ref DR, dxsDesert, ref SR,
									CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 4:  //Trees
							case 5:  //Trees
							case 6:  //Trees
								gDX.dixuBackBuffer.Blt(ref DR, dxsTrees1, ref SR,
									CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 9:  //cave floor
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveFloor, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 23:  //Cave open area
								gDX.dixuBackBuffer.Blt(ref DR, dxsCaveOpenArea, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
						}//switch

						//Draw Overlay Tiles
						switch(oData) 
						{
							/////Monsters
							case 1: //Kobolds
								gDX.dixuBackBuffer.Blt(ref DR, dxsMonster1, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 2: //Orcs
								gDX.dixuBackBuffer.Blt(ref DR, dxsOrc, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 3: //Goblins
								gDX.dixuBackBuffer.Blt(ref DR, dxsGoblin, ref SR,
									CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							/////Characters
							case 80:
								if(gobjParty.FightFindCharacter(iRow, iCol).HitPoints>0)
									gDX.dixuBackBuffer.Blt(ref DR, dxsFighter, ref SR,
										CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 81:
								if(gobjParty.FightFindCharacter(iRow, iCol).HitPoints>0)
									gDX.dixuBackBuffer.Blt(ref DR, dxsWizard, ref SR,
										CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
							case 82:
								if(gobjParty.FightFindCharacter(iRow, iCol).HitPoints>0)
									gDX.dixuBackBuffer.Blt(ref DR, dxsCleric, ref SR,
										CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
								break;
						}//switch

						//draw an outline over the character with focus
						if(oFightChar != null) 
						{
							if(oFightChar.FightCol == iCol && oFightChar.FightRow == iRow)
							{
								if(oFightChar.HitPoints > 0)
									gDX.dixuBackBuffer.Blt(ref DR, dxsOutline, ref SR,
										CONST_DDBLTFLAGS.DDBLT_KEYSRC | CONST_DDBLTFLAGS.DDBLT_WAIT);
							}
						}


					}//col
				}//row


				//Headings
				gDX.dixuDrawText("Who", 480, 0);
				gDX.dixuDrawText("HP", 560, 0);
				gDX.dixuDrawText("AC", 590, 0);
				gDX.dixuDrawText("C", 620, 0);

				//Show names, hitpoints, ac and class
				foreach(CCharacter oCharacter in gobjParty.Characters) 
				{
					ccount++;
					gDX.dixuDrawText(oCharacter.Name, 480, ccount * 20);
					if(oCharacter.HitPoints <= 0) 
						gDX.dixuDrawText("D", 560, ccount * 20);
					else
						gDX.dixuDrawText(oCharacter.HitPoints.ToString(), 560,
							ccount * 20);
					gDX.dixuDrawText(oCharacter.ArmorClass.ToString(), 590,
						ccount * 20);
					gDX.dixuDrawText(oCharacter.ClassName.Substring(0,1), 620, ccount * 20);
				}

				//Draw Move message
				if(FightCharMove && (oFightChar != null)) 
				{
					gDX.dixuDrawText("Player Turn", 480, 250);
					gDX.dixuDrawText("Who: " + oFightChar.Name, 480, 280);
				}

				//Draw attack prompt
				if(FightCharAttack) 
				{
					if(gAttackMessage.Length<=0) 
					{
						gDX.dixuDrawText("Attack -- Dir?", 480, 350);
					}
					else 
					{
						gDX.dixuDrawText("Attack -- " + gAttackMessage, 480, 350);
					}
				}

				//if there was a problem with the last move, display to user
				if(gMoveMessage.Length>0)
					gDX.dixuDrawText(gMoveMessage, 480, 430);
					
				//spell related processing
				if(SpellPending) 
				{
					if(SpellParm.Length==0) 
					{
						gDX.dixuDrawText(SpellName, 480, 320);
						gDX.dixuDrawText(SpellPrompt, 480, 350);
					}
					else 
					{
						if(SpellType=="Any")
							ProcessSpellAny();
						if(SpellType=="Attack") 
						{
							oCombat.ProcessSpellAttack();
						}
					}
				}

				//Draw the screen
				gDX.dixuBackBufferDraw(pic);
			}
			catch (Exception e) 
			{
				MessageBox.Show(e.ToString());
			}
		}
		
		/// <summary>
		/// Cast a spell!
		/// </summary>
		public static void CastSpell(int CharOrder) 
		{
			SpellPending = false;
			SpellCaster = 0;
			SpellID = 0;
			SpellParm = "";
			SpellType = "";
			SpellPrompt = "";

			int 
				iSpellID = -1;
			bool
				bModeOK = false,
				bFoundSpell = false;

			CCharacter oCharacter = gobjParty.GetCharacterByOrder(CharOrder);

			if(!FightMode)
				PromptForWho = false;

			//while we ask questions and cast the spell, suspend movement of monsters
			SuspendPlay = true;
			
			if(oCharacter.ClassID==(int)CCharacter.eClass.Fighter)
			{
				MessageBox.Show("Fighters don't have spells!  No spell for you!");
				SuspendPlay = false;
				return;
			}

			//prompt for spell			 
			Ecalpon.fInputBox f = new Ecalpon.fInputBox("Cast a spell", "Which spell id?");
			f.ShowDialog();
			string Result = f.Response;
			f.Close();
			f = null;

			//convert the string response into an integer
			try 
			{
				iSpellID = System.Convert.ToInt32(f.Response);
			}
			catch
			{
				MessageBox.Show("Could not convert your entry.");
				SuspendPlay = false;
				return;
			} 
			
			//Get the collection of spells
			CSpells oSpells = oCharacter.Spells;
			CSpell oTargetSpell = null;

			//find the spell in the character's collection of spells
			foreach(CSpell oSpell in oSpells)
			{
				if(oSpell.ID==iSpellID)
				{
					bFoundSpell = true;
					oTargetSpell = oSpell;
					break;
				}
			}

			if(bFoundSpell)
			{
				switch(oTargetSpell.SpellType) 
				{
					case "Attack":
						if(!FightMode) 
							MessageBox.Show("You're not fighting anyone.");
						else
							bModeOK = true;
						break;
					case "Defense":
						if(!FightMode) 
							MessageBox.Show("You're not fighting anyone.");
						else
							bModeOK = true;
						break;
					default:
						bModeOK = true;
						break;
				}//switch spell type

				if(oTargetSpell.Count>0 && bModeOK) 
				{
					oTargetSpell.Count--;
					//Give ok to proceed
					SpellPending = true;
					SpellCaster = oCharacter.Order;
					SpellType = oTargetSpell.SpellType;
					SpellID = oTargetSpell.ID;
					SpellName = oTargetSpell.Name;
					switch(oTargetSpell.ID) 
					{
						case 1: //magic missle
							SpellPrompt = "Direction?";
							break;
						case 2: //cure light wounds
							SpellPrompt = "Dest. char?";
							break;
					} //switch
				}
				else
				{
					MessageBox.Show("No more of that spell left.");
				}
			}
			else
			{
				MessageBox.Show("You don't know that spell.");
			}

			SuspendPlay = false;
		}

		/// <summary>
		/// Spell casting outside of combat
		/// </summary>
		public static void ProcessSpellAny() 
		{
			int
				iModifier = 0,
				iDestChar = 0;

			CSpell oSpell = new CSpell();
			oSpell.LoadByID(SpellID);

			CCharacter oCharacter = gobjParty.GetCharacterByOrder(SpellCaster);
			CCharacter oDestChar = null;

			switch(SpellID) 
			{
				case 2: //cure light wounds
					try 
					{
						iDestChar = System.Convert.ToInt32(SpellParm);
					}
					catch
					{
						MessageBox.Show("Could not convert your entry.");
						return;
					} 
					oDestChar = gobjParty.GetCharacterByOrder(iDestChar);
					iModifier = MyRand(oSpell.Modifier) * oCharacter.Level;
					if(iModifier + oDestChar.HitPoints > oDestChar.MaxHitPoints)
						oDestChar.HitPoints = oDestChar.MaxHitPoints;
					else
						oDestChar.HitPoints = oDestChar.HitPoints + iModifier;
					break;
				case 5: //Time Slip
					TimeSlip = true;
					if(!FightMode) 
					{
						if(!InEncounter) 
							SlipCt = 6;
						else
							SlipCt = 3;
					}
					break;
			} //switch

			SpellPending = false;
	
		}

		/// <summary>
		/// Purge game tiles from memory
		/// </summary>
		public static void DestroyObjects() 
		{
			//Terrain and characters
			dxsMountain = null;
			dxsGrass = null;
			dxsDesert = null;
			dxsTrees1 = null;
			dxsTrees2 = null;
			dxsTrees3 = null;
			dxsOrc = null;
			dxsTown = null;
			dxsFighter = null;
			dxsGoblin = null;
			dxsCave = null;
			dxsPass = null;

			//Fightmode specific surfaces
			dxsMonster1 = null;
			dxsCleric = null;
			dxsWizard = null;
			dxsOutline = null;

			//Encounter-mode specific surfaces
			dxsStoneWall = null;
			dxsWoodFloor = null;
			dxsProprietor = null;
			dxsVBarrier = null;
			dxsHBarrier = null;
			dxsTeleporter = null;
			dxsDoor = null;

			//Cave wall surfaces
			dxsCaveFloor = null;
			dxsCaveTL = null;
			dxsCaveTR = null;
			dxsCaveBL = null;
			dxsCaveBR = null;
			dxsCaveLWall = null;
			dxsCaveRWall = null;
			dxsCaveCeiling = null;
			dxsCaveOpenArea = null;
			dxsCaveFloorM = null;
			dxsCaveWallM = null;
			dxsCaveTRM = null;
			dxsCaveTBM = null;
			dxsCaveTTM = null;
			dxsCaveTLM = null;
			dxsCavePlus = null;

			//sound buffers
			dsHit = null;
			dsDeadJim = null;

			gDX.dixuDone();

		}

		/// <summary>
		/// Load the overlay into memory.  This includes impassable terrain,
		/// encounters, and monsters
		/// </summary>
		private static void LoadSurfaceMonsters() 
		{
			string sSQL;
			OleDbDataReader drReader;
			CDataAccess oDataAccess = new CDataAccess();
			CMonster oMonster;		

			sSQL = "SELECT * FROM Overlay";

			try 
			{
				oDataAccess.FillDataReader(out drReader, sSQL);
			
				while(drReader.Read())
				{
					gintOverlay[(System.Int16)drReader["oRow"],(System.Int16)drReader["oCol"]] =
						(System.Int16)drReader["oData"];

					if((gintOverlay[(System.Int16)drReader["oRow"],(System.Int16)drReader["oCol"]] > 0)	&&
						(gintOverlay[(System.Int16)drReader["oRow"],(System.Int16)drReader["oCol"]] < 30)) 
					{
						oMonster = new CMonster();
						oMonster.FightRow = (System.Int16) drReader["oRow"];
						oMonster.FightCol = (System.Int16) drReader["oCol"];
						oMonster.ID = (System.Int16) drReader["oData"];
						gobjSurfaceMonsters.Add(oMonster);
					}
				}
				drReader.Close();
				oDataAccess = null;
			}
			
			catch(Exception e)
			{
				MessageBox.Show(e.ToString());
			}
		}

		/// <summary>
		/// Load the game grid into memory
		/// </summary>
		public static void LoadMatrix() 
		{
			string sSQL;
			OleDbDataReader drReader;
			CDataAccess oDataAccess = new CDataAccess();
			
			//This is the top left map
			sSQL = "SELECT * FROM MapData WHERE ID = 0";

			try 
			{
				oDataAccess.FillDataReader(out drReader, sSQL);

				drReader.Read();
			
				while(drReader.Read())
				{
					gintMatrix[(System.Int16)drReader["Row"]+1,(System.Int16)drReader["Col"]+1] =
						(System.Int16)drReader["Data"];
				}
				drReader.Close();
				oDataAccess = null;
				
				for(int iRow=1; iRow<MAX_ROWS; iRow++) 
				{
					for(int iCol=1; iCol<MAX_COLS; iCol++) 
					{
						switch(gintMatrix[iRow, iCol]) 
						{
							case tMOUNTAIN:
								gintOverlay[iRow, iCol] = oIMPASSABLE;
								break;
							
							case tWALL:
								gintOverlay[iRow, iCol] = oIMPASSABLE;
								break;
						}
					}
				}

			}
			
			catch(Exception e)
			{
				MessageBox.Show(e.ToString());
			}
		}

		/// <summary>
		/// Play a sound
		/// </summary>
		/// <param name="Sound"></param>
		public static void PlaySound(Sounds Sound)
		{
			
			switch((int)Sound)
			{
				case (int)Sounds.DEATH:
					if(dsDeadJim!=null) 
					{
						dsDeadJim.Stop();
						dsDeadJim.Play(DxVBLib.CONST_DSBPLAYFLAGS.DSBPLAY_DEFAULT);
					}
					break;
				case (int)Sounds.HIT:
					if(dsHit!=null) 
					{
						dsHit.Stop();
						dsHit.Play(DxVBLib.CONST_DSBPLAYFLAGS.DSBPLAY_DEFAULT);
					}
					break;					
			}
			
		}

		/// <summary>
		/// Load sounds
		/// </summary>
		public static void InitSounds()
		{
			dsHit = gDX.LoadSoundFromWaveFile("Hit");
			dsDeadJim = gDX.LoadSoundFromWaveFile("HeDead");
		}

		/// <summary>
		/// Load game tiles into memory
		/// </summary>
		/// <param name="pic"></param>
		public static void InitGameTiles(PictureBox pic) 
		{
			dxsMountain = gDX.dixuCreateSurface(25,25,"mountain",pic);
			dxsGrass = gDX.dixuCreateSurface(25,25,"grass1",pic);
			dxsDesert = gDX.dixuCreateSurface(25,25,"desert",pic);
			dxsTrees1 = gDX.dixuCreateSurface(25,25,"trees1",pic);
			dxsTrees2 = gDX.dixuCreateSurface(25,25,"trees1",pic);
			dxsTrees3 = gDX.dixuCreateSurface(25,25,"trees1",pic);
			dxsFighter = gDX.dixuCreateSurface(25,25,"fighter",pic);
			dxsMonster1 = gDX.dixuCreateSurface(25,25,"monster1",pic);
			dxsCleric = gDX.dixuCreateSurface(25,25,"cleric",pic);
			dxsWizard = gDX.dixuCreateSurface(25,25,"wizard",pic);
			dxsOutline = gDX.dixuCreateSurface(25,25,"outline",pic);
			dxsOrc = gDX.dixuCreateSurface(25,25,"orc",pic);
			dxsTown = gDX.dixuCreateSurface(25,25,"town",pic);
			dxsPass = gDX.dixuCreateSurface(25,25,"pass",pic);
			dxsStoneWall = gDX.dixuCreateSurface(25,25,"stonewall",pic);
			dxsWoodFloor = gDX.dixuCreateSurface(25,25,"woodfloor",pic);
			dxsProprietor = gDX.dixuCreateSurface(25,25,"proprietor",pic);
			dxsVBarrier = gDX.dixuCreateSurface(25,25,"vbarrier",pic);
			dxsHBarrier = gDX.dixuCreateSurface(25,25,"hbarrier",pic);
			dxsCave = gDX.dixuCreateSurface(25,25,"cave",pic);
			dxsCaveFloor = gDX.dixuCreateSurface(25,25,"cavefloor",pic);
			dxsCaveTL = gDX.dixuCreateSurface(25,25,"cavetl",pic);
			dxsCaveTR = gDX.dixuCreateSurface(25,25,"cavetr",pic);
			dxsCaveBL = gDX.dixuCreateSurface(25,25,"cavebl",pic);
			dxsCaveBR = gDX.dixuCreateSurface(25,25,"cavebr",pic);
			dxsCaveLWall = gDX.dixuCreateSurface(25,25,"cavewalll",pic);
			dxsCaveRWall = gDX.dixuCreateSurface(25,25,"cavewallr",pic);
			dxsCaveCeiling = gDX.dixuCreateSurface(25,25,"caveceiling",pic);
			dxsCaveOpenArea = gDX.dixuCreateSurface(25,25,"caveopenarea",pic);
			dxsCaveFloorM = gDX.dixuCreateSurface(25,25,"cavefloorm",pic);
			dxsCaveWallM = gDX.dixuCreateSurface(25,25,"cavewallm",pic);
			dxsCaveTRM = gDX.dixuCreateSurface(25,25,"cavetrm",pic);
			dxsCaveTBM = gDX.dixuCreateSurface(25,25,"cavetbm",pic);
			dxsCaveTTM = gDX.dixuCreateSurface(25,25,"cavettm",pic);
			dxsCaveTLM = gDX.dixuCreateSurface(25,25,"cavetlm",pic);
			dxsCavePlus = gDX.dixuCreateSurface(25,25,"caveplus",pic);
			dxsTeleporter = gDX.dixuCreateSurface(25,25,"teleporter",pic);
			dxsDoor = gDX.dixuCreateSurface(25,25,"door",pic);
			dxsGoblin = gDX.dixuCreateSurface(25,25,"goblin",pic);
			
		}
		
		/// <summary>
		/// Display a message on the screen for a number of seconds
		/// </summary>
		/// <param name="Msg"></param>
		/// <param name="SecCount"></param>
		public static void ShowMessage(string Msg, int SecCount) 
		{
			MessageShowing = true;
			MessageSecCount = SecCount;
			gMessage = Msg;
		}
		
		/// <summary>
		/// Destroy aggregate static objects
		/// </summary>
		public static void CleanUp() 
		{
			oRandom = null;
			gobjParty = null;
			gobjSurfaceMonsters = null;
		}

		/// <summary>
		/// Distribute loot to the party
		/// </summary>
		/// <param name="Loot"></param>
		public static void DistributeLoot(CLoot Loot)
		{
			SuspendPlay = true;
			fLoot f = new fLoot(Loot);
			f.ShowDialog();
			f = null;
			SuspendPlay = false;
		}
		
		/// <summary>
		/// Display a buy/sell dialog
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="TreasureType"></param>
		public static void DisplayStore(string Category, int TreasureType)
		{
			fInputBox f = new fInputBox("Commerce Store", "Type 'Buy' or 'Sell'");
			f.ShowDialog();
			string Result = f.Response;
			f.Close();
			f = null;

			if(Result!="Buy" && Result!="Sell")
				MessageBox.Show("What didn't you understand? Type 'Buy' or 'Sell'.");
			else
			{
				SuspendPlay = true;
				fCommerce fstore = new fCommerce(Category, TreasureType, Result);
				fstore.ShowDialog();
				fstore = null;
				SuspendPlay = false;
			}
		}

		/// <summary>
		/// Validate the surface movement.
		/// </summary>
		/// <param name="Row"></param>
		/// <param name="Col"></param>
		/// <returns></returns>
		public static bool ValidateMove(int Row, int Col) 
		{
			bool bMove = false;

			switch(gintMatrix[Row,Col]) 
			{
				case tMOUNTAIN: //Mountains
					gMoveMessage = "Mountains!";
					break;
				case tTOWN: //Town
				switch(gintOverlay[Row,Col]) 
				{
					case 120: //Craylee
						ShowMessage("Craylee",1);
						QueryEnterEncounter(120);
						break;
				}
					bMove = true;
					break;
				case tCAVE: //Cave
				switch(gintOverlay[Row,Col]) 
				{
					case 121: //Kobold's Lair
						ShowMessage("Kobold's Lair", 1);
						QueryEnterEncounter(121);
						break;
				}
					bMove = true;
					break;
				case tPASS: //Pass
					MessageBox.Show("A wall of deadly force blocks your passage...");
					GroupHit(1,4);
					//play sound
					gMoveMessage = "Arggggggggg!!!";
					PlaySound(Sounds.HIT);
					break;
				case tWALL: //Wall
					gMoveMessage = "Stone Wall!";
					break;
				default:
					bMove = true;
					break;
			}

			return bMove;
		}

		/// <summary>
		/// Determine if the party wants to enter the encounter
		/// </summary>
		/// <param name="EncounterID"></param>
		private static void QueryEnterEncounter(int EncounterID) 
		{
			if(oEncounter!=null) 
				oEncounter = null;
			
			SuspendPlay = true;
			oEncounter = new CEncounter(EncounterID);

			DialogResult iResult = MessageBox.Show("Enter the " + oEncounter.EncounterType +
				" known as " + oEncounter.Name + "?", "Encounter", MessageBoxButtons.YesNo);
			if(iResult == DialogResult.Yes) 
				oEncounter.InitializeEncounter();
			SuspendPlay = false;
		}

		/// <summary>
		/// Ask if the party wants to leave the encounter
		/// </summary>
		public static void QueryLeaveEncounter()
		{
			DialogResult iResult = MessageBox.Show("Leave?", "Encounter", MessageBoxButtons.YesNo);
			if(iResult == DialogResult.Yes) 
				InEncounter = false;
		}


		/// <summary>
		/// Deal a blow to each character
		/// </summary>
		/// <param name="LowerLimit"></param>
		/// <param name="UpperLimit"></param>
		private static void GroupHit(int LowerLimit, int UpperLimit) 
		{
			int iDamage = MyRand(LowerLimit, UpperLimit);

			foreach(CCharacter oCharacter in gobjParty.Characters)
			{
				oCharacter.HitPoints -= iDamage;
			}
		}
		/// <summary>
		/// Randomly generate and randomly place monsters on the overlay grid.
		/// </summary>
		public static void CheckForMonsters() 
		{
			int iChance;
			int randRow;
			int randCol;
			CMonster oMonster;

			iChance = MyRand(10);
			if(iChance==10) 
			{
				randRow = MyRand(MAX_ROWS-1);
				randCol = MyRand(MAX_COLS-1);
				if(gintOverlay[randRow,randCol]==0) 
				{
					oMonster = new CMonster();
					oMonster.LoadMonsterByID(MyRand(3));
					gintOverlay[randRow, randCol] = oMonster.ID;
					oMonster.FightCol = randCol;
					oMonster.FightRow = randRow;
					gobjSurfaceMonsters.Add(oMonster);
				}
			}
		}
		
		/// <summary>
		/// Determines if a surface monster is adjacent to the party.
		/// </summary>
		/// <param name="oMonster"></param>
		/// <returns></returns>
		public static bool IsSurfaceMonsterAdjacent(ref CMonster oMonster) 
		{
			
			mPoint[] adj = new mPoint[9];
			bool bAdjacent = false;

			//Adjacency list for the eight squares around the party:
			//  123
			//  4 5
			//  678

			adj[1].Row = -1;
			adj[1].Col = -1;
			
			adj[2].Row = -1;
			adj[2].Col = 0;

			adj[3].Row = -1;
			adj[3].Col = 1;

			adj[4].Row = 0;
			adj[4].Col = -1;

			adj[5].Row = 0;
			adj[5].Col = 1;

			adj[6].Row = 1;
			adj[6].Col = -1;

			adj[7].Row = 1;
			adj[7].Col = 0;

			adj[8].Row = 1;
			adj[8].Col = 1;

			for(int i=1; i<9; i++) 
				if((gintOverlay[adj[i].Row + gCurRow, adj[i].Col + gCurCol] > 0 &&
					gintOverlay[adj[i].Row + gCurRow, adj[i].Col + gCurCol] < 30) &&
					(adj[i].Row + gCurRow == oMonster.FightRow &&
					adj[i].Col + gCurCol == oMonster.FightCol)) 
				{
					bAdjacent = true;
					break;
				}

			return bAdjacent;

		}

		/// <summary>
		/// Move the monsters if the chars are within a 20 sq radius
		/// </summary>
		public static void MoveSurfaceMonsters() 
		{
			int r,c;

			//Only move monsters within 20 tiles of the party.
			foreach(CMonster oMonster in gobjSurfaceMonsters) 
			{
				r = oMonster.FightRow;
				c = oMonster.FightCol;
				if((((gCurRow - 20 <= r) && (r < gCurRow + 20)) &&
					((gCurCol - 20 <= c) && (c < gCurCol + 20)))) 
				{
					CMonster oMoveMonster = oMonster;
					MoveThisMonster(ref oMoveMonster);
					if(FightMode)
						break;
				}
			}

		}

		/// <summary>
		/// Move a monster on the overlay
		/// </summary>
		/// <param name="oMonster"></param>
		public static void MoveThisMonster(ref CMonster oMonster) 
		{

			int Row = oMonster.FightRow,
				Col = oMonster.FightCol,
				mID = oMonster.ID,
				rmod = 0,
				cmod = 0;
			bool bMoved = false;

			if(gintOverlay[Row, Col]==mID) 
			{
				if(!IsSurfaceMonsterAdjacent(ref oMonster)) 
				{
					int r = oMonster.FightRow,
						c = oMonster.FightCol;
					if(r<gCurRow) //Chars are below us
						rmod = 1;
					if(r==gCurRow)
						rmod = 0;
					if(r>gCurRow)
						rmod = -1;
					if(c<gCurCol)
						cmod = 1;
					if(c==gCurCol)
						cmod = 0;
					if(c>gCurCol)
						cmod = -1;

					if(gintOverlay[r+rmod, c+cmod] == 0) //Diagonal move
					{
						gintOverlay[r,c]=0;
						gintOverlay[r+rmod, c+cmod] = oMonster.ID;
						oMonster.FightRow = r+rmod;
						oMonster.FightCol = c+cmod;
						bMoved = true;
					}
					if(!bMoved) {
						if(gintOverlay[r+rmod, c] == 0) //Row Move
						{
							gintOverlay[r,c]=0;
							gintOverlay[r+rmod, c] = oMonster.ID;
							oMonster.FightRow = r+rmod;
							oMonster.FightCol = c;
							bMoved = true;
						}
					}

					if(!bMoved) 
					{
						if(gintOverlay[r, c+cmod] == 0) //Col move
						{
							gintOverlay[r,c]=0;
							gintOverlay[r, c+cmod] = oMonster.ID;
							oMonster.FightRow = r;
							oMonster.FightCol = c+cmod;
						}
					}

					
				} //Check for adjacency
				else
				{
					SuspendPlay = true;
					if(oCombat==null)
						oCombat = new CCombat(oMonster.ID, 4, 8, CCombat.Locale.OUTSIDE,
							Row, Col, -1, -1);
					else
					{
						oCombat = null;
						oCombat = new CCombat(oMonster.ID, 4, 8, CCombat.Locale.OUTSIDE,
							Row, Col, -1, -1);
					}
					SuspendPlay = false;
				}
			} //Check for sync
		}
		
		/// <summary>
		/// Load a saved game from the database
		/// </summary>
		public static void LoadSavedGame(int GameNumber)
		{
			//Reset all storage objects and arrays and repopulate
			FightMode = false;

			gobjParty = null;
			gobjSurfaceMonsters = null;
			gintOverlay = null;
			gintMatrix = null;

			gintOverlay = new int[MAX_COLS, MAX_ROWS];
			gintMatrix = new int[MAX_COLS, MAX_ROWS];

			//Create global variables
			gobjParty = new CParty();
			gobjParty.LoadParty(GameNumber);

			gCurRow = gobjParty.Row;
			gCurCol = gobjParty.Col;

			gobjSurfaceMonsters = new CMonsters();
			LoadSurfaceMonsters();
			LoadMatrix();

		}

		/// <summary>
		/// Restore the game from the saved tables
		/// </summary>
		public static void RestoreCharacters()
		{
			CDataAccess oDataAccess = new CDataAccess();

			oDataAccess.ExecuteStatement("DELETE * FROM CharData");

			oDataAccess.ExecuteStatement("INSERT INTO CharData SELECT * FROM SaveCharData");

			oDataAccess.ExecuteStatement("DELETE * FROM Inventory");

			oDataAccess.ExecuteStatement("INSERT INTO Inventory SELECT * FROM SaveInventory");

			oDataAccess.ExecuteStatement("DELETE * FROM Overlay");

			oDataAccess.ExecuteStatement("INSERT INTO Overlay SELECT * FROM SaveOverlay");

			oDataAccess = null;

			LoadSavedGame(GameID);
		}

		/// <summary>
		/// Save character data and overlay
		/// </summary>
		public static void SaveGame()
		{
			string sSQL;
			CDataAccess oDataAccess = new CDataAccess();

			//Save Current Row and Column
			sSQL = "UPDATE SavedGame SET CurRow = " + gCurRow.ToString() + ", CurCol = " +
				gCurCol.ToString() + " WHERE ID = " + GameID.ToString();
			oDataAccess.ExecuteStatement(sSQL);

			//save the overlay
			oDataAccess.ExecuteStatement("DELETE * FROM Overlay");
			
			for(int r=0; r<gintOverlay.GetUpperBound(0); r++) 
			{
				for(int c=0; c<gintOverlay.GetUpperBound(1); c++)
				{
					if(gintOverlay[r,c] > 0 && gintOverlay[r,c] != 99) 
					{
						sSQL = "INSERT INTO Overlay VALUES (" + r.ToString() +
							", " + c.ToString() + ", " + gintOverlay[r,c].ToString() +
							")";
						oDataAccess.ExecuteStatement(sSQL);
					}
				}
			}

			//save character related information
			foreach(CCharacter oCharacter in gobjParty.Characters)
			{
				//save attributes
				sSQL = "UPDATE CharData SET HP = " + oCharacter.HitPoints.ToString() +
					", MaxHP = " + oCharacter.MaxHitPoints.ToString() +
					", Level = " + oCharacter.Level.ToString() +
					", Gold = " + oCharacter.Gold.ToString() +
					", Experience = " + oCharacter.Experience.ToString() +
					", ST = " + oCharacter.Strength.ToString() +
					", CH = " + oCharacter.Charisma.ToString() +
					", CO = " + oCharacter.Constitution.ToString() +
					", IN = " + oCharacter.Intelligence.ToString() +
					", DX = " + oCharacter.Dexterity.ToString() +
					", WI = " + oCharacter.Wisdom.ToString() +
					", Wisdom_ID = " + oCharacter.WeaponID.ToString() +
					", AC = " + oCharacter.ArmorClass.ToString() +
					", Class = " + oCharacter.ClassID.ToString();

				oDataAccess.ExecuteStatement(sSQL);

				//save spells
				sSQL = "DELETE * FROM MemorizedSpells WHERE CharID = " + oCharacter.ID.ToString();
				oDataAccess.ExecuteStatement(sSQL);

				foreach(CSpell oSpell in oCharacter.Spells) 
				{
					sSQL = "INSERT Into MemorizedSpells (CharID, SpellID, Count) Values (" +
						oCharacter.ID.ToString() + ", " + oSpell.ID.ToString() + ", " +
						oSpell.Count.ToString() + ")";
					oDataAccess.ExecuteStatement(sSQL);
				}

				//save inventory
				sSQL = "DELETE * FROM Inventory WHERE CharData_ID = " + oCharacter.ID.ToString();
				oDataAccess.ExecuteStatement(sSQL);

				foreach(CItem oItem in oCharacter.Inventory)
				{
                    sSQL = "INSERT INTO Inventory (CharData_ID, Charges, InUse, Item_ID) " +
						"VALUES (" + oCharacter.ID.ToString() + ", " + oItem.Charges.ToString() +
						", " + (oItem.InUse ? "-1" : "0") + oItem.ID.ToString() + ")";
					oDataAccess.ExecuteStatement(sSQL);
				}
			}//save character data
			
			oDataAccess = null;
		}

		/// <summary>
		/// Save character data
		/// </summary>
		public static void SaveCharacters() 
		{
			//Save character stats, spells, inventory, overlay
			SaveGame();

			CDataAccess oDataAccess = new CDataAccess();

			oDataAccess.ExecuteStatement("DELETE * FROM SaveInventory");

			oDataAccess.ExecuteStatement("INSERT INTO SaveInventory SELECT * FROM Inventory");

			oDataAccess.ExecuteStatement("DELETE * FROM SaveCharData");

			oDataAccess.ExecuteStatement("INSERT INTO SaveCharData SELECT * FROM CharData");

			oDataAccess.ExecuteStatement("DELETE * FROM SaveOverlay");

			oDataAccess.ExecuteStatement("INSERT INTO SaveOverlay SELECT * FROM Overlay");

			oDataAccess = null;
		}

		/// <summary>
		/// Main entry point for program
		/// </summary>
		public static void Main() 
		{

			//Load Saved Game
			LoadSavedGame(GameID);

			//Create the Direct X objects
			gDX = new CDirectx();

			//Start the random number generator
			oRandom = new Random();

			//Set some of the program globals
			PromptForSpell = false;
			PromptForWho = false;
			SpellPending = false;
			InEncounter = false;
			FightMode = false;
			SuspendPlay = false;
			g_dixuAppEnd = false;

			//Show the splash screen and menu
			fMenu Menu = new fMenu();
			Menu.ShowDialog();

			Application.Run(new fEcalpon());
		}
	}
}