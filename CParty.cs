using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections;

public class CParty
{
	private const int CHARACTER_MAX = 5;
	private ArrayList oCharacters;
	private int iRow;
	private int iCol;

	public CParty()
	{
		oCharacters = new ArrayList();
	}


	public int AverageLevel 
	{
		get 
		{
			int LevelSum=0;
			foreach(CCharacter oCharacter in oCharacters)
				LevelSum += oCharacter.Level;
			return (int)(LevelSum/oCharacters.Count);
		}
	}

	private int LevelCheck (int Experience, int ClassID) 
	{
		string sSQL = "";
		OleDbDataReader drReader;
		CDataAccess oDataAccess = new CDataAccess();
		int iLevel = 0;

		switch(ClassID) 
		{
			case (int)CCharacter.eClass.Cleric:
				sSQL = "SELECT ExpLevel FROM ExpCleric WHERE ([Lower] < " + Experience.ToString() + ") AND ([Higher] > " + Experience.ToString() + ")";
				break;
			case (int)CCharacter.eClass.Fighter:
				sSQL = "SELECT ExpLevel FROM ExpFighter WHERE ([Lower] < " + Experience.ToString() + ") AND ([Higher] > " + Experience.ToString() + ")";
				break;
			case (int)CCharacter.eClass.Wizard:
				sSQL = "SELECT ExpLevel FROM ExpMage WHERE ([Lower] < " + Experience.ToString() + ") AND ([Higher] > " + Experience.ToString() + ")";
				break;
		}
		
		try 
		{
			oDataAccess.FillDataReader(out drReader, sSQL);
			drReader.Read();
			iLevel = (System.Int32) drReader["ExpLevel"];
		}
		catch(Exception e)
		{
			MessageBox.Show(e.ToString());
		}

		return iLevel;
	}

	public void CheckForLevelIncreases() 
	{
		int iLevelCheck = 0;
		Random oRandom = new Random();

		foreach(CCharacter oCharacter in oCharacters)
		{
			iLevelCheck = LevelCheck(oCharacter.Experience, oCharacter.ClassID);
			if(iLevelCheck > oCharacter.Level)
			{
				//add hitpoints
				for(int i=oCharacter.Level; i<iLevelCheck+1; i++)
				{
					switch(oCharacter.ClassID)
					{
						case (int)CCharacter.eClass.Fighter:
							oCharacter.MaxHitPoints += oRandom.Next(1,11);
							break;
						case (int)CCharacter.eClass.Cleric:
							oCharacter.MaxHitPoints += oRandom.Next(1,9);
							break;
						case (int)CCharacter.eClass.Wizard:
							oCharacter.MaxHitPoints += oRandom.Next(1,7);
							break;
					}
				}
				oCharacter.Level = iLevelCheck;
			}
		}

	}

	public CCharacter FightFindCharacter(int Row, int Col) 
	{
		CCharacter oFightChar = null;

		foreach(CCharacter oCharacter in oCharacters)
			if(oCharacter.FightCol == Col && oCharacter.FightRow == Row) 
			{
				oFightChar = oCharacter;
				break;
			}

		return oFightChar;

	}

	/// <summary>
	/// Combat function to iterate through move order to find the next
	/// conscious character.
	/// </summary>
	/// <param name="Start"></param>
	/// <returns></returns>
	public int FindNextConsciousChar(int Start) 
	{
		int iOrder = 0;
		bool bFound = false;

		for(int i=Start; i<6; i++) 
		{
			foreach(CCharacter oCharacter in oCharacters) 
			{
				if(oCharacter.Order==i && oCharacter.HitPoints>0) 
				{
					iOrder = oCharacter.Order;	
					bFound = true;
					break;
				}
			}
			if(bFound)
				break;
		}
		return iOrder;
	}




	public int Row 
	{
		get 
		{
			return iRow;
		} 
		set 
		{
			  iRow = value; 
		}
	}

	public int Col 
	{
		get
		{
			return iCol;
		}
		set 
		{
			iCol = value;
		}
	}

	public ArrayList Characters 
	{
		get
		{
			return oCharacters;
		}
	}

	public int Count 
	{
		get 
		{
			return oCharacters.Count;
		}
	}


	public int GetCharacterIDByOrder(int Order) 
	{

		int iCharacterID = -1;

		foreach(CCharacter oCharacter in oCharacters) 
		{
			if(oCharacter.Order == Order) 
			{
				iCharacterID = oCharacter.ID;
				break;
			}
		}

		return iCharacterID;
	}

	public CCharacter GetCharacterByOrder(int Order) 
	{

		CCharacter oCharReturn = null;

		foreach(CCharacter oCharacter in oCharacters) 
		{
			if(oCharacter.Order == Order) 
			{
				oCharReturn = oCharacter;
				break;
			}
		}

		return oCharReturn;
	}

	public CCharacter GetCharacterByInternalIndex(int Index) 
	{
		CCharacter oCharacter = null;

		foreach(CCharacter oCharLoop in oCharacters) 
		{
			if(oCharLoop.InternalIndex == Index) 
			{
				oCharacter = oCharLoop;
				break;
			}
		}

		return oCharacter;
	}

	public CCharacter GetCharacterByID(int ID) 
	{
		CCharacter oCharacter = null;

		foreach(CCharacter oCharLoop in oCharacters) 
		{
			if(oCharLoop.ID == ID) 
			{
				oCharacter = oCharLoop;
				break;
			}
		}

		return oCharacter;
	}

	public void LoadParty(int ID) 
	{

		string sSQL;
		OleDbDataReader drReader;
		CDataAccess oDataAccess = new CDataAccess();
		CCharacter oCharacter;

		sSQL = "SELECT * FROM SavedGame WHERE ID = " + ID.ToString();

		try 
		{
			oDataAccess.FillDataReader(out drReader, sSQL);

			drReader.Read();
			
			iRow = (System.Int16) drReader["CurRow"];
			iCol = (System.Int16) drReader["CurCol"];

			if((System.Int16) drReader["CharData_ID_1"] > -1) 
			{
				oCharacter = new CCharacter();
				oCharacter.LoadCharacterByID((System.Int16) drReader["CharData_ID_1"]);
				oCharacter.Order = 1;
				oCharacter.InternalIndex = oCharacters.Count;
				oCharacters.Add(oCharacter);
			}

			if((System.Int16) drReader["CharData_ID_2"] > -1) 
			{
				oCharacter = new CCharacter();
				oCharacter.LoadCharacterByID((System.Int16) drReader["CharData_ID_2"]);
				oCharacter.Order = 2;
				oCharacter.InternalIndex = oCharacters.Count;
				oCharacters.Add(oCharacter);
			}

			if((System.Int16) drReader["CharData_ID_3"] > -1) 
			{
				oCharacter = new CCharacter();
				oCharacter.LoadCharacterByID((System.Int16) drReader["CharData_ID_3"]);
				oCharacter.Order = 3;
				oCharacter.InternalIndex = oCharacters.Count;
				oCharacters.Add(oCharacter);
			}

			if((System.Int16) drReader["CharData_ID_4"] > -1) 
			{
				oCharacter = new CCharacter();
				oCharacter.LoadCharacterByID((System.Int16) drReader["CharData_ID_4"]);
				oCharacter.Order = 4;
				oCharacter.InternalIndex = oCharacters.Count;
				oCharacters.Add(oCharacter);
			}

			if((System.Int16) drReader["CharData_ID_5"] > -1) 
			{
				oCharacter = new CCharacter();
				oCharacter.LoadCharacterByID((System.Int16) drReader["CharData_ID_5"]);
				oCharacter.Order = 5;
				oCharacter.InternalIndex = oCharacters.Count;
				oCharacters.Add(oCharacter);
			}

			drReader.Close();
			oDataAccess = null;
		}
			
		catch(Exception e)
		{
			MessageBox.Show(e.ToString());
		}


	}
}
