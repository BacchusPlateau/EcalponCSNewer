using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

public class CSpells : CollectionBase
{
	
	public void Delete(int Index) 
	{
		if (Index > Count - 1 || Index < 0)
		{
			System.Windows.Forms.MessageBox.Show("Index not valid!");
			return;
		}
		List.RemoveAt(Index);

		//Reindex
		CSpell oSpell=null;
		for(int i=0; i<List.Count; i++) 
		{
			oSpell = (CSpell) List[i];
			oSpell.InternalIndex = i;
		}

	}
	
	public void Add(CSpell NewSpell) 
	{
		NewSpell.InternalIndex = List.Count;
		List.Add(NewSpell);
	}

	public CSpell Item(int Index) 
	{
		return (CSpell) List[Index];

	}

	public void LoadAvailableSpells(int CharacterClass, int CharacterLevel)
	{
		string sSQL;
		OleDbDataReader drReader;
		CDataAccess oDataAccess = new CDataAccess();
		CSpell oSpell;

		sSQL = "SELECT * FROM SPELLS WHERE Class = " + CharacterClass.ToString() +
			" AND Spells.Level <= " + CharacterLevel.ToString() + " ORDER BY " +
			" Spells.Level, Spells.Name";

		try 
		{
			oDataAccess.FillDataReader(out drReader, sSQL);

			while(drReader.Read()) 
			{
				oSpell = new CSpell();
				oSpell.ID = (int) drReader["ID"];
				oSpell.Count = 0;  //when displaying available spells, count doesn't apply
				oSpell.Name = drReader["Name"].ToString();
				oSpell.SpellType = drReader["Type"].ToString();
				oSpell.InternalIndex = List.Count;
				oSpell.Modifier = (int) drReader["Modifier"];
				oSpell.SpellClass = (int) drReader["Class"];
				oSpell.Level = (int) drReader["Level"];
				oSpell.GroupSpell = ((int) drReader["Group"]) == 0 ? false : true;
				oSpell.Attribute = drReader["Attribute"].ToString();
				List.Add(oSpell);
			}

			drReader.Close();
			oDataAccess = null;
		}
			
		catch(Exception e)
		{
			MessageBox.Show(e.ToString());
		}

	}

	public void LoadSpellsByCharacterID(int ID) 
	{
		
		string sSQL;
		OleDbDataReader drReader;
		CDataAccess oDataAccess = new CDataAccess();
		CSpell oSpell;

		sSQL = "SELECT ms.SpellID, ms.Count, s.Name, " +
			"s.[Type], s.Modifier, s.Class, s.Level, s.Group, s.Attribute " + 
			"FROM MemorizedSpells ms, Spells s WHERE ms.SpellID = s.ID " +
			"AND ms.CharID = " + ID.ToString();

		try 
		{
			oDataAccess.FillDataReader(out drReader, sSQL);

			while(drReader.Read()) 
			{
				oSpell = new CSpell();
				oSpell.ID = (int) drReader["SpellID"];
				oSpell.Count = (int) drReader["Count"];
				oSpell.Name = drReader["Name"].ToString();
				oSpell.SpellType = drReader["Type"].ToString();
				oSpell.InternalIndex = List.Count;
				oSpell.Modifier = (int) drReader["Modifier"];
				oSpell.SpellClass = (int) drReader["Class"];
				oSpell.Level = (int) drReader["Level"];
				oSpell.GroupSpell = ((int) drReader["Group"]) == 0 ? false : true;
				oSpell.Attribute = drReader["Attribute"].ToString();
				List.Add(oSpell);
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

