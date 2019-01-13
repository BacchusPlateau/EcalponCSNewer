using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Ecalpon
{
	/// <summary>
	/// Special sub adventures, i.e. dungeons, towns, space
	/// </summary>
	public class CEncounter
	{
		public int[,] Matrix;
		public int[,] Overlay;
		private CMonsters oMonsters;
		private string sName;
		private int iID;
		private string sEncounterType;
		private int iRows;
		private int iCols;
		private int iCurRow;
		private int iCurCol;
		private int ibgrnd;
		private int irndbg;		

		//accessors
		public CMonsters Monsters 
		{
			get
			{
				return oMonsters;
			}
		}

		public string EncounterType
		{
			get
			{
				return sEncounterType;
			}
		}


		public string Name 
		{
			get
			{
				return sName;
			}
		}

		public int ID 
		{
			get
			{
				return iID;
			}
		}

		public int Rows
		{
			get
			{
				return iRows;
			}
		}

		public int Cols
		{
			get
			{
				return iCols;
			}
		}

		public int CurRow 
		{
			get
			{
				return iCurRow;
			}
			set
			{
				iCurRow = value;
			}
		}

		public int CurCol
		{
			get
			{
				return iCurCol;
			}
			set
			{
				iCurCol = value;
			}
		}

		public int bgrnd
		{
			get 
			{
				return ibgrnd;
			}
		}

		public int rndbg
		{
			get
			{
				return ibgrnd;
			}
		}

		/// <summary>
		/// Validate a movement direction inside the encounter
		/// </summary>
		/// <param name="r"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		public bool ValidateMove(int r, int c) 
		{
			bool bMoveOK = false;

			switch(Matrix[r,c])
			{
				case 1: //stonewall
					CEcalpon.gMoveMessage = "Wall!";
					break;
				case 4: //vertical barrier
					CEcalpon.gMoveMessage = "Barrier!";
					break;
				case 9: case 15: case 16: case 17: case 18: case 20: case 21: case 22:
				case 24: case 25: case 26: case 27: case 28: case 29: case 30:
					CEcalpon.gMoveMessage = "Rock!";
					break;
				default:
					bMoveOK = true;
					break;
			}

			if(Overlay[r,c] > 50 && Overlay[r,c] != 99) 
			{
				ProcessHook(Overlay[r,c], r, c);
				bMoveOK = true;
			}
			
			return bMoveOK;

		}

		/// <summary>
		/// A 'hook' is an event somewhere within an encounter.  The event can be
		/// a fight, information, treasure, etc.
		/// </summary>
		/// <param name="HookID"></param>
		/// <param name="r"></param>
		/// <param name="c"></param>
		public void ProcessHook(int HookID, int r, int c) 
		{
			OleDbDataReader drReader = null;
			CDataAccess oDataAccess = new CDataAccess();
			string sSQL, hType = "";
			bool bRunOnce = false;
			int iTreasureType=-1;

			sSQL = "SELECT hType, RunOnce FROM hooks WHERE ID = " + HookID.ToString();

			oDataAccess.FillDataReader(out drReader, sSQL);

			try
			{
				drReader.Read();
				hType = drReader["hType"].ToString();
				bRunOnce = ((int)drReader["RunOnce"]) != 0 ? true : false;			
			}
			catch(Exception e)
			{
				MessageBox.Show(e.ToString());
			}
			drReader = null;		

			switch(hType)
			{
				case "HookInfo" :
					sSQL = "SELECT Info FROM HookInfo WHERE ID = " + HookID.ToString();
					string sInfo = "";

					try
					{
						drReader.Read();
						sInfo = drReader["hType"].ToString();
					}
					catch(Exception e)
					{
						MessageBox.Show(e.ToString());
					}
					drReader = null;
					MessageBox.Show(sInfo, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
				case "HookFight" :
					sSQL = "SELECT * FROM HookFight WHERE ID = " + HookID.ToString();
					int
						iMonsterID = -1,
						iMaxMonster = -1,
						iMinMonster = -1;

					try
					{
						drReader.Read();
						iMonsterID = (int)drReader["MonsterID"];
						iMaxMonster = (int)drReader["MaxMonster"];
						iMinMonster = (int)drReader["MinMonster"];
					}
					catch(Exception e)
					{
						MessageBox.Show(e.ToString());
					}
					drReader = null;
					CEcalpon.oCombat = new CCombat(iMonsterID, iMinMonster, iMaxMonster,
						CCombat.Locale.ENCOUNTER, 0, 0, bgrnd, rndbg);

					break;
				case "HookLoot" : 
					sSQL = "SELECT * FROM HookLoot WHERE ID = " + HookID.ToString();
					
					try
					{
						drReader.Read();
						iTreasureType = (int)drReader["TreasureType"];
					}
					catch(Exception e)
					{
						MessageBox.Show(e.ToString());
					}
					drReader = null;
					CLoot oLoot = new CLoot(iTreasureType);
					CEcalpon.DistributeLoot(oLoot);
					break;
				case "HookCommerce" :
					sSQL = "SELECT * FROM HookCommerce WHERE ID = " + HookID.ToString();
					string sCategory = "";
					try
					{
						drReader.Read();
						iTreasureType = (int)drReader["TreasureType"];
						sCategory = drReader["Category"].ToString();
					}
					catch(Exception e)
					{
						MessageBox.Show(e.ToString());
					}
					drReader = null;
					CEcalpon.DisplayStore(sCategory, iTreasureType);
					break;
				case "HookTeleport" : //Bahahahhahah!
					sSQL = "SELECT Row, Col FROM HookTeleport WHERE ID = " + HookID.ToString();
					try
					{
						drReader.Read();
						CurRow = (int)drReader["Row"];
						CurCol = (int)drReader["Col"];
					}
					catch(Exception e)
					{
						MessageBox.Show(e.ToString());
					}
					drReader = null;
					CEcalpon.ShowMessage("Poof!", 2);
					CEcalpon.PlaySound(CEcalpon.Sounds.HIT);
					break;
			}

			oDataAccess = null;

			if(bRunOnce)
				Overlay[r,c] = 0;

		}


		/// <summary>
		/// Setup data structures used in the encounter
		/// </summary>
		public void InitializeEncounter()
		{
			string sSQL;
			int
				iMapID = -1,
				iOverlayID = -1,
				iData,
				iRow,
				iCol;

			CMonster oMonster = null;
			
			OleDbDataReader drReader = null;
			CDataAccess oDataAccess = new CDataAccess();
		
			//Get dimensions of the encounter
			sSQL = "SELECT m.[Rows], m.[Cols], m.ID, e.OverlayID FROM MapMeta m, " +
				"Encounter e WHERE e.MapID = m.ID AND e.ID = " + ID.ToString();

			oDataAccess.FillDataReader(out drReader, sSQL);

			try
			{
				drReader.Read();
				iRows = (System.Int16)drReader["Rows"]+1;
				iCols = (System.Int16)drReader["Cols"]+1;
				iMapID = (System.Int16)drReader["ID"];
				iOverlayID = (int)drReader["OverlayID"];
				drReader = null;
			}
			catch(Exception e)
			{
				MessageBox.Show(e.ToString());
			}

			//Load the encounter matrix
			Matrix = new int[Rows,Cols];
			Overlay = new int[Rows,Cols];
			oMonsters = new CMonsters();

			sSQL = "SELECT * FROM MapData WHERE ID = " + iMapID.ToString();

			oDataAccess = null;
			oDataAccess = new CDataAccess();
			oDataAccess.FillDataReader(out drReader, sSQL);

			try
			{
				while(drReader.Read()) 
				{
					Matrix[(System.Int16)drReader["Row"]+1, (System.Int16)drReader["col"]+1] = (System.Int16)drReader["Data"];
					switch((System.Int16)drReader["Data"])
					{
						//mark overlay tiles as impassable
						case 1: case 4: case 6: case 9: case 15: case 16: case 17:
						case 18: case 20: case 21: case 22: case 24: case 25: case 26:
						case 27: case 28: case 29: case 30: case 32:
							Overlay[(System.Int16)drReader["Row"]+1, (System.Int16)drReader["col"]+1] = 99;
							break;
					}
				}
				drReader = null;
			}
			catch(Exception e)
			{
				MessageBox.Show(e.ToString());
			}

			//load the encounter overlay
			sSQL = "SELECT * FROM MapData WHERE ID = " + iOverlayID.ToString();
			
			oDataAccess = null;
			oDataAccess = new CDataAccess();
			oDataAccess.FillDataReader(out drReader, sSQL);
			try
			{
				while(drReader.Read()) 
				{
					iRow = (System.Int16)drReader["Row"];
					iCol = (System.Int16)drReader["col"];
					iData = (System.Int16)drReader["Data"];
					
					//only set the overlay if there is nothing there
					if(Overlay[iRow+1,iCol+1]==0)
                        Overlay[iRow+1,iCol+1] = iData;

					//Create monsters
					if(iData > 0 && iData < 30) 
					{
						oMonster = new CMonster();
						oMonster.LoadMonsterByID(iData);
						oMonster.FightCol = iCol+1;
						oMonster.FightRow = iRow+1;
						oMonsters.Add(oMonster);
					}

				}
			
				drReader = null;
			}
				catch(Exception e)
			{
				MessageBox.Show(e.ToString());
			}

			oDataAccess = null;

			CEcalpon.UsingGem = false;
			CEcalpon.InEncounter = true;
		}

		/// <summary>
		/// Move the monsters within the encounter that are in a 20 tile radius
		/// </summary>
		public void MoveEncounterMonsters() 
		{
			int r,c;
			CMonster oMoveThisMonster;

			foreach(CMonster oMonster in oMonsters)
			{
				r = oMonster.FightRow;
				c = oMonster.FightCol;
				if((((CurRow - 20) <= r) && (r < (CurRow + 20))) && 
					(((CurRow - 20) <= c) && (c < (CurCol + 20))))
				{
					oMoveThisMonster = oMonster;
					MoveEncounterMonster(oMoveThisMonster);
					if(CEcalpon.FightMode)
						return;
				}
			}
		}

		/// <summary>
		/// Move a monster on the overlay
		/// </summary>
		/// <param name="oMonster"></param>
		private void MoveEncounterMonster(CMonster oMonster)
		{
			int
				Row=0,
				Col=0,
				r=0,
				c=0,
				rmod=0,
				cmod=0,
				mID=0;

			bool
				bMoved = false,
				bModFound = false;

			Row = oMonster.FightRow;
			Col = oMonster.FightCol;
			mID = oMonster.ID;

			//Check to see if overlay is in sync
			if(Overlay[Row,Col] != mID)
				return;

			if(!IsEncounterMonsterAdjacent(oMonster))
			{
				r = oMonster.FightRow;
				c = oMonster.FightCol;

				//Determine the vertical movement
				if(r < CurRow) //chars are below us
				{
					rmod = 1;
					bModFound = true;
				}
				if(r == CurRow && !bModFound) //chars are on our row
				{
					rmod = 0;
					bModFound = true;
				}
				if(r > CurRow && !bModFound) //chars are above us
				{
					rmod = -1;
				}

				//Determine the horizontal movement
				bModFound = false;
				if(c < CurCol) //chars are to the right of us
				{
					cmod = 1;
					bModFound = true;
				}
				if(c == CurCol && !bModFound) //chars are on our col
				{
					cmod = 0;
					bModFound = true;
				}
				if(c > CurCol && !bModFound) //chars are to the left of us
				{
					cmod = -1;
				}

				//perform the move
				if(Overlay[r + rmod, c + cmod] == 0) //diagonal move
				{
					Overlay[r, c] = 0;
					Overlay[r + rmod, c + cmod] = oMonster.ID;
					oMonster.FightRow = r + rmod;
					oMonster.FightCol = c + cmod;
					bMoved = true;
				}
				if(!bMoved && Overlay[r + rmod, c] == 0) //row move
				{
					Overlay[r, c] = 0;
					Overlay[r + rmod, c + cmod] = oMonster.ID;
					oMonster.FightRow = r + rmod;
					oMonster.FightCol = c;
					bMoved = true;
				}
				if(!bMoved && Overlay[r, c + cmod] == 0) //col move
				{
					Overlay[r, c] = 0;
					Overlay[r + rmod, c + cmod] = oMonster.ID;
					oMonster.FightRow = r;
					oMonster.FightCol = c + cmod;
				}
			}
			else
			{
				//Initialize a conflict!!!
				CEcalpon.SuspendPlay = true;
				CEcalpon.oCombat = null;
				CEcalpon.oCombat = new CCombat(oMonster.ID, 4, 8, CCombat.Locale.ENCOUNTER,
					Row, Col, this.bgrnd, this.rndbg);
				CEcalpon.SuspendPlay = false;
			}

		}

		/// <summary>
		/// Determines if a monster is adjacent to the characters
		/// </summary>
		/// <param name="oMonster"></param>
		/// <returns></returns>
		private bool IsEncounterMonsterAdjacent(CMonster oMonster)
		{
			bool
				bIsAdjacent = false;

			CEcalpon.mPoint[] adj = new CEcalpon.mPoint[9];
			
			//Adjacency list for the eight squares around the party:
			//   123
			//   4 5
			//   678

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
				if(((adj[i].Row + CurRow) <= Rows) && ((adj[i].Col + CurCol) <= Cols))
					if((Overlay[adj[i].Row + CurRow, adj[i].Col + CurCol] > 0 &&
						Overlay[adj[i].Row + CurRow, adj[i].Col + CurCol] < 30) &&
						(adj[i].Row + CurRow == oMonster.FightRow &&
						adj[i].Col + CurCol == oMonster.FightCol)) 
					{
						bIsAdjacent = true;
						break;
					}

			return bIsAdjacent;

		}

		/// <summary>
		/// After a battle, remove the monster from the overlay
		/// </summary>
		public void RemoveEncounterMonster() 
		{
			foreach(CMonster oMonster in oMonsters) 
			{
				if(oMonster.FightRow==CEcalpon.oCombat.FightRow && oMonster.FightCol==CEcalpon.oCombat.FightCol) 
				{
					oMonsters.Delete(oMonster.InternalIndex);
					break;
				}
			}

			Overlay[CEcalpon.oCombat.FightRow, CEcalpon.oCombat.FightCol] = 0;

		}

		/// <summary>
		/// Constructor that does some preliminary loading of data
		/// </summary>
		/// <param name="EncounterID"></param>
		public CEncounter(int EncounterID)
		{
			string sSQL;
			OleDbDataReader drReader = null;
			CDataAccess oDataAccess = new CDataAccess();

			sSQL = "SELECT * FROM Encounter WHERE ID = " + EncounterID.ToString();

			oDataAccess.FillDataReader(out drReader, sSQL);

			try 
			{
				drReader.Read();

				sName = drReader["Name"].ToString();
				iID = EncounterID;
				sEncounterType = drReader["eType"].ToString();
				iCurRow = (int)drReader["StartRow"];
				iCurCol = (int)drReader["StartCol"];
				ibgrnd = (int)drReader["BackGround"];
				irndbg = (int)drReader["RndBackGround"];

			}
			catch(Exception e)
			{
				MessageBox.Show(e.ToString());
			}
			oDataAccess = null;
		}
	}
}
