using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections;

public class CMonsters : CollectionBase
{

	public CMonster Item(int Index) 
	{
		return (CMonster) List[Index];
	}

	public void Delete(int Index) 
	{
		if (Index > Count - 1 || Index < 0)
		{
			System.Windows.Forms.MessageBox.Show("Index not valid!");
			return;
		}
		List.RemoveAt(Index);

		//Reindex
		CMonster oMonster=null;
		for(int i=0; i<List.Count; i++) 
		{
			oMonster = (CMonster) List[i];
			oMonster.InternalIndex = i;
		}
	}
	
	public void Add(CMonster NewMonster) 
	{
		NewMonster.InternalIndex = List.Count;
		List.Add(NewMonster);
	}

	public void LoadMonstersForCombat(int NumberOfMonsters, int MonsterID) 
	{
	
		string sSQL;
		OleDbDataReader drReader;
		CDataAccess oDataAccess = new CDataAccess();
		CMonster oMonster;
		

		sSQL = "SELECT * FROM Monster WHERE ID = " + MonsterID.ToString();

		try 
		{
			oDataAccess.FillDataReader(out drReader, sSQL);

			drReader.Read();
			
			for(int i=0;  i<NumberOfMonsters; i++) 
			{
				oMonster = new CMonster();

				oMonster.Name = drReader["Name"].ToString();
				oMonster.ID = (System.Int16) drReader["ID"];
				oMonster.HitPoints = (System.Int16) drReader["HP"];
				oMonster.MaxHitPoints = oMonster.HitPoints;
				oMonster.Level = (System.Int16) drReader["Level"];
				oMonster.ExperienceValue = (System.Int16) drReader["ExpValue"];
				oMonster.Strength = 15;
				oMonster.Charisma = 15;
				oMonster.Charisma = 15;
				oMonster.Intelligence = 15;
				oMonster.Dexterity = 15;
				oMonster.Wisdom = 15;
				oMonster.WeaponID = (System.Int16) drReader["Weapon_ID"];
				oMonster.ArmorClass = (System.Int16) drReader["AC"];
				switch(i) 
				{
					case 0:
						oMonster.FightRow = 4;
						oMonster.FightCol = 8;
						break;
					case 1:
						oMonster.FightRow = 4;
						oMonster.FightCol = 10;
						break;
					case 2:
						oMonster.FightRow = 3;
						oMonster.FightCol = 7;
						break;
					case 3:
						oMonster.FightRow = 3;
						oMonster.FightCol = 11;
						break;
					case 4:
						oMonster.FightRow = 3;
						oMonster.FightCol = 9;
						break;
					case 5:
						oMonster.FightRow = 2;
						oMonster.FightCol = 9;
						break;
					case 6:
						oMonster.FightRow = 2;
						oMonster.FightCol = 6;
						break;
					case 7:
						oMonster.FightRow = 2;
						oMonster.FightCol = 12;
						break;
				}
				oMonster.DamageModifier = (System.Int16) drReader["DamageModifier"];
				oMonster.TreasureType = (int) drReader["TreasureType"];
				oMonster.ComputeAC();
				
				this.InnerList.Add(oMonster);

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

